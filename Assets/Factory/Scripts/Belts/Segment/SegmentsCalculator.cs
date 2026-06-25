using System;

namespace Factory
{
    public static class SegmentsCalculator
    {
        /// ⬅a⬅ + ⬅b⬅ = ⬅ab⬅
        public static BeltsSegment MergeSegments(BeltsSegment a, BeltsSegment b)
        {
            var ad = a.Data;
            var bd = b.Data;
            
            int size = ad.size + bd.size;
            float speed = ad.speed;
            BeltsSegmentData.Item[] items = new BeltsSegmentData.Item[size * BeltSettings.MaxItemsInTile];

            int k = 0;
            int j = 0;
            var end = ad.count + ad.head;
            for (var i = ad.head; i < end; i++)
            {
                j = i % ad.items.Length;
                items[k] = ad.items[j];
                items[k].Progress += bd.size;
                k++;
            }
            
            end = bd.count + bd.head;
            for (var i = bd.head; i < end; i++)
            {
                j = i % bd.items.Length;
                items[k] = bd.items[j];
                k++;
            }
            
            int count = ad.count + bd.count;
            int head = 0;
            
            var data = new BeltsSegmentData(size, speed, items, count, head);
            
            
            var b1 = a.Belts;
            var b2 = b.Belts;
            
            BeltBuild[] belts = new BeltBuild[b1.Length + b2.Length];
            
            b1.CopyTo(belts, 0);
            b2.CopyTo(belts, b1.Length);

            return new BeltsSegment(data, belts);
        }

        public static BeltsSegment CreateNewSegment(BeltBuild[] belts)
        {
            int size = belts.Length;
            float speed = BeltSettings.Speed;
            var items = new BeltsSegmentData.Item[size * BeltSettings.MaxItemsInTile];
            int count = 0;
            int head = 0;
            
            var data = new BeltsSegmentData(size, speed, items, count, head);
            return new BeltsSegment(data, belts);
        }
        public static BeltsSegment AddBeltsToSegment(BeltsSegment segment, BeltBuild[] belts)
        {
            return MergeSegments(segment, CreateNewSegment(belts));
        }
        public static BeltsSegment AddBeltsToSegment(BeltBuild[] belts, BeltsSegment segment)
        {
            return MergeSegments(CreateNewSegment(belts), segment);
        }
    }
}