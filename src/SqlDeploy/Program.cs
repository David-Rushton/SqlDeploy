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
using SqlDeploy.Configurations;


namespace SqlDeploy
{
    public class SqlVersion
    {
        public string Version { get; set; }
    }


    class Program
    {
        static void Main(string[] args)
        {
            // test connection
            // deploy vc schema
            // file groups
            // sequences
            // tables
                // synonyms
            // views
            // types
            // procedures
            // security


            // functions


            // data movements

            var host = GetHost(args);
            var target = host.Services.GetService<TargetConfiguration>();

            Thread.Sleep(30000);
            Console.WriteLine("Waking up");


            using var connection = new SqlConnection(target.ConnectionString);
            var result = connection.Query<SqlVersion>("SELECT @@VERSION AS Version").FirstOrDefault();
            Console.WriteLine(result.Version);




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
                    services.AddSingleton(host.Configuration.GetSection("target").Get<TargetConfiguration>());
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
