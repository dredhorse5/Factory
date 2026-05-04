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

        public void CreateBeltQueue(int[] cells, BeltDirections inputDirection = BeltDirections.NONE, BeltDirections outputDirection = BeltDirections.NONE)
        {
            Vector2Int curCell = new Vector2Int(0,0);
            Vector2Int nextCell = new Vector2Int(0,0);
            
            BeltDirections input = (BeltDirections.Up);
            BeltDirections output = (BeltDirections.Up);

            if (inputDirection != BeltDirections.NONE)
                input = inputDirection;
            
            for (int i = 0; i < cells.Length - 2; i += 2)
            {
                curCell = new Vector2Int(cells[i], cells[i + 1]);
                nextCell = new Vector2Int(cells[i + 2], cells[i + 3]);
                
                if(i > 0)
                    input = output;
                
                if(nextCell.x == curCell.x && nextCell.y < curCell.y)
                    output = BeltDirections.Down;
                else if(nextCell.x == curCell.x && nextCell.y > curCell.y)
                    output = BeltDirections.Up;
                else if(nextCell.x > curCell.x && nextCell.y == curCell.y)
                    output = BeltDirections.Right;
                else if(nextCell.x < curCell.x && nextCell.y == curCell.y)
                    output = BeltDirections.Left;
                
                CreateBelt(curCell, input, output);
                
            }
            CreateBelt(cells[^2],cells[^1], output, outputDirection == BeltDirections.NONE ? output : outputDirection );
        }

        public uint CreateBelt(int posx, int posy, BeltDirections inputDirection, BeltDirections outputDirection)=>
            CreateBelt(new Vector2Int(posx, posy), inputDirection, outputDirection);
        public uint CreateBelt(Vector2Int pos, BeltDirections inputDirection, BeltDirections outputDirection)
        {
            if (world.OccupiedTiles.ContainsKey(pos))
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
            
            OnBeltCreated?.Invoke(belt.id);
            
            return belt.id;
        }
    }
}