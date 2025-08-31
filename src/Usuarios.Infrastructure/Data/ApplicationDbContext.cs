using Microsoft.EntityFrameworkCore;

namespace Usuarios.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    private readonly string _provider;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        _provider = DetectProvider();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    private string DetectProvider()
    {
        try
        {
            if (Database.IsRelational())
            {
                var connection = Database.GetDbConnection();
                return connection.GetType().Name switch
                {
                    "SqlConnection" => "SqlServer",
                    "NpgsqlConnection" => "PostgreSql",
                    _ => "SqlServer"
                };
            }
            else
            {
                return "InMemory";
            }
        }
        catch
        {
            return "SqlServer";
        }
    }

    public string GetMigrationsPath()
    {
        return _provider switch
        {
            "PostgreSql" => "Migrations/PostgreSql",
            "SqlServer" => "Migrations/SqlServer",
            "InMemory" => "Migrations/InMemory",
            _ => "Migrations/SqlServer"
        };
    }
}