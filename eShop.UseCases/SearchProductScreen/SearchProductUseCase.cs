using System;
using System.Collections.Generic;
using System.Text;
using eShop.CoreBusiness.Models;
using eShop.UseCases.PluginInterfaces.DataStore;

namespace eShop.UseCases.SearchProductScreen
{
    public class SearchProductUseCase : ISearchProductUseCase
    {
        private readonly IProductRepository productRepository;
        public SearchProductUseCase(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        public IEnumerable<Product> Execute(string? filter = null)
        {
            return productRepository.GetProducts(filter);
        }
    }
}
