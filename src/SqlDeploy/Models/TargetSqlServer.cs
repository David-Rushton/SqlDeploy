using System;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SqlDeploy.Configs;


namespace SqlDeploy.Models
{
    public class TargetSqlServer
    {
        readonly TargetConfig _targetConfig;
        readonly ILogger<TargetSqlServer> _logger;


        public TargetSqlServer(ILogger<TargetSqlServer> logger, TargetConfig targetConfig) =>
            (_logger, _targetConfig) = (logger, targetConfig)
        ;


        public void CreateTargetDatabaseIfNotExists()
        {
            using var masterConnection = new SqlConnection(_targetConfig.MasterConnectionString);
            masterConnection.InfoMessage += (o, e) => _logger.LogInformation(e.Message);

            masterConnection.Execute(CreateTargetDatabaseIfNotExistsStatement,
                new
                {
                    Database = _targetConfig.Database
                }
            );
        }


        private string CreateTargetDatabaseIfNotExistsStatement => @"

            DECLARE @DB_NAME SYSNAME = @Database;

            IF DB_ID(@DB_NAME) IS NULL
            BEGIN
                DECLARE @Qry NVARCHAR(255) = N'CREATE DATABASE ' + QUOTENAME(@DB_NAME) + ';';
                DECLARE @Msg NVARCHAR(255) = N'Creating database ' + QUOTENAME(@DB_NAME) + ';';

                PRINT @Msg;
                EXECUTE sp_executesql @Qry;
            END

        ";
    }
}
