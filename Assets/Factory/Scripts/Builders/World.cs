using System;
using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    [Serializable]
    public class World
    {
        public Dictionary<uint, Belt> Belts = new Dictionary<uint, Belt>();
        public Dictionary<uint, BaseBuild> Builds = new Dictionary<uint, BaseBuild>();
        public Dictionary<Vector2Int, uint> OccupiedTiles = new Dictionary<Vector2Int, uint>();
        public uint[,] tiles;
        public uint[,] terrain;
        
        private uint NextBeltId = 0;

        public World(int sizex, int sizey)
        {
            tiles = new uint[sizex , sizey];
        }
        
        public uint GetNextBuildId() => ++NextBeltId;
        
        public bool GetOccupiedTileID(Vector2Int position, out uint id)
        {
            if (OccupiedTiles.ContainsKey(position))
            {
                id = OccupiedTiles[position];
                return true;
            }

            id = 0;
            return false;
        }

        public bool GetBuildID(Vector2Int pos, out uint id)
        {
            if (pos.x > tiles.GetLength(0) || pos.y > tiles.GetLength(1) || pos.x < 0 || pos.y < 0)
            {
                id = 0;
                return false;
            }

            id = tiles[pos.x, pos.y];
            return id > 0;
        }
        public bool GetBuild(uint id, out BaseBuild build) => Builds.TryGetValue(id, out build);
        public bool GetBuild(Vector2Int pos, out BaseBuild build)
        {
            if (GetBuildID(pos, out uint id))
                return GetBuild(id, out build);

            build = null;
            return false;
        }

        public bool HasBuild(Vector2Int pos) => tiles[pos.x, pos.y] > 0;
        public bool HasBuild(int x, int y) => tiles[x, y] > 0;

        public bool IsAreaFree(Vector2Int[] tiles)
        {
            for (var i = 0; i < tiles.Length; i++)
                if (HasBuild(tiles[i]) || terrain[tiles[i].x,tiles[i].y] == 0)
                    return false;
            return true;
        }
    }
}