using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoneActionServer.DAL.Repositories;

namespace StoneActionServer.DAL;

public static class Extensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<AppDbContext>(options =>
        {
            var connectionConf = configuration.GetConnectionString("PostgresConnection");
            //var connectionConf = configuration.GetConnectionString("PostgresConnectionDocker");

            //options.LogTo(Console.WriteLine);
            options.UseNpgsql(connectionConf);
        });
        serviceCollection.AddScoped<IAuthRepository,AuthRepository>();
        serviceCollection.AddScoped<IInventoryRepository,InventoryRepository>();
        serviceCollection.AddScoped<ITradeRepository,TradeRepository>();
        return serviceCollection;
    }
}