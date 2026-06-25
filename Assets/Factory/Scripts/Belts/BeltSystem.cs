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
        private readonly BeltsSegmentSystem _segmentSystem;

        private readonly Dictionary<uint, BeltBuild> belts = new();

        [Inject]
        public BeltSystem(WorldProvider world, BuildSystem buildSystem, BeltsSegmentSystem segmentSystem)
        {
            _world = world;
            _buildSystem = buildSystem;
            _segmentSystem = segmentSystem;

            _buildSystem.OnBuildCreated += OnNewBuild;
            TickSystem.OnFixedTick += FixedTick;
        }
        

        private void OnNewBuild(uint obj, BaseBuild build)
        {
            if (build is BeltBuild beltBuild)
            {
                belts.Add(obj, beltBuild);
                
                var backSegment = GetSegmentAtCell(beltBuild.GetInputCell());
                var forwardSegment = GetSegmentAtCell(beltBuild.GetOutputCell());
                
                // есть сегмент сзади, то добавляем к нему новый belt
                if (backSegment != null && backSegment.GetOutputCell == beltBuild.transform.Cell)
                {
                    var seg1 = _segmentSystem.AddBeltsToSegment(new BeltBuild[] { beltBuild }, backSegment);
                    //а если есть и передний, то соединяем их
                    if(forwardSegment != null && forwardSegment.GetInputCell == beltBuild.transform.Cell)
                        _segmentSystem.MergeSegments(forwardSegment, seg1);
                }
                //если же только передний - то присоединяем к нему belt сзади
                else if (forwardSegment != null &&  forwardSegment.GetInputCell == beltBuild.transform.Cell)
                {
                    _segmentSystem.AddBeltsToSegment(forwardSegment, new BeltBuild[] { beltBuild });
                }
                // если ничего - создаем новый сегмент
                else
                    _segmentSystem.CreateNewSegment(new BeltBuild[] { beltBuild });
            }
        }

        private BeltsSegment GetSegmentAtCell(Vector2Int cell)
        {
            if (!_world.world.TryGetBuild(cell, out var build))
                return null;

            if (build is not BeltBuild belt)
                return null;

            return _segmentSystem.GetSegment(belt.SegmentID);
        }

        public void FixedTick(float tickTime)
        {
            _segmentSystem.Tick(tickTime);
        }
        
        public void Dispose()
        {
            TickSystem.OnFixedTick -= FixedTick;
        }
    }
}