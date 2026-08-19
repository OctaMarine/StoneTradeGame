using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoneActionServer.BusinessLogic.Models.Crafting;
using StoneActionServer.BusinessLogic.Services;
using StoneActionServer.DAL.Repositories;

namespace StoneActionServer.BusinessLogic;

public static class Extensions
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection serviceCollection,IConfiguration configuration)
    {
        serviceCollection.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));
        
        //models
        serviceCollection.AddScoped<IModifierCalculator, YieldMultiplierCalculator>();
        serviceCollection.AddScoped<IModifierCalculator, ChanceMultiplierCalculator>();
        serviceCollection.AddScoped<IModifierCalculator, ChanceToDropExtraItemCalculator>();
        serviceCollection.AddScoped<IModifierCalculator, ResourceSaveChanceCalculator>();
        
        serviceCollection.AddScoped<JwtService>();
        serviceCollection.AddScoped<IAuthService,AuthService>();
        serviceCollection.AddScoped<IInventoryService,InventoryService>();
        serviceCollection.AddScoped<ITradeService,TradeService>();
        serviceCollection.AddScoped<ICraftingService,CraftingService>();
        serviceCollection.AddScoped<ILevelingService,LevelingService>();
        return serviceCollection;
    }
}