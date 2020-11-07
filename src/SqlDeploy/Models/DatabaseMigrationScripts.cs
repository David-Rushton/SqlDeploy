using Microsoft.Extensions.Logging;
using Serilog;
using SqlDeploy.Configs;
using System;
using System.IO;
using System.Linq;


namespace SqlDeploy.Models
{
    public class DatabaseMigrationScripts
    {
        readonly SourceConfig _sourceConfig;
        readonly ILogger<DatabaseMigrationScripts> _logger;


        public DatabaseMigrationScripts(SourceConfig sourceConfig, ILogger<DatabaseMigrationScripts> logger) =>
            (_sourceConfig, _logger) = (sourceConfig, logger)
        ;


        public void Deploy()
        {
            foreach(var file in Directory.GetFiles(_sourceConfig.Root))
            {
                _logger.LogInformation($"Migration script found: {file}");
            }
        }
    }
}
