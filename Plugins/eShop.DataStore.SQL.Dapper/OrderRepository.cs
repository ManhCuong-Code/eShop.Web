using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using eShop.CoreBusiness.Models;
using eShop.UseCases.PluginInterfaces.DataStore;

namespace eShop.DataStore.SQL.Dapper
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDataAccess _dataAccess;

        public OrderRepository(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public int CreateOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            using IDbConnection connection = new SqlConnection(_dataAccess.ConnectionString);
            connection.Open();
            using IDbTransaction transaction = connection.BeginTransaction();

            try
            {
                order.DatePlaced = DateTime.Now;
                if (string.IsNullOrEmpty(order.UniqueId))
                    order.UniqueId = Guid.NewGuid().ToString();

                string sqlOrder = @"
                    INSERT INTO [dbo].[Order] 
                    (
                        [DatePlaced], [DateProcessing], [DateProcessed],
                        [CustomerName], [CustomerAddress], [CustomerCity],
                        [CustomerStateProvince], [CustomerCountry], [AdminUser], [UniqueId]
                    )
                    VALUES 
                    (
                        @DatePlaced, @DateProcessing, @DateProcessed,
                        @CustomerName, @CustomerAddress, @CustomerCity,
                        @CustomerStateProvince, @CustomerCountry, @AdminUser, @UniqueId
                    );
                    SELECT CAST(SCOPE_IDENTITY() as int);";

                int orderId = connection.QuerySingle<int>(sqlOrder, new
                {
                    order.DatePlaced,
                    order.DateProcessing,
                    order.DateProcessed,
                    order.CustomerName,
                    order.CustomerAddress,
                    order.CustomerCity,
                    order.CustomerStateProvince,
                    order.CustomerCountry,
                    order.AdminUser,
                    order.UniqueId
                }, transaction: transaction);

                order.OrderId = orderId;
                order.OrderID = orderId;

                string sqlLineItem = @"
                    INSERT INTO [dbo].[OrderLineItem] 
                    ([ProductId], [OrderId], [Quantity], [Price])
                    VALUES 
                    (@ProductId, @OrderId, @Quantity, @Price);";

                if (order.LineItems != null && order.LineItems.Any())
                {
                    foreach (var item in order.LineItems)
                    {
                        connection.Execute(sqlLineItem, new
                        {
                            item.ProductId,
                            OrderId = orderId,
                            item.Quantity,
                            item.Price
                        }, transaction: transaction);
                    }
                }

                transaction.Commit();
                return orderId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public Order? GetOrder(int id)
        {
            using IDbConnection connection = new SqlConnection(_dataAccess.ConnectionString);
            string sql = @"
                SELECT o.OrderId as OrderID, o.*, 
                       li.LineItemId, li.ProductId, li.OrderId as OrderID, li.Quantity, li.Price,
                       p.ProductId as Id, p.*
                FROM [dbo].[Order] o
                LEFT JOIN [dbo].[OrderLineItem] li ON o.OrderId = li.OrderId
                LEFT JOIN [dbo].[Product] p ON li.ProductId = p.ProductId
                WHERE o.OrderId = @OrderId;";

            var orderDictionary = new Dictionary<int, Order>();

            connection.Query<Order, OrderLineItem, Product, Order>(
                sql,
                (order, lineItem, product) =>
                {
                    if (!orderDictionary.TryGetValue(order.OrderID ?? 0, out var currentOrder))
                    {
                        currentOrder = order;
                        currentOrder.LineItems = new List<OrderLineItem>();
                        orderDictionary.Add(currentOrder.OrderID ?? 0, currentOrder);
                    }

                    if (lineItem != null)
                    {
                        lineItem.Product = product;
                        currentOrder.LineItems.Add(lineItem);
                    }

                    return currentOrder;
                },
                new { OrderId = id },
                splitOn: "LineItemId,Id");

            return orderDictionary.Values.FirstOrDefault();
        }

        public Order? GetOrderByUniqueId(string uniqueId)
        {
            using IDbConnection connection = new SqlConnection(_dataAccess.ConnectionString);
            string sql = @"
                SELECT o.OrderId as OrderID, o.*, 
                       li.LineItemId, li.ProductId, li.OrderId as OrderID, li.Quantity, li.Price,
                       p.ProductId as Id, p.*
                FROM [dbo].[Order] o
                LEFT JOIN [dbo].[OrderLineItem] li ON o.OrderId = li.OrderId
                LEFT JOIN [dbo].[Product] p ON li.ProductId = p.ProductId
                WHERE o.UniqueId = @UniqueId;";

            var orderDictionary = new Dictionary<int, Order>();

            connection.Query<Order, OrderLineItem, Product, Order>(
                sql,
                (order, lineItem, product) =>
                {
                    if (!orderDictionary.TryGetValue(order.OrderID ?? 0, out var currentOrder))
                    {
                        currentOrder = order;
                        currentOrder.LineItems = new List<OrderLineItem>();
                        orderDictionary.Add(currentOrder.OrderID ?? 0, currentOrder);
                    }

                    if (lineItem != null)
                    {
                        lineItem.Product = product;
                        currentOrder.LineItems.Add(lineItem);
                    }

                    return currentOrder;
                },
                new { UniqueId = uniqueId },
                splitOn: "LineItemId,Id");

            return orderDictionary.Values.FirstOrDefault();
        }

        public IEnumerable<Order> GetOrders()
        {
            string sql = @"SELECT OrderId as OrderID, * FROM [dbo].[Order] ORDER BY DatePlaced DESC;";
            var orders = _dataAccess.Query<Order, dynamic>(sql, new { });
            foreach (var o in orders)
            {
                o.LineItems = GetLineItemsByOrderId(o.OrderID ?? 0).ToList();
            }
            return orders;
        }

        public IEnumerable<Order> GetOutstandingOrders()
        {
            string sql = @"SELECT OrderId as OrderID, * FROM [dbo].[Order] WHERE DateProcessed IS NULL ORDER BY DatePlaced DESC;";
            var orders = _dataAccess.Query<Order, dynamic>(sql, new { });
            foreach (var o in orders)
            {
                o.LineItems = GetLineItemsByOrderId(o.OrderID ?? 0).ToList();
            }
            return orders;
        }

        public IEnumerable<Order> GetProcessedOrders()
        {
            string sql = @"SELECT OrderId as OrderID, * FROM [dbo].[Order] WHERE DateProcessed IS NOT NULL ORDER BY DateProcessed DESC;";
            var orders = _dataAccess.Query<Order, dynamic>(sql, new { });
            foreach (var o in orders)
            {
                o.LineItems = GetLineItemsByOrderId(o.OrderID ?? 0).ToList();
            }
            return orders;
        }

        public IEnumerable<OrderLineItem> GetLineItemsByOrderId(int orderId)
        {
            using IDbConnection connection = new SqlConnection(_dataAccess.ConnectionString);
            string sql = @"
                SELECT li.LineItemId, li.ProductId, li.OrderId as OrderID, li.Quantity, li.Price,
                       p.ProductId as Id, p.*
                FROM [dbo].[OrderLineItem] li
                LEFT JOIN [dbo].[Product] p ON li.ProductId = p.ProductId
                WHERE li.OrderId = @OrderId;";

            return connection.Query<OrderLineItem, Product, OrderLineItem>(
                sql,
                (lineItem, product) =>
                {
                    lineItem.Product = product;
                    return lineItem;
                },
                new { OrderId = orderId },
                splitOn: "Id");
        }

        public void UpdateOrder(Order order)
        {
            string sql = @"UPDATE [dbo].[Order] 
                           SET [DateProcessing] = @DateProcessing,
                               [DateProcessed] = @DateProcessed,
                               [AdminUser] = @AdminUser
                           WHERE [OrderId] = @OrderId;";

            _dataAccess.ExecuteCommand(sql, new
            {
                order.DateProcessing,
                order.DateProcessed,
                order.AdminUser,
                OrderId = order.OrderID
            });
        }
    }
}