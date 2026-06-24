using UnityEngine;

namespace Factory
{
    public static class BeltUtils
    {
        public static readonly Vector2Int[] DirVectors =
        {
            new(0, 1),   // Up
            new(1, 0),   // Right
            new(0, -1),  // Down
            new(-1, 0)   // Left
        };
        
        public static Vector2 GetVectorDirection(BeltDirections directions) => DirVectors[(int)directions];
        public static Vector2Int GetOutputCell(this BeltBuild belt) => belt.transform.Position + DirVectors[(int)belt.OutputDirection];
        public static Vector2Int GetInputCell(this BeltBuild belt) => belt.transform.Position - DirVectors[(int)belt.InputDirection];
        
        public static BeltShapes GetTurn(BeltBuild beltBuild)
        {
            Vector2Int input = DirVectors[(int)beltBuild.InputDirection];
            Vector2Int output = DirVectors[(int)beltBuild.OutputDirection];

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