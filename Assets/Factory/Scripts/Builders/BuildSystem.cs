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
        public Action<uint, BaseBuild> OnBuildReconfigured;
        

        public BuildSystem(WorldProvider worldProvider, BuildsDatabase buildsDatabase)
        {
            this.worldProvider = worldProvider;
            this.buildsDatabase = buildsDatabase;
        }
        
        #region Create Build

        public uint CreateBuild(Cell cell, BuildRotations rotation, string buildID, IBuildData data)
        {
            if (buildsDatabase.GetBuild(buildID, out var build))
                return CreateBuild(cell, rotation, build, data);
            Debug.LogError("Build not found with id: " + buildID);
            return 0;
        }
        
        public uint CreateBuild(Cell cell, BuildRotations rotation, BaseBuildSO buildSO, IBuildData data)
        {
            var tiles = BuildTransformCalculator.GetOccupiedTiles(cell, rotation, buildSO.Size);
            if (CanPlaceBuild(tiles, buildSO))
            {
                var build = buildSO.CreateBuild(worldProvider.world.GetNextBuildId(), data);
                build.transform = new BuildTransform(cell, rotation, buildSO.Size);
                build.OnPlaced();
                worldProvider.world.Builds.Add(build.id, build);
                for (var i = 0; i < tiles.Length; i++)
                    worldProvider.world.cells[tiles[i].x, tiles[i].y] = build.id;
                OnBuildCreated?.Invoke(build.id, build);
                return build.id;
            }
            
            return 0;
        }
        
        #endregion

        #region Destroy Build

        

        public void DestroyBuild(Cell atCell)
        {
            var buildId = worldProvider.world.cells[atCell.x, atCell.y];
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
                worldProvider.world.cells[tiles[i].x, tiles[i].y] = 0;
            worldProvider.world.Builds.Remove(build.id);
            OnBuildDestroyed?.Invoke(build.id, build);
        }

        #endregion

        
        public void ReconfigureBuild(BaseBuild build, IBuildData data)
        {
            build.SetData(data);
            build.OnReconfigured();
            OnBuildReconfigured?.Invoke(build.id, build);
        }
        
        public bool CanPlaceBuild(Cell cell, BuildRotations rotation, BaseBuildSO buildSO)
        {
            var tiles = BuildTransformCalculator.GetOccupiedTiles(cell, rotation, buildSO.Size);
            return CanPlaceBuild(tiles, buildSO);
        }
        public bool CanPlaceBuild(Cell[] tiles, BaseBuildSO buildSO) => worldProvider.world.IsAreaFree(tiles);

    }
}