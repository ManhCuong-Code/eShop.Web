using System;
using System.Collections.Generic;
using System.Text;
using eShop.CoreBusiness.Models;
using eShop.UseCases.PluginInterfaces.DataStore;
using eShop.UseCases.ViewProductSrceen.Interfaces;

namespace eShop.UseCases.ViewProductSrceen
{
    public class ViewProductUseCase : IViewProductUseCase
    {
        private readonly IProductRepository productRepository;
        public ViewProductUseCase(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        public Product? Execute(int id)
        {
            return productRepository.GetProduct(id);
        }
    }
}
