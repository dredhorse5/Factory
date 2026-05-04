using System;
using UnityEngine;

namespace Factory
{
    public class BuildSystem
    {
        private readonly World world;
        public static Action<uint> OnBeltCreated;

        public BuildSystem(World world)
        {
            this.world = world;
        }
        
        public uint CreateBelt(Vector2Int pos, BeltDirections inputDirection, BeltDirections outputDirection)
        {
            if (world.OccupiedTiles[pos] > 0)
            {
                Debug.LogError($"Tile already occupied: {pos}, by {world.OccupiedTiles[pos]}");
                return 0;
            }

            var belt = new Belt()
            {
                id = world.GetNextBeltId(),
                
                speed = GameSettings.BeltSpeed,
                items = new int[GameSettings.BeltSize],
                
                cell = pos,
                inputDirection = inputDirection,
                outputDirection = outputDirection,
            };
            
            world.Belts.Add(belt.id, belt);
            world.OccupiedTiles.Add(pos, belt.id);
            
            return belt.id;
        }
    }
}