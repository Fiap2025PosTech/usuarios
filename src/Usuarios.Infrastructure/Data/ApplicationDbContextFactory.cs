using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Usuarios.Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Carrega a configuração do projeto API
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../FCGames.API"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Detecta o provider baseado na configuração
        var provider = configuration["DatabaseProvider"] ?? "SqlServer";

        Console.WriteLine($"Using {provider} provider");

        if (provider == "PostgreSql")
        {
            var connectionString = configuration.GetConnectionString("PostgreSql");
            options.UseNpgsql(connectionString, x =>
            {
                x.MigrationsHistoryTable("__EFMigrationsHistory", "public");
            });
        }
        else
        {
            var connectionString = configuration.GetConnectionString("SqlServer");
            options.UseSqlServer(connectionString, x =>
            {
                x.MigrationsHistoryTable("__EFMigrationsHistory", "dbo");
            });
        }

        return new ApplicationDbContext(options.Options);
    }
}