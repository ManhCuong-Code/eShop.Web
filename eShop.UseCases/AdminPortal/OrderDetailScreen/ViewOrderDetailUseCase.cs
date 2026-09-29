using eShop.CoreBusiness.Models;
using eShop.UseCases.PluginInterfaces.DataStore;

namespace eShop.UseCases.AdminPortal.OrderDetailScreen
{
    public class ViewOrderDetailUseCase : IViewOrderDetailUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public ViewOrderDetailUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Order? Execute(int orderId)
        {
            return _orderRepository.GetOrder(orderId);
        }
    }
}