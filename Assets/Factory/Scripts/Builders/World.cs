using System;
using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    [Serializable]
    public class World
    {
        public Dictionary<uint, BaseBuild> Builds = new Dictionary<uint, BaseBuild>();
        public uint[,] cells;
        public uint[,] terrain;
        
        private uint nextBuildId = 0;

        public World(int sizex, int sizey)
        {
            cells = new uint[sizex , sizey];
        }
        
        public uint GetNextBuildId() => ++nextBuildId;
        
        public bool TryGetBuildID(Cell cell, out uint id)
        {
            id = GetBuildID(cell);
            return id > 0;
        }

        public uint GetBuildID(Cell cell)
        {
            if (cell.x > cells.GetLength(0) || cell.y > cells.GetLength(1) || cell.x < 0 || cell.y < 0)
                return 0;
            return cells[cell.x, cell.y];
        }
        public bool TryGetBuild(uint id, out BaseBuild build) => Builds.TryGetValue(id, out build);
        public bool TryGetBuild(Cell pos, out BaseBuild build)
        {
            if (TryGetBuildID(pos, out uint id))
                return TryGetBuild(id, out build);

            build = null;
            return false;
        }

        public bool HasBuild(Cell cell) => HasBuild(cell.x, cell.y);
        public bool HasBuild(int x, int y)
        {
            if(x < 0 || x >= cells.GetLength(0) || y < 0 || y >= cells.GetLength(1))
                return false;
            return cells[x, y] > 0;
        }

        public bool IsAreaFree(Cell[] cells)
        {
            for (var i = 0; i < cells.Length; i++)
                if (HasBuild(cells[i]) || terrain[cells[i].x,cells[i].y] == 0)
                    return false;
            return true;
        }
    }
}