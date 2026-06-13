using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Factory
{
    public class BeltSystem : IDisposable
    {
        private readonly World _world;
        private List<BeltsSegment> beltsSegments;

        public BeltSystem(World world)
        {
            _world = world;
            TickSystem.OnTick += Tick;
        }

        private void CreateNewSegment()
        {
            
        }
        
        public void Tick(float tickTime)
        {
            foreach (var beltsSegment in beltsSegments)
                beltsSegment.Tick(tickTime);
        }
        
        public void Dispose()
        {
            TickSystem.OnTick -= Tick;
        }
    }
}