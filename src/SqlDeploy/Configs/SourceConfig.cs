using System;


namespace SqlDeploy.Configs
{
    public class SourceConfig
    {
        public string Root { get; set; }


        public override string ToString() => $"Source root: {Root}";
    }
}
