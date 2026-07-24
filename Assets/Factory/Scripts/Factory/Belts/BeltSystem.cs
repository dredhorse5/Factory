using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace Factory
{
    public class BeltSystem : IDisposable
    {
        private readonly WorldProvider _world;
        private readonly BuildSystem _buildSystem;

        private readonly Dictionary<uint, BeltBuild> belts = new();
        
        public IEnumerable<BeltBuild> Belts => belts.Values;

        [Inject]
        public BeltSystem(WorldProvider world, BuildSystem buildSystem)
        {
            _world = world;
            _buildSystem = buildSystem;

            _buildSystem.OnBuildCreated += OnNewBuild;
            _buildSystem.OnBuildDestroyed += OnBuildDestroy;
            _buildSystem.OnBuildReconfigured += OnBuildReconfigure;
        }



        private void OnNewBuild(uint obj, BaseBuild build)
        {
            if (build is BeltBuild belt)
            {
                belts.Add(obj, belt);
                MarkDirtyCellAndNeighbours(build.transform.Cell);
            }
        }
        
        private void OnBuildDestroy(uint arg1, BaseBuild arg2)
        {
            if (belts.TryGetValue(arg1, out var belt))
            {
                MarkDirtyCellAndNeighbours(arg2.transform.Cell);
                belts.Remove(arg1);
            }
        }
        
        private void OnBuildReconfigure(uint arg1, BaseBuild arg2)
        {
            if (belts.TryGetValue(arg1, out var belt))
                MarkDirtyCellAndNeighbours(arg2.transform.Cell);
        }

        private void MarkDirtyCellAndNeighbours(Cell cell)
        {
        }

        public void Dispose()
        {
            _buildSystem.OnBuildCreated -= OnNewBuild;
            _buildSystem.OnBuildDestroyed -= OnBuildDestroy;
            _buildSystem.OnBuildReconfigured -= OnBuildReconfigure;
        }
    }
}