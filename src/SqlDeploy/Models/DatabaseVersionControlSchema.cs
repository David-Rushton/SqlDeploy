using System;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SqlDeploy.Configs;


namespace SqlDeploy.Models
{
    public class DatabaseVersionControlSchema
    {
        readonly TargetConfig _targetConfig;
        readonly ILogger<DatabaseVersionControlSchema> _logger;


        public DatabaseVersionControlSchema(ILogger<DatabaseVersionControlSchema> logger, TargetConfig targetConfig) =>
            (_logger, _targetConfig) = (logger, targetConfig)
        ;


        public void CreateOrUpdateSchema()
        {
            using var targetConnection = new SqlConnection(_targetConfig.TargetConnectionString);
            targetConnection.InfoMessage += (o, e) => _logger.LogInformation($"{e.Message}");

            targetConnection.Execute(CreateOrUpdateVersionControlSchemaStatement);
        }

        public void RecordMigrationStart()
        {

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


        private void SqlConnectionInformationEventHandler(object sender, EventArgs e) =>
            _logger.LogInformation(e.ToString())
        ;


        private string CreateOrUpdateVersionControlSchemaStatement => @"

            IF(SCHEMA_ID('db') IS NULL)
            BEGIN
                PRINT N'Creating schema db';
                EXECUTE sp_executesql N'CREATE SCHEMA db AUTHORIZATION dbo;';
            END

            IF(OBJECT_ID('db.Versions') IS NULL)
            BEGIN
                PRINT N'Creating table db.Versions';
                CREATE TABLE db.Versions
                (
                    VersionNumber           VARCHAR(15)     NOT NULL,
                    VersionDeployedBy       SYSNAME         NOT NULL    CONSTRAINT DF_db_Versions_VersionDeployedBy DEFAULT(SUSER_SNAME()),
                    VersionDeployStart      DATETIME2(7)    NOT NULL    CONSTRAINT DF_db_Versions_VersionDeployStart DEFAULT(SYSUTCDATETIME()),
                    VersionDeployEnd        DATETIME2(7)    NULL,
                    VersionDeployDuration                   AS ISNULL(DATEDIFF(MILLISECOND, VersionDeployStart, VersionDeployEnd), -1),
                    VersionDeploySuccess    BIT             NOT NULL    CONSTRAINT DF_db_Versions_VersionDeploySuccess DEFAULT(1),

                    CONSTRAINT PK_db_Versions PRIMARY KEY CLUSTERED (VersionNumber),
                );
            END

            IF(OBJECT_ID('db.ChangeScripts') IS NULL)
            BEGIN
                PRINT N'Creating table db.ChangeScripts';
                CREATE TABLE db.ChangeScripts
                (
                    ChangeScriptId              INT IDENTITY(1, 1)  NOT NULL,
                    VersionNumber               VARCHAR(15)         NOT NULL    CONSTRAINT FK_db_ChangeScripts_VersionNumber REFERENCES db.Versions(VersionNumber),
                    ChangeScriptName            VARCHAR(255)        NOT NULL,
                    ChangeScriptDeployStart     DATETIME2(7)        NOT NULL    CONSTRAINT FK_db_ChangeScripts_ChangeScriptDeployStart DEFAULT(SYSUTCDATETIME()),
                    ChangeScriptDeployEnd       DATETIME2(7)        NULL,
                    ChangeScriptDeployDuration                      AS ISNULL(DATEDIFF(MILLISECOND, ChangeScriptDeployStart, ChangeScriptDeployEnd), -1),
                    ChangeScriptDeploySuccess   BIT                 NOT NULL    CONSTRAINT FK_db_ChangeScripts_ChangeScriptDeploySuccess DEFAULT(1),

                    CONSTRAINT PK_db_ChangeScripts PRIMARY KEY CLUSTERED (ChangeScriptId),
                );
            END

        ";
    }
}
