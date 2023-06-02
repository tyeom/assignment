using assignment.Models;
using Microsoft.EntityFrameworkCore;

namespace assignment.DataContext;

public class EmployeeContacDBContext : DbContext
{
    public EmployeeContacDBContext()
    {
        //
    }

    public EmployeeContacDBContext(DbContextOptions<EmployeeContacDBContext> options)
        : base(options)
    {
    }

    public string? DbPath { get; }

    public DbSet<EmployeeContacModel> EmployeeContac { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();

        options.UseSqlite(configuration.GetConnectionString("EmployeeContacDB"));
    }
}
