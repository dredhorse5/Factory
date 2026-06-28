using System;
using UnityEngine;
using VContainer;

namespace Factory
{
    public class BuildSystem
    {
        private readonly WorldProvider worldProvider;
        private readonly BuildsDatabase buildsDatabase;
        
        public Action<uint, BaseBuild> OnBuildCreated;
        public Action<uint, BaseBuild> OnBuildWillDestroy;
        public Action<uint, BaseBuild> OnBuildDestroyed;
        

        public BuildSystem(WorldProvider worldProvider, BuildsDatabase buildsDatabase)
        {
            this.worldProvider = worldProvider;
            this.buildsDatabase = buildsDatabase;
        }

        #region Belt

        

        /*
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
            */
        
        
        
        /*public uint CreateBelt(Vector2Int pos, BeltDirections inputDirection, BeltDirections outputDirection)
        {
            if (worldProvider.world.OccupiedTiles.ContainsKey(pos))
            {
                Debug.LogError($"Tile already occupied: {pos}, by {worldProvider.world.OccupiedTiles[pos]}");
                return 0;
            }

            var belt = new Belt()
            {
                id = worldProvider.world.GetNextBuildId(),

                speed = GameSettings.BeltSpeed,
                items = new ushort[GameSettings.BeltSize],
                progress = new float[GameSettings.BeltSize],

                cell = pos,
                inputDirection = inputDirection,
                outputDirection = outputDirection,
            };

            worldProvider.world.Belts.Add(belt.id, belt);
            worldProvider.world.OccupiedTiles.Add(pos, belt.id);

            OnBuildCreated?.Invoke(belt.id);

            return belt.id;
        }*/
        
        #endregion
        
        

        public uint CreateBuild(Vector2Int cell, BuildRotations rotation, string buildID)
        {
            if (buildsDatabase.GetBuild(buildID, out var build))
                return CreateBuild(cell, rotation, build);
            Debug.LogError("Build not found with id: " + buildID);
            return 0;
        }
        
        public uint CreateBuild(Vector2Int cell, BuildRotations rotation, BaseBuildSO buildSO)
        {
            var tiles = BuildTransformCalculator.GetOccupiedTiles(cell, rotation, buildSO.Size);
            if (CanPlaceBuild(tiles, buildSO))
            {
                var build = buildSO.CreateBuild(worldProvider.world.GetNextBuildId());
                build.transform = new BuildTransform(cell, rotation, buildSO.Size);
                build.OnPlaced();
                worldProvider.world.Builds.Add(build.id, build);
                for (var i = 0; i < tiles.Length; i++)
                    worldProvider.world.tiles[tiles[i].x, tiles[i].y] = build.id;
                OnBuildCreated?.Invoke(build.id, build);
                return build.id;
            }
            
            return 0;
        }

        public void DestroyBuild(Vector2Int atCell)
        {
            var buildId = worldProvider.world.tiles[atCell.x, atCell.y];
            if(buildId > 0)
                DestroyBuild(buildId);
        }

        public void DestroyBuild(uint buildId)
        {
            if(worldProvider.world.Builds.TryGetValue(buildId, out var build))
                DestroyBuild(build);
        }

        public void DestroyBuild(BaseBuild build)
        {
            if (!worldProvider.world.Builds.ContainsKey(build.id))
            {
                Debug.LogError($"Build {build.id} already destroyed");
                return;
            }
            
            OnBuildWillDestroy?.Invoke(build.id, build);
            build.OnDestroyed();
            var tiles = build.transform.GetOccupiedTiles();
            for (var i = 0; i < tiles.Length; i++)
                worldProvider.world.tiles[tiles[i].x, tiles[i].y] = 0;
            worldProvider.world.Builds.Remove(build.id);
            OnBuildDestroyed?.Invoke(build.id, build);
        }


        public bool CanPlaceBuild(Vector2Int cell, BuildRotations rotation, BaseBuildSO buildSO)
        {
            var tiles = BuildTransformCalculator.GetOccupiedTiles(cell, rotation, buildSO.Size);
            return CanPlaceBuild(tiles, buildSO);
        }
        public bool CanPlaceBuild(Vector2Int[] tiles, BaseBuildSO buildSO) => worldProvider.world.IsAreaFree(tiles);

    }
}