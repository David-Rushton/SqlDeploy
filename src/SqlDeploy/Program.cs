using System;
using System.IO;
using System.Linq;
using System.Threading;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Extensions.Hosting;
using SqlDeploy.Configs;
using SqlDeploy.Models;


namespace SqlDeploy
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = GetHost(args).Services.GetService<App>();
            app.Invoke();
        }

        static IHost GetHost(string[] args) =>
            new HostBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", true);
                    config.AddEnvironmentVariables("SQLDEPLOY_");
                })
                .ConfigureServices((host, services) =>
                {
                    services.AddSingleton(host.Configuration.GetSection("source").Get<SourceConfig>());
                    services.AddSingleton(host.Configuration.GetSection("target").Get<TargetConfig>());
                    services.AddSqlDeployModels();
                })
                .UseSerilog((context, services, config) =>
                {
                    config
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                    ;
                })
                .Build()
        ;
    }
}
