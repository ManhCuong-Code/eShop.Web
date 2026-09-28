using System;
using System.Collections.Generic;
using System.Text;
using eShop.CoreBusiness.Models;

namespace eShop.UseCases.PluginInterfaces.DataStore
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts(string? filter = null);
        Product? GetProduct(int id);
    }
}
