using System;
using Dapper;
using Microsoft.Data.Sql;


namespace SqlDeploy.Models
{
    public class DatabaseDeploySchema
    {
        public void UpdateSchema()
        {

        }


        private string UpdateSchemaStatement => @"

            IF(SCHEMA_ID('db') IS NULL)
            BEGIN
                PRINT N'Creating schema db';
                CREATE SCHEMA db AUTHORIZATION dbo;
            END


            IF(OBJECT_ID('db.Versions') IS NULL)
            BEGIN
                PRINT N'Creating table db.Versions';

                CREATE TABLE db.Versions
                {
                    VersionNumber   VARCHAR(15)     NOT NULL;
                    ...
                }
            END


        ";
    }
}
