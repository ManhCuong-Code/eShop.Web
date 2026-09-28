using eShop.CoreBusiness.Models;
using eShop.DataStore.HandCoded;
using eShop.ShoppingCart.LocalStorage;
using eShop.UseCases.PluginInterfaces.DataStore;
using eShop.UseCases.PluginInterfaces.UI;
using eShop.UseCases.SearchProductScreen;
using eShop.UseCases.ViewProductSrceen;
using eShop.UseCases.ViewProductSrceen.Interfaces;
using eShop.UseCases.ShoppingCartScreen;
using eShop.UseCases.ShoppingCartScreen.Interfaces;
using eShop.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IShoppingCart,ShoppingCart>();

builder.Services.AddTransient<IViewProductUseCase, ViewProductUseCase>();
builder.Services.AddTransient<ISearchProductUseCase, SearchProductUseCase>();
builder.Services.AddTransient<IAddProductToCartUseCase, AddProductToCartUseCase>();
builder.Services.AddTransient<IViewShoppingCartUseCase, ViewShoppingCartUseCase>();
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(eShop.Web.CustomerPortal.Pages.SearchComponent).Assembly);

app.Run();
