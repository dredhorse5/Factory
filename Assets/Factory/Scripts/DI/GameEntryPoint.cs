using Factory;
using UnityEngine;
using VContainer.Unity;

public class GameEntryPoint : IStartable
{
    private readonly BuildSystem buildSystem;
    
    public GameEntryPoint(BuildSystem buildSystem)
    {
        this.buildSystem = buildSystem;
    }
    
    public void Start()
    {
        buildSystem.CreateBuild(new BuildPlacement(5, 3, BuildRotations.R0), "Cube2X3");
        buildSystem.CreateBuild(new BuildPlacement(9, 4, BuildRotations.R90), "Cube2X3");
        buildSystem.CreateBuild(new BuildPlacement(8, 8, BuildRotations.R180), "Cube2X3");
        buildSystem.CreateBuild(new BuildPlacement(10, 11, BuildRotations.R270), "Cube2X3");
        for (int x = 0; x < 15; x++)
        for (int y = 0; y < 15; y++)
            buildSystem.CreateBuild(new BuildPlacement(x, y, (BuildRotations)(Random.Range(0,5))), "Arrow");
    }
}