using System;
using System.Collections.Generic;

namespace Factory
{
    public class BeltsSegment : IItemInput
    {
        private int size;
        private readonly int speed;
        private Item[] items;
        private int count;
        private int head;

        private IItemInput output; // для сегмента конвейеров выход - это вход какого-нибудь завода
        private IItemOutput input; // и наоборот - для конвейеров это вход

        public BeltsSegment(int size, int speed)
        {
            this.size = size;
            this.speed = speed;
            items = new Item[size * BeltSettings.MaxItemsInTile];
        }

        public void Tick(float dt)
        {
            int j = 0;
            var end = count + head;
            for (var i = head; i < end; i++)
            {
                j = i % items.Length;
                items[j].Progress += dt * speed;
                if (i == head) // претендент на выход
                {
                    if (items[j].Progress >= size)
                    {
                        if (output.TryAccept(items[j].Id, items[j].Progress - size))
                        {
                            items[j].Id = 0;
                            items[j].Progress = 0;
                            head++;
                            count--;
                            head %= items.Length;
                        }
                    }
                }
                else
                {
                    var maxProg = items[(j - 1 + items.Length) % items.Length].Progress - BeltSettings.MinDistanceBetweenItems;
                    if (items[j].Progress > maxProg)
                        items[j].Progress = maxProg;
                }
            }
        }
        
        
        public bool CanAccept(ushort item, float progress)
        {
            if(size == 0 || count == items.Length)
                return false;
            if (count == 0)
                return true;
            
            var tail = (head + count) % items.Length;
            var lastItem = (head + count - 1 + items.Length) % items.Length;
            if(items[lastItem].Progress - BeltSettings.MinDistanceBetweenItems > 0)
                return true;
            return false;
        }

        public bool TryAccept(ushort item, float progress)
        {
            if (CanAccept(item, progress))
            {
                var tail = (head + count) % items.Length;
                if (count == 0)
                    items[tail] = new Item() { Id = item, Progress = progress };
                else
                {
                    var lastItem = (head + count - 1 + items.Length) % items.Length;
                    var maxDist = items[lastItem].Progress - BeltSettings.MinDistanceBetweenItems;
                    items[tail] = new Item() { Id = item, Progress = progress > maxDist? maxDist : progress };
                }

                count++;
                return true;
            }
            return false;
        }
        

        
        [Serializable]
        private struct Item
        {
            public ushort Id;
            public float Progress;
        }

    }
    
}