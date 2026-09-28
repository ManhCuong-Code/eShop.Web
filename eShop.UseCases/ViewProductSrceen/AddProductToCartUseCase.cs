using eShop.UseCases.PluginInterfaces.DataStore;
using eShop.UseCases.PluginInterfaces.UI;
using eShop.UseCases.ViewProductSrceen.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.UseCases.ViewProductSrceen
{
    public class AddProductToCartUseCase : IAddProductToCartUseCase
    {
        private readonly IProductRepository productRepository;
        private readonly IShoppingCart shoppingCart;

        public AddProductToCartUseCase(IProductRepository productRepository, IShoppingCart shoppingCart)
        {
            this.productRepository = productRepository;
            this.shoppingCart = shoppingCart;
        }
        public async void Execute(int productId)
        {
            var product = productRepository.GetProduct(productId);
            if (product != null)
            {
                await shoppingCart.AddProductAsync(product);
            }
        }
    }
}
