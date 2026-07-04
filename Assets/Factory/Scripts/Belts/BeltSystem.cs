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
        private readonly Queue<BeltBuild> dirtyBelts = new();
        [Inject]
        public BeltSystem(WorldProvider world, BuildSystem buildSystem, BeltsSegmentSystem segmentSystem)
        {
            _world = world;
            _buildSystem = buildSystem;
            _segmentSystem = segmentSystem;

            _buildSystem.OnBuildCreated += OnNewBuild;
            _buildSystem.OnBuildWillDestroy += OnBuildDestroy;
            TickSystem.OnFixedTick += FixedTick;
        }



        private void OnNewBuild(uint obj, BaseBuild build)
        {
            if (build is BeltBuild belt)
            {
                MarkDirty(belt);
                
                belts.Add(obj, belt);
                
                /*var backSegment = GetSegmentAtCell(belt.GetInputCell());
                var forwardSegment = GetSegmentAtCell(belt.GetOutputCell());
                
                // есть сегмент сзади, то добавляем к нему новый belt
                if (backSegment != null && backSegment.GetOutputCell == belt.transform.Cell)
                {
                    var seg1 = _segmentSystem.AddBeltsToSegment(new BeltBuild[] { belt }, backSegment);
                    //а если есть и передний, то соединяем их
                    if(forwardSegment != null && forwardSegment.GetInputCell == belt.transform.Cell)
                        _segmentSystem.MergeSegments(forwardSegment, seg1);
                }
                //если же только передний - то присоединяем к нему belt сзади
                else if (forwardSegment != null &&  forwardSegment.GetInputCell == belt.transform.Cell)
                {
                    _segmentSystem.AddBeltsToSegment(forwardSegment, new BeltBuild[] { belt });
                }
                // если ничего - создаем новый сегмент
                else
                    _segmentSystem.CreateNewSegment(new BeltBuild[] { belt });

                TryRotateBackwardBelt(belt);*/
            }
        }

        private void TryRotateBackwardBelt(BeltBuild toBelt)
        {
            var cell = toBelt.GetInputCell();
            var buildid = _world.world.cells[cell.x,cell.y];
            if (buildid > 0)
            {
                if(_world.world.TryGetBuild(buildid, out var build))
                {
                    if (build is BeltBuild belt)
                    {
                        var cellFromRight = belt.GetCellFromRight();
                        var cellFromLeft = belt.GetCellFromLeft();
                        
                        if (cellFromRight == toBelt.transform.Cell)
                        {
                            var data = belt.beltData;
                            data.Shape = BeltShapes.CornerRight;
                            _buildSystem.ReconfigureBuild(belt, data);
                        }
                        else if (cellFromLeft == toBelt.transform.Cell)
                        {
                            var data = belt.beltData;
                            data.Shape = BeltShapes.CornerLeft;
                            _buildSystem.ReconfigureBuild(belt, data);
                        }
                    }
                }
            }
        }
        
        private void OnBuildDestroy(uint arg1, BaseBuild arg2)
        {
            if (belts.TryGetValue(arg1, out var belt))
            {
                belts.Remove(arg1);
                _segmentSystem.RemoveBeltFromSegment(belt.SegmentID, belt);
            }
        }

        private BeltsSegment GetSegmentAtCell(Cell cell)
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

        private void TryAddDirty(Cell cell)
        {
            
        }
        private void MarkDirty(BeltBuild belt)
        {
            if(!dirtyBelts.Contains(belt))
                dirtyBelts.Enqueue(belt);
        }
        public void Dispose()
        {
            _buildSystem.OnBuildCreated -= OnNewBuild;
            _buildSystem.OnBuildWillDestroy -= OnBuildDestroy;
            TickSystem.OnFixedTick -= FixedTick;
        }
    }
}