using System;
using System.Collections.Generic;

namespace Factory
{
    public class BeltsSegmentSystem
    {
        private readonly Dictionary<uint, BeltsSegment> beltsSegments = new();
        public IEnumerable<BeltsSegment> Segments => beltsSegments.Values;

        private uint lastBeltSegmentId;
        
        public event Action<BeltsSegment> OnSegmentCreated;
        public event Action<BeltsSegment> OnSegmentRemoved;

        public uint RegisterSegment(BeltsSegment segment)
        {
            lastBeltSegmentId++;

            segment.SetID(lastBeltSegmentId);

            beltsSegments.Add(lastBeltSegmentId, segment);
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
            beltsSegments[oldSegment.Id] = newSegment;
            OnSegmentCreated?.Invoke(newSegment);
        }

        public void UnregisterSegment(BeltsSegment segment) => UnregisterSegment(segment.Id);
        public void UnregisterSegment(uint id)
        {
            if (!beltsSegments.TryGetValue(id, out var segment))
                throw new InvalidOperationException(
                    $"Segment {id} not found");
            OnSegmentRemoved?.Invoke(segment);
            beltsSegments.Remove(id);
        }

        public BeltsSegment GetSegment(uint id)
        {
            if (id == 0)
                return null;

            beltsSegments.TryGetValue(id, out var segment);

            return segment;
        }

        public bool TryGetSegment(uint id, out BeltsSegment segment)
        {
            return beltsSegments.TryGetValue(id, out segment);
        }

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

        public void Tick(float dt)
        {
            foreach (var segment in beltsSegments.Values)
                segment.Tick(dt);
        }
    }
}