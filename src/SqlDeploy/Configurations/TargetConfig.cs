using System;
using Microsoft.Data.SqlClient;


namespace SqlDeploy.Configurations
{
    public class TargetConfiguration
    {
        public string Server { get; set; }

        public string Database { get; set; }

        public string UserId { get; set; }

        public string Password { get; set; }

        public string TargetConnectionString => GetConnectionString();

        public string MasterConnectionString => GetConnectionString("master");


        public override string ToString() =>
            $"Server: {Server}\nDatabase: {Database}\nUser id: {UserId}\nPassword: {Password}\n"
        ;


        private string GetConnectionString(string? database = null) =>
            new SqlConnectionStringBuilder()
            {
                ApplicationName = "SqlDeploy",
                ApplicationIntent = ApplicationIntent.ReadOnly,
                MultiSubnetFailover = true,
                DataSource = Server,
                InitialCatalog = database ?? Database,
                UserID = UserId,
                Password = Password
            }.ToString()
        ;
    }
}
