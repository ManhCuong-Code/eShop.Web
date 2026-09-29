using System.Collections.Generic;
using eShop.CoreBusiness.Models;

namespace eShop.UseCases.AdminPortal.ProcessedOrdersScreen
{
    public interface IViewProcessedOrdersUseCase
    {
        IEnumerable<Order> Execute();
    }
}