using System;

namespace Factory
{
    public static class SegmentsCalculator
    {
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
                k++;
            }
            
            end = bd.count + bd.head;
            for (var i = bd.head; i < end; i++)
            {
                j = i % bd.items.Length;
                items[k] = bd.items[j];
                items[k].Progress += ad.size;
                k++;
            }
            
            int count = ad.count + bd.count;
            int head = 0;
            
            var data = new BeltsSegmentData(size, speed, items, count, head);
            var b1 = a.Belts;
            var b2 = b.Belts;
            BeltBuild[] belts = new BeltBuild[b1.Length + b2.Length];
            Array.Copy(b1, 0, belts, 0, b1.Length);
            Array.Copy(b2, 0, belts, b1.Length, b2.Length);

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

        public static BeltsSegment AddBeltsToSegment(BeltsSegment segment, BeltBuild[] belts, bool asHead)
        {
            if(asHead)
                return MergeSegments(segment, CreateNewSegment(belts));
            else
                return MergeSegments(CreateNewSegment(belts), segment);
        }
    }
}