using System;
using eShop.CoreBusiness.Models;
using eShop.UseCases.PluginInterfaces.DataStore;

namespace eShop.UseCases.AdminPortal.ProcessOrderScreen
{
    public class ProcessOrderUseCase : IProcessOrderUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public ProcessOrderUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public bool Execute(int orderId, string adminUserName)
        {
            var order = _orderRepository.GetOrder(orderId);
            if (order == null) return false;

            order.AdminUser = adminUserName;
            order.DateProcessed = DateTime.Now;
            _orderRepository.UpdateOrder(order);
            return true;
        }
    }
}