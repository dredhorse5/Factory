
using UnityEngine;

public struct BuildTransform
{
    public Vector2Int Cell;
    public BuildRotations Rotation;
    public Vector2Int Size;

    public BuildTransform(Vector2Int position, BuildRotations rotation, Vector2Int size) =>
        (Cell, Rotation, Size) = (position, rotation, size);
    
    public Vector2Int GetRotatedSize => Rotation is BuildRotations.R90 or BuildRotations.R270 ? new Vector2Int(Size.y, Size.x) : Size;
}

public struct BuildPlacement
{
    public Vector2Int Position;
    public BuildRotations Rotation;

    public BuildPlacement(int x, int y, BuildRotations rotation) =>
        (Position, Rotation) = (new Vector2Int(x, y), rotation);
    public Vector2Int[] GetOccupiedTiles(Vector2Int staticSize)
    {
        var realSize = Rotation is BuildRotations.R90 or BuildRotations.R270
            ? new Vector2Int(staticSize.y, staticSize.x)
            : staticSize;
        Vector2Int[] array = new Vector2Int[realSize.x * realSize.y];
        for (int x = 0; x < realSize.x; x++)
        for (int y = 0; y < realSize.y; y++)
            array[x * realSize.y + y] = Position + new Vector2Int(x, y);
        return array;
    }
}
public enum BuildRotations {R0, R90, R180, R270}