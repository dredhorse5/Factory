using System.Linq;
using System.Threading.Tasks;
using Factory;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameEntryPoint : IStartable
{
    [Inject] BuildSystem buildSystem;
    [Inject] WorldProvider worldProvider;
    [Inject] WorldFactory worldFactory;
    [Inject] Map map;
    [Inject] BeltSystem _beltSystem;
    [Inject] SimulationSystem _simulationSystem;
    [Inject] ConnectionsSystem _connectionsSystem;
    public void Start()
    {
        Application.targetFrameRate = 30;
        var world = worldFactory.Create(256, 256);
        worldProvider.SetWorld(world);
        map.GenerateMap();

        _ = SpawnItems();
    }
    
    private async Task SpawnItems()
    {
        while (true)
        {
            if(_beltSystem.Belts.Any())
            {
                var beltBuild = _beltSystem.Belts.Last();
                if (beltBuild != null && beltBuild.TryGetComponent(out ConnectorBuildComponent conn))
                    conn.OutputConnections[0].Endpoint.TryInsert(new ItemProgress() { Item = 0, Progress = 0 });
            }
            

            await Task.Delay(5000);
        }
    }
}