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
            Item[] items = new Item[size * BeltSettings.MaxItemsInTile];

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
            var items = new Item[size * BeltSettings.MaxItemsInTile];
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

        public static BeltsSegment[] RemoveBelt(BeltsSegment segment, BeltBuild byBelt)
        {
            if(segment.Belts.Length == 1) 
                return Array.Empty<BeltsSegment>();
            if(segment.Belts[^1] == byBelt)
                return new[] { RemoveBeltAtSide(segment, BeltRemoveSide.Last) };
            else if(segment.Belts[0] == byBelt)
                return new[] { RemoveBeltAtSide(segment, BeltRemoveSide.First) };
            return SeparateSegment(segment, byBelt);
        }
        public static BeltsSegment[] SeparateSegment(BeltsSegment segment, BeltBuild byBelt)
        {
            int splitIndex = Array.IndexOf(segment.Belts, byBelt);

            if (splitIndex <= 0 || splitIndex >= segment.Belts.Length - 1)
                throw new ArgumentException("Invalid split belt.", nameof(byBelt));

            var leftBelts = new BeltBuild[splitIndex];
            var rightBelts = new BeltBuild[segment.Belts.Length - splitIndex - 1];

            Array.Copy(segment.Belts, 0, leftBelts, 0, leftBelts.Length);
            Array.Copy(segment.Belts, splitIndex + 1, rightBelts, 0, rightBelts.Length);

            var leftSegment = CreateNewSegment(leftBelts);
            var rightSegment = CreateNewSegment(rightBelts);

            var src = segment.Data;
            var left = leftSegment.Data;
            var right = rightSegment.Data;

            int rightSize = right.size;
            int boundary = rightSize + 1; // +1 за счет удаляемого конвейера

            int end = src.head + src.count;
            for (int i = src.head; i < end; i++)
            {
                int j = i % src.items.Length;
                var item = src.items[j];

                if (item.Progress <= rightSize)
                {
                    // Предмет остается в правом сегменте
                    right.items[right.count++] = item;
                }
                else if (item.Progress <= boundary)
                {
                    // Предмет находился на удаляемом конвейере
                    continue;
                }
                else if(left.count < left.items.Length)
                {
                    // Предмет остается в левом сегменте
                    item.Progress -= boundary;
                    left.items[left.count++] = item;
                }
            }

            return new[]
            {
                leftSegment,
                rightSegment
            };
        }
        public static BeltsSegment RemoveBeltAtSide(BeltsSegment segment, BeltRemoveSide size)
        {
            var belts = new BeltBuild[segment.Belts.Length - 1];

            if (size == BeltRemoveSide.First)
                Array.Copy(segment.Belts, 1, belts, 0, belts.Length);
            else
                Array.Copy(segment.Belts, 0, belts, 0, belts.Length);

            var result = CreateNewSegment(belts);

            var src = segment.Data;
            var dst = result.Data;

            int end = src.head + src.count;
            int newSize = dst.size;

            for (int i = src.head; i < end; i++)
            {
                if(dst.items.Length <= i)
                    break;
                int j = i % src.items.Length;
                var item = src.items[j];

                if (size == BeltRemoveSide.First)
                {
                    // предмет находился на удаляемом конвейере
                    if (item.Progress >= newSize)
                        continue;

                }
                else
                {
                    // предмет находился на удаляемом конвейере
                    if (item.Progress < 1)
                        continue;
                    item.Progress--;
                }

                dst.items[dst.count++] = item;
            }

            return result;
        }
    }
    
    public enum BeltRemoveSide { First, Last }
}