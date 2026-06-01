using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Factory
{
    public class GameManager : MonoBehaviour
    {
        public BeltSystem BeltSystem;
        public BuildSystem BuildSystem;
        public GhostBuildSystem GhostBuildSystem;
        public World World;

        public static GameManager Instance { get; private set; }

        private Paths paths;

        private void Awake()
        {
            Instance = this;
            World = new World(256,256);
            BeltSystem = new BeltSystem(World);
            BuildSystem = new BuildSystem(World);
            GhostBuildSystem = new GhostBuildSystem(BuildSystem);
        }

        private void Start()
        {
            BuildSystem.CreateBuild(new BuildPlacement(5, 3, BuildRotations.R0), "Cube2X3");
            BuildSystem.CreateBuild(new BuildPlacement(9, 4, BuildRotations.R90), "Cube2X3");
            BuildSystem.CreateBuild(new BuildPlacement(8, 8, BuildRotations.R180), "Cube2X3");
            BuildSystem.CreateBuild(new BuildPlacement(10, 11, BuildRotations.R270), "Cube2X3");
            for (int x = 0; x < 15; x++)
            for (int y = 0; y < 15; y++)
                    BuildSystem.CreateBuild(new BuildPlacement(x, y, (BuildRotations)(Random.Range(0,5))), "Arrow");
        }


        [ContextMenu("PutItem")]
        public void PutItem()
        {
            World.Belts[1].items[0] = Random.Range(1, 10);
        }
        
    }
}