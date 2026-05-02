using System;
using System.Collections.Generic;

namespace Factory
{
    [Serializable]
    public class World
    {
        public Dictionary<uint, Belt> Belts = new Dictionary<uint, Belt>();
        public uint NextBeltId = 1;
    }
}