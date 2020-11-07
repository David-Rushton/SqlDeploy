using Microsoft.Extensions.Logging;
using Serilog;
using SqlDeploy.Configs;
using SqlDeploy.Models;
using System;
using System.Collections.Generic;


namespace SqlDeploy
{
    public class App
    {
        readonly ILogger<App> _logger;
        readonly SourceConfig _sourceConfig;
        readonly TargetConfig _targetConfig;
        readonly TargetSqlServer _targetSqlServer;
        readonly DatabaseMigrationScripts _dbMigrationScripts;


        public App(
            ILogger<App> logger,
            SourceConfig sourceConfig,
            TargetConfig targetConfig,
            TargetSqlServer targetSqlServer,
            DatabaseMigrationScripts dbMigrationScripts
        )
        {
            _logger = logger;
            _sourceConfig = sourceConfig;
            _targetConfig = targetConfig;
            _targetSqlServer = targetSqlServer;
            _dbMigrationScripts = dbMigrationScripts;
        }


        public IEnumerable<string> GetProcedures()
        {
            throw new NotImplementedException();
        }




        public void Invoke()
        {
            _logger.LogInformation("Evolving database");
            _logger.LogInformation($"Source: {_sourceConfig}");
            _logger.LogInformation($"Target: {_targetConfig}");

            _targetSqlServer.CreateTargetDatabaseIfNotExists();
            _dbMigrationScripts.Deploy();


            _logger.LogInformation("Evolution complete");
        }



        private void ValidateSources()
        {

        }
    }
}
