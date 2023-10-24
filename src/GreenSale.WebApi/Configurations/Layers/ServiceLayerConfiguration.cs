using GreenSale.Service.Interfaces.Auth;
using GreenSale.Service.Interfaces.BuyerPosts;
using GreenSale.Service.Interfaces.Categories;
using GreenSale.Service.Interfaces.Common;
using GreenSale.Service.Interfaces.Notifications;
using GreenSale.Service.Interfaces.Roles;
using GreenSale.Service.Interfaces.SellerPosts;
using GreenSale.Service.Interfaces.Storages;
using GreenSale.Service.Interfaces.Users;
using GreenSale.Service.Service.Auth;
using GreenSale.Service.Service.BuyerPosts;
using GreenSale.Service.Service.Categories;
using GreenSale.Service.Service.Common;
using GreenSale.Service.Service.Notifications;
using GreenSale.Service.Service.Roles;
using GreenSale.Service.Service.SellerPosts;
using GreenSale.Service.Service.Storages;
using GreenSale.Service.Service.Users;

namespace GreenSale.WebApi.Configurations.Layers;

public static class ServiceLayerConfiguration
{
    public static void ConfigureServiceLayer(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ISmsSender, SmsSender>();
        builder.Services.AddScoped<IAuthServices, AuthServise>();
        builder.Services.AddScoped<IUserRoleService, UserRoleService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IPaginator, Pagination>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<IStoragesService, StorageService>();
        builder.Services.AddScoped<IFileService, FileService>();
        builder.Services.AddScoped<ISellerPostService, SellerPostService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IIdentityService, IdentityService>();
        builder.Services.AddScoped<IBuyerPostService, BuyerPostService>();
        builder.Services.AddScoped<IBuyerPostStarService, BuyerPostStarService>();
        builder.Services.AddScoped<ISellerPostStarService, SellerPostStarService>();
        builder.Services.AddScoped<IStoragePostStarService, StoragePostStarService>();
    }
}
