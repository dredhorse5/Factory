using System;

namespace Factory
{
    public class BeltsSegmentData
    {
        public int size;
        public float speed;
        public Item[] items;
        public int count;
        public int head;

        public BeltsSegmentData(int size, float speed, Item[] items, int count, int head)
        {
            this.size = size;
            this.speed = speed;
            this.items = items;
            this.count = count;
            this.head = head;
        }
        
        [Serializable]
        public struct Item
        {
            public ushort Id;
            public float Progress;
        }
    }
}