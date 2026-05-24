using System;
using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    public class BeltSystem : IDisposable
    {
        private readonly World _world;

        public BeltSystem(World world)
        {
            _world = world;
            TickSystem.OnTick += Tick;
        }
        
        public void Tick(float tickTime)
        {
            foreach (var belt in _world.Belts.Values)
                TransferItems(belt);
            foreach (var belt in _world.Belts.Values)
                MoveItems(belt, tickTime);
        }

        private void TransferItems(Belt belt)
        {
            if (belt.itemToTransfer > 0)
            {
                if(_world.GetOccupiedTileID(Utils.GetOutputCell(belt), out var nextBeltID))
                {
                    if (nextBeltID > 0)
                    {
                        var nextBelt = _world.Belts[nextBeltID];

                        if (nextBelt.items[0] == 0)
                        {
                            nextBelt.items[0] = belt.itemToTransfer;
                            nextBelt.progress[0] = 0f;
                            belt.itemToTransfer = 0;
                        }
                    }
                }
            }
        }

        private void MoveItems(Belt belt, float tickTime)
        { 
            for (var i = 0; i < belt.progress.Length; i++)
                if (belt.items[i] != 0)
                    belt.progress[i] += tickTime * belt.speed;

            for (var i = belt.items.Length - 1; i >= 0; i--)
            {
                if (belt.progress[i] > 1f)
                {
                    if (i == belt.items.Length - 1)
                    {
                        if (belt.itemToTransfer == 0)
                        {
                            belt.itemToTransfer = belt.items[i];
                            belt.items[i] = 0;
                            belt.progress[i] = 0f;
                        } 
                        else
                            belt.progress[i] = 1f;
                    }
                    else
                    {
                        if(belt.items[i + 1] == 0)
                        {
                            belt.items[i + 1] = belt.items[i];
                            belt.items[i] = 0;
                            belt.progress[i] = 0f;
                        }
                        else
                            belt.progress[i] = 1f;
                    }
                }
            }
        }
        
        public void Dispose()
        {
            TickSystem.OnTick -= Tick;
        }
    }
}