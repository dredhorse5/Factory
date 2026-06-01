using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Factory
{
    public class GameManager : MonoBehaviour
    {
        public BeltSystem BeltSystem;
        public BuildSystem BuildSystem;
        public World World;

        public static GameManager Instance { get; private set; }

        private Paths paths;

        private void Awake()
        {
            Instance = this;
            World = new World(256,256);
            BeltSystem = new BeltSystem(World);
            BuildSystem = new BuildSystem(World);
        }

        private void Start()
        {
            /*paths = new Paths(BuildSystem);
            paths.Path4();
            PutItem();*/

            BuildSystem.CreateBuild(new BuildPlacement(5, 3, BuildRotations.R0), "Cube2X3");
            BuildSystem.CreateBuild(new BuildPlacement(9, 4, BuildRotations.R90), "Cube2X3");
            BuildSystem.CreateBuild(new BuildPlacement(8, 8, BuildRotations.R180), "Cube2X3");
            BuildSystem.CreateBuild(new BuildPlacement(10, 11, BuildRotations.R270), "Cube2X3");
            for (int x = 0; x < 15; x++)
            {
                for (int y = 0; y < 15; y++)
                {
                    BuildSystem.CreateBuild(new BuildPlacement(x, y, (BuildRotations)(Random.Range(0,5))), "Arrow");
                }
            }
            /*BuildSystem.CreateBuild(new BuildPlacement(0, 0, BuildRotations.R0), "CatWalk");
            BuildSystem.CreateBuild(new BuildPlacement(0, 1, BuildRotations.R90), "Arrow");
            BuildSystem.CreateBuild(new BuildPlacement(0, 2, BuildRotations.R180), "CatWalk");
            BuildSystem.CreateBuild(new BuildPlacement(0, 3, BuildRotations.R270), "Arrow");
            
            BuildSystem.CreateBuild(new BuildPlacement(1, 0, BuildRotations.R0), "Barrel");
            BuildSystem.CreateBuild(new BuildPlacement(1, 1, BuildRotations.R90), "Barrel");
            BuildSystem.CreateBuild(new BuildPlacement(1, 2, BuildRotations.R180), "Barrel");
            BuildSystem.CreateBuild(new BuildPlacement(1, 3, BuildRotations.R270), "Barrel");*/
        }


        [ContextMenu("PutItem")]
        public void PutItem()
        {
            World.Belts[1].items[0] = Random.Range(1, 10);
        }
        
    }
}