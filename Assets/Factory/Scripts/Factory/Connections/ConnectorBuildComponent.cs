namespace Factory
{
    public class ConnectorBuildComponent : BaseBuildComponent, IConnectable
    {
        public ConnectorBuildComponent(Connection[] outputConnections, Connection[] inputConnections)
        {
            OutputConnections = outputConnections;
            InputConnections = inputConnections;
        }

        protected override void OnInitialize() { }

        public Connection[] OutputConnections { get; set; }
        public Connection[] InputConnections { get; set; }
    }
}