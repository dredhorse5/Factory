using Factory;
using VContainer;

public class TransferSimulation : ISimulation
{
    private readonly BuildSystem buildSystem;
    private readonly WorldProvider worldProvider;
    

    [Inject]
    public TransferSimulation(BuildSystem buildSystem, WorldProvider worldProvider)
    {
        this.buildSystem = buildSystem;
        this.worldProvider = worldProvider;
    }

    public void Tick(float dt)
    {
        var builds = worldProvider.world.Builds.Values;
        foreach (var build in builds)
        {
            if(build.TryGetComponent(out ConnectorBuildComponent connector))
            {
                UpdateConnectionComponent(connector);
            }
        }
    }
    void UpdateConnectionComponent(ConnectorBuildComponent connector)
    {
        for (var i = 0; i < connector.OutputConnections.Length; i++)
        {
            UpdateConnection(connector.OutputConnections[i]);
        }
    }

    void UpdateConnection(Connection connection)
    {
        if (connection == null)
            return;
        var target = connection.Connected;
        if (target == null)
            return;
        if (!connection.Endpoint.CanExtract())
            return;
        if (!target.Endpoint.CanInsert(default))
            return;
        connection.Endpoint.TryExtract(out var item);
        item.Progress--;
        target.Endpoint.TryInsert(item);
    }
}