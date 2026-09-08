namespace Factory
{
    public class ConnectorBuildComponent : IBuildComponent, IConnectable
    {
        public ConnectorBuildComponent(Connection[] outputConnections, Connection[] inputConnections)
        {
            OutputConnections = outputConnections;
            InputConnections = inputConnections;
        }

        public void Initialize(BaseBuild build)
        {
            
        }

        public Connection[] OutputConnections { get; set; }
        public Connection[] InputConnections { get; set; }
    }
}