using System;
using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    public class BeltsSegmentSystem : IDisposable
    {
        private readonly Dictionary<uint, BeltsSegment> segments = new();
        public IEnumerable<BeltsSegment> Segments => segments.Values;

        private uint lastBeltSegmentId;
        
        public event Action<BeltsSegment> OnSegmentCreated;
        public event Action<BeltsSegment> OnSegmentRemoved;
        
        private readonly DirtyCellsQueue dirtyCells = new();
        
        public void MarkDirtyCell(Cell cell) => dirtyCells.Enqueue(cell);

        public BeltsSegmentSystem()
        {
            TickSystem.OnFixedTick += Tick;
        }

        #region Registe/Unregister

        

        public uint RegisterSegment(BeltsSegment segment)
        {
            lastBeltSegmentId++;

            segment.SetID(lastBeltSegmentId);

            segments.Add(lastBeltSegmentId, segment);
            OnSegmentCreated?.Invoke(segment);

            return lastBeltSegmentId;
        }

        public void ReRegisterSegment(BeltsSegment oldSegment, BeltsSegment newSegment)
        {
            if (oldSegment.Id == 0)
                throw new InvalidOperationException(
                    "Cannot re-register segment with id 0");

            if (newSegment.Id != 0)
                throw new InvalidOperationException(
                    "New segment must not be registered");

            newSegment.SetID(oldSegment.Id);

            OnSegmentRemoved?.Invoke(oldSegment);
            segments[oldSegment.Id] = newSegment;
            OnSegmentCreated?.Invoke(newSegment);
        }

        public void UnregisterSegment(BeltsSegment segment) => UnregisterSegment(segment.Id);
        public void UnregisterSegment(uint id)
        {
            if (!segments.TryGetValue(id, out var segment))
                throw new InvalidOperationException(
                    $"Segment {id} not found");
            OnSegmentRemoved?.Invoke(segment);
            segments.Remove(id);
        }

        
        
        #endregion
        
        
        
        public BeltsSegment GetSegment(uint id)
        {
            if (id == 0)
                return null;

            segments.TryGetValue(id, out var segment);

            return segment;
        }

        public bool TryGetSegment(uint id, out BeltsSegment segment)
        {
            return segments.TryGetValue(id, out segment);
        }


        #region Calculation

        


        /// ⬅a⬅ + ⬅b⬅ = ⬅ab⬅
        public BeltsSegment MergeSegments(BeltsSegment a, BeltsSegment b)
        {
            var newSegment = SegmentsCalculator.MergeSegments(a, b);
            UnregisterSegment(b);
            ReRegisterSegment(a, newSegment);
            return newSegment;
        }

        public BeltsSegment CreateNewSegment(BeltBuild[] belts)
        {
            var seg = SegmentsCalculator.CreateNewSegment(belts);
            RegisterSegment(seg);
            return seg;
        }

        public BeltsSegment AddBeltsToSegment(BeltsSegment segment, BeltBuild[] belts)
        {
            var seg = SegmentsCalculator.AddBeltsToSegment(segment, belts);
            ReRegisterSegment(segment, seg);
            return seg;
        }
        public BeltsSegment AddBeltsToSegment(BeltBuild[] belts, BeltsSegment segment)
        {
            var seg = SegmentsCalculator.AddBeltsToSegment(belts, segment);
            ReRegisterSegment(segment, seg);
            return seg;
        }

        public void RemoveBeltFromSegment(uint segmentId, BeltBuild byBelt)
        {
            if(segments.TryGetValue(segmentId, out var segment))
                RemoveBeltFromSegment(segment, byBelt);
            else
                Debug.LogWarning($"Segment {segmentId} not found");
        }

        public void RemoveBeltFromSegment(BeltsSegment segment, BeltBuild byBelt)
        {
            if(segment.Data.size == 1)
            {
                UnregisterSegment(segment);
                return;
            }
            var newSegments = SegmentsCalculator.RemoveBelt(segment, byBelt);
            ReRegisterSegment(segment, newSegments[0]);
            if (newSegments.Length == 2)
                RegisterSegment(newSegments[1]);
        }

        
        
        #endregion
        
        
        
        public void Tick(float dt)
        {
            foreach (var segment in segments.Values)
                segment.Tick(dt);
        }

        public void Dispose()
        {
            TickSystem.OnFixedTick -= Tick;
        }
    }
}