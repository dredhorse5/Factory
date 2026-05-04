using UnityEngine;

namespace Factory
{
    public static class GridUtils
    {
        public static readonly Vector2Int[] DirVectors =
        {
            new(0, 1),   // Up
            new(1, 0),   // Right
            new(0, -1),  // Down
            new(-1, 0)   // Left
        };

        public static Vector2Int GetOutputCell(Belt belt) => belt.cell + DirVectors[(int)belt.outputDirection];
        
    }
}