using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoneActionServer.BusinessLogic.Services;
using StoneActionServer.DAL.Repositories;

namespace StoneActionServer.BusinessLogic;

public static class Extensions
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection serviceCollection,IConfiguration configuration)
    {
        serviceCollection.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));
        serviceCollection.AddScoped<JwtService>();
        serviceCollection.AddScoped<IAuthService,AuthService>();
        serviceCollection.AddScoped<IInventoryService,InventoryService>();
        serviceCollection.AddScoped<ITradeService,TradeService>();
        return serviceCollection;
    }
}