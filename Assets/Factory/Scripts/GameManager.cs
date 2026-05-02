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

        private uint belt1;

        private void Awake()
        {
            Instance = this;
            World = new World();
            BeltSystem = new BeltSystem(World);
            BuildSystem = new BuildSystem(World);

            belt1 = BuildSystem.CreateBelt(20, 0, 0);
            var belt2 = BuildSystem.CreateBelt(5, belt1, 0);
            var belt3 = BuildSystem.CreateBelt(9, belt2, 0);
            var belt4 = BuildSystem.CreateBelt(20, belt3, 0);
        }

        [ContextMenu("PutItem")]
        public void PutItem()
        {
            World.Belts[belt1].items[0] = Random.Range(0, 10);
        }
        
    }
}