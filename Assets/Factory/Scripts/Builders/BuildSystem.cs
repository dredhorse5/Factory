using System;
using UnityEngine;
using VContainer;

namespace Factory
{
    public class BuildSystem
    {
        private readonly World world;
        public static Action<uint> OnBuildCreated;
        
        [Inject]
        private BuildsDatabase buildsDatabase;

        public BuildSystem(World world)
        {
            this.world = world;
        }

        #region Belt

        

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
        
        
        
        #endregion
        
        
        public uint CreateBelt(Vector2Int pos, BeltDirections inputDirection, BeltDirections outputDirection)
        {
            if (world.OccupiedTiles.ContainsKey(pos))
            {
                Debug.LogError($"Tile already occupied: {pos}, by {world.OccupiedTiles[pos]}");
                return 0;
            }

            var belt = new Belt()
            {
                id = world.GetNextBuildId(),
                
                speed = GameSettings.BeltSpeed,
                items = new int[GameSettings.BeltSize],
                progress = new float[GameSettings.BeltSize],
                
                cell = pos,
                inputDirection = inputDirection,
                outputDirection = outputDirection,
            };
            
            world.Belts.Add(belt.id, belt);
            world.OccupiedTiles.Add(pos, belt.id);
            
            OnBuildCreated?.Invoke(belt.id);
            
            return belt.id;
        }

        public uint CreateBuild(BuildPlacement transform, string buildID)
        {
            if (buildsDatabase.GetBuild(buildID, out var build))
                return CreateBuild(transform, build);
            Debug.LogError("Build not found with id: " + buildID);
            return 0;
        }
        
        public uint CreateBuild(BuildPlacement transform, BaseBuildSO buildSO)
        {
            var tiles = transform.GetOccupiedTiles(buildSO.Size);
            if (CanPlaceBuild(tiles, buildSO))
            {
                var build = buildSO.CreateBuild(world.GetNextBuildId());
                build.transform = new BuildTransform()
                {
                    Position = transform.Position,
                    Rotation = transform.Rotation,
                    Size = buildSO.Size
                };
                world.Builds.Add(build.id, build);
                for (var i = 0; i < tiles.Length; i++)
                    world.tiles[tiles[i].x, tiles[i].y] = build.id;
                OnBuildCreated?.Invoke(build.id);
            }
            else return 0;
            
            return 0;
        }

        public bool CanPlaceBuild(BuildPlacement transform, BaseBuildSO buildSO)
        {
            var tiles = transform.GetOccupiedTiles(buildSO.Size);
            return CanPlaceBuild(tiles, buildSO);
        }
        public bool CanPlaceBuild(Vector2Int[] tiles, BaseBuildSO buildSO) => world.IsAreaFree(tiles);

    }
}