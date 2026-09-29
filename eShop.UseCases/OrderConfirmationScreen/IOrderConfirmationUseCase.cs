using eShop.CoreBusiness.Models;

namespace eShop.UseCases.OrderConfirmationScreen
{
    public interface IOrderConfirmationUseCase
    {
        Order? Execute(string uniqueId);
    }
}