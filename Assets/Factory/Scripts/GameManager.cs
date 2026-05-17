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
            World = new World();
            BeltSystem = new BeltSystem(World);
            BuildSystem = new BuildSystem(World);
        }

        private void Start()
        {
            paths = new Paths(BuildSystem);
            paths.Path6();
        }


        [ContextMenu("PutItem")]
        public void PutItem()
        {
            World.Belts[1].items[0] = Random.Range(1, 10);
        }
        
    }
}