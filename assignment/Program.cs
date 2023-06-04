using assignment.DataContext;
using assignment.Extensions;
using assignment.Repositories;
using assignment.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Web;

namespace assignment;

public class Program
{
    public static void Main(string[] args)
    {
        var logger = LogManager.Setup()
            .LoadConfigurationFromAppSettings()
            .GetCurrentClassLogger();

        var builder = WebApplication.CreateBuilder(args);

        // 01. DB 생성 [Sqlite]
        var dbContext = new EmployeeContacDBContext();
        dbContext.Database.EnsureCreated();

        // 02. NLog 사용 설정
        builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        builder.Host.UseNLog();

        // 03. NLog Service IoC 등록
        builder.Services.AddSingleton<ILoggerService, LoggerService>();

        // 04. DBContext Service IoC 등록
        builder.Services.AddDbContext<EmployeeContacDBContext>(options =>
        {
            options.UseSqlite($"{builder.Configuration.GetConnectionString("EmployeeContacDB")}");
        });

        // 05. EmployeeContacRepository IoC 등록
        builder.Services.AddScoped<EmployeeContacRepository>();

        // 06. IEmployeeContacService IoC 등록
        builder.Services.AddScoped<IEmployeeContacService, EmployeeContacService>();


        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        logger.Debug("WebApplication build complete");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Global Exception log 처리 미들웨어 등록
        app.ConfigureCustomExceptionMiddleware();

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}