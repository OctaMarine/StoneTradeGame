using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StoneActionServer.DAL;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // 1. Настраиваем опции
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            
        // 2. Указываем строку подключения (должна совпадать с вашей в appsettings.json)
        var connectionString = "Server=localhost;Database=stoneaction;Username=postgres;Password=admin";
            
        optionsBuilder.UseNpgsql(connectionString, 
            b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
        optionsBuilder.UseSnakeCaseNamingConvention();

        return new AppDbContext(optionsBuilder.Options);
    }
}