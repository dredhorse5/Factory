using Factory;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameEntryPoint : IStartable
{
    [Inject]
    private BuildSystem buildSystem;
    [Inject]
    private WorldProvider worldProvider;
    [Inject]
    private WorldFactory worldFactory;
    [Inject]
    private Map map;
    public void Start()
    {
        Application.targetFrameRate = 30;
        var world = worldFactory.Create(256, 256);
        worldProvider.SetWorld(world);
        map.GenerateMap();
    }
}