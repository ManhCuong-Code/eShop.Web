using System.Collections.Generic;
using eShop.CoreBusiness.Models;
using eShop.UseCases.PluginInterfaces.DataStore;

namespace eShop.DataStore.SQL.Dapper
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDataAccess _dataAccess;

        public ProductRepository(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public Product? GetProduct(int id)
        {
            string sql = @"SELECT ProductId as Id, Brand, Name, Price, ImageLink, Description 
                           FROM [dbo].[Product] 
                           WHERE ProductId = @ProductId;";

            return _dataAccess.QuerySingle<Product, dynamic>(sql, new { ProductId = id });
        }

        public IEnumerable<Product> GetProducts(string? filter = null)
        {
            string sql = @"SELECT ProductId as Id, Brand, Name, Price, ImageLink, Description 
                           FROM [dbo].[Product] 
                           WHERE @Filter IS NULL 
                              OR TRIM(@Filter) = '' 
                              OR LOWER(Name) LIKE '%' + LOWER(@Filter) + '%' 
                              OR LOWER(Brand) LIKE '%' + LOWER(@Filter) + '%';";

            return _dataAccess.Query<Product, dynamic>(sql, new { Filter = filter });
        }
    }
}