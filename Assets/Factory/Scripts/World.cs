using System;
using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    [Serializable]
    public class World
    {
        public Dictionary<uint, Belt> Belts = new Dictionary<uint, Belt>();
        public Dictionary<Vector2Int, uint> OccupiedTiles = new Dictionary<Vector2Int, uint>();
        
        private uint NextBeltId = 0;
        
        public uint GetNextBeltId() => ++NextBeltId;
    }
}