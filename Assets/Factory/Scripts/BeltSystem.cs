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
                TickBelt(belt, tickTime);
        }

        private void TickBelt(Belt belt, float tickTime)
        {
            belt.progress += tickTime * belt.speed;

            if (belt.progress >= 1f)
            {
                belt.progress -= 1f;

                // внутреннее движение
                for (var i = belt.items.Length - 1; i > 0; i--)
                {
                    if (belt.items[i] == 0)
                    {
                        belt.items[i] = belt.items[i - 1];
                        belt.items[i - 1] = 0;
                    }
                }

                // transfer (временно
                if (belt.items[^1] != 0 && belt.nextBeltId > 0)
                {
                    var next = _world.Belts[belt.nextBeltId];

                    if (next.items[0] == 0)
                    {
                        next.items[0] = belt.items[^1];
                        belt.items[^1] = 0;
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