using eShop.CoreBusiness.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.UseCases.ViewProductSrceen.Interfaces
{
    public interface IViewProductUseCase
    {
        Product? Execute(int id);
    }
}
