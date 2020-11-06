using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlDeploy.Models;


namespace SqlDeploy
{
    public static class ServicesConfiguration
    {
        public static void AddSqlDeployModels(this IServiceCollection services)
        {
            services.AddSingleton<DatabaseDeploySchema>();
        }
    }
}
