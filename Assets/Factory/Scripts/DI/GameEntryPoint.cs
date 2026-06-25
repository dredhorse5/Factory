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
    [Inject] BeltsSegmentSystem _beltsSegmentSystem;
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
            foreach (var seg in _beltsSegmentSystem.Segments)
                seg.TryAccept(1, 0);
            

            await Task.Delay(5000);
        }
    }
}