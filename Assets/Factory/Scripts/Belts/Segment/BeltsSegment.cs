using System;
using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    public class BeltsSegment : IItemInput
    {
        private uint id;
        private readonly BeltsSegmentData data;
        private readonly BeltBuild[] belts;

        private IItemInput output; // для сегмента конвейеров выход - это вход какого-нибудь завода
        private IItemOutput input; // и наоборот - для конвейеров это вход

        public Vector2Int GetOutputCell => belts[^1].GetOutputCell();
        public Vector2Int GetInputCell => belts[0].GetInputCell();

        public BeltsSegmentData Data => data;
        public uint Id => id;
        public BeltBuild[] Belts => belts; 
        
        public void SetID(uint id)
        {
            for (var i = 0; i < belts.Length; i++)
                belts[i].SegmentID = id;

            this.id = id;
        }

        public BeltsSegment(BeltsSegmentData data, BeltBuild[] belts)
        {
            this.data = data;
            this.belts = belts;
        }

        public void Tick(float dt)
        {
            int j = 0;
            var end = data.count + data.head;
            for (var i = data.head; i < end; i++)
            {
                j = i % data.items.Length;
                data.items[j].Progress += dt * data.speed;
                if (i == data.head) // претендент на выход
                {
                    if (data.items[j].Progress >= data.size)
                    {
                        if (output.TryAccept(data.items[j].Id, data.items[j].Progress - data.size))
                        {
                            data.items[j].Id = 0;
                            data.items[j].Progress = 0;
                            data.head++;
                            data.count--;
                            data.head %= data.items.Length;
                        }
                    }
                }
                else
                {
                    var maxProg = data.items[(j - 1 + data.items.Length) % data.items.Length].Progress - BeltSettings.MinDistanceBetweenItems;
                    if (data.items[j].Progress > maxProg)
                        data.items[j].Progress = maxProg;
                }
            }
        }
        
        
        public bool CanAccept(ushort item, float progress)
        {
            if(data.size == 0 || data.count == data.items.Length)
                return false;
            if (data.count == 0)
                return true;
            
            //var tail = (data.head + data.count) % data.items.Length;
            var lastItem = (data.head + data.count - 1 + data.items.Length) % data.items.Length;
            if(data.items[lastItem].Progress - BeltSettings.MinDistanceBetweenItems > 0)
                return true;
            return false;
        }

        public bool TryAccept(ushort item, float progress)
        {
            if (CanAccept(item, progress))
            {
                var tail = (data.head + data.count) % data.items.Length;
                if (data.count == 0)
                    data.items[tail] = new BeltsSegmentData.Item() { Id = item, Progress = progress };
                else
                {
                    var lastItem = (data.head + data.count - 1 + data.items.Length) % data.items.Length;
                    var maxDist = data.items[lastItem].Progress - BeltSettings.MinDistanceBetweenItems;
                    data.items[tail] = new BeltsSegmentData.Item() { Id = item, Progress = progress > maxDist? maxDist : progress };
                }

                data.count++;
                return true;
            }
            return false;
        }
    }
    
}