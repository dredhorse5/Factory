using UnityEngine;

public static class BeltUtils
{
    public static readonly Cell[] DirVectors =
    {
        new(0, 1),   // Up
        new(1, 0),   // Right
        new(0, -1),  // Down
        new(-1, 0)   // Left
    };
    
    public static Cell GetOutputCell(this BeltBuild belt) => belt.transform.Cell + DirVectors[(int)belt.belt.OutputConnections[0].Direction];
    public static Cell GetInputCell(this BeltBuild belt) => belt.transform.Cell - DirVectors[(int)belt.belt.InputConnections[0].Direction];
    public static Cell GetCellFromRight(this BeltBuild belt) => belt.transform.Cell + DirVectors[((int)belt.belt.InputConnections[0].Direction + 1) % 4];
    public static Cell GetCellFromLeft(this BeltBuild belt) => belt.transform.Cell + DirVectors[((int)belt.belt.OutputConnections[0].Direction + 3) % 4];
}

public enum BeltShapes {CornerLeft = -1, Straight = 0, CornerRight = 1 }
