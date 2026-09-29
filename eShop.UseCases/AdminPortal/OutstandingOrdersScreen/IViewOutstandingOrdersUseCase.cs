using System.Collections.Generic;
using eShop.CoreBusiness.Models;

namespace eShop.UseCases.AdminPortal.OutstandingOrdersScreen
{
    public interface IViewOutstandingOrdersUseCase
    {
        IEnumerable<Order> Execute();
    }
}