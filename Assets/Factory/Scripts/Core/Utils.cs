using UnityEngine;

namespace Factory
{
    public static class Utils
    {
        public static readonly Vector2Int[] DirVectors =
        {
            new(0, 1),   // Up
            new(1, 0),   // Right
            new(0, -1),  // Down
            new(-1, 0)   // Left
        };
        
        public static Vector2 GetVectorDirection(BeltDirections directions) => DirVectors[(int)directions];
        //public static Vector2Int GetOutputCell(Belt belt) => belt.cell + DirVectors[(int)belt.outputDirection];
        
        public static BeltShapes GetTurn(Belt belt)
        {
            Vector2Int input = DirVectors[(int)belt.inputDirection];
            Vector2Int output = DirVectors[(int)belt.outputDirection];

            if (input == output)
                return BeltShapes.Straight;

            int cross = input.x * output.y - input.y * output.x;

            if (cross > 0)
                return BeltShapes.CornerLeft;
            else
                return BeltShapes.CornerRight;
        }
    }

    public enum BeltShapes { Straight, CornerLeft, CornerRight }
}