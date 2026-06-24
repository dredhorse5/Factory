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
    public void Start()
    {
        Application.targetFrameRate = 30;
        var world = worldFactory.Create(256, 256);
        worldProvider.SetWorld(world);
        map.GenerateMap();
    }
}