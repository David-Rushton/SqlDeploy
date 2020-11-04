DROP DATABASE IF EXISTS SchemaDeploy;
GO


DROP TABLE IF EXISTS dbo.Versions;
CREATE TABLE dbo.Versions
(
    VersionNumber       NVARCHAR(15)    NOT NULL PRIMARY KEY,
    VersionDeployed     DATETIME2       NOT NULL ,
    VersionDeployedBy   SYSNAME         NOT NULL ,
);
