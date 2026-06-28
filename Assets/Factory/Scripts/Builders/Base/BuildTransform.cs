
using UnityEngine;

public struct BuildTransform
{
    public Vector2Int Cell;
    public BuildRotations Rotation;
    public Vector2Int Size;

    public BuildTransform(Vector2Int position, BuildRotations rotation, Vector2Int size) =>
        (Cell, Rotation, Size) = (position, rotation, size);
    
    public Vector2Int GetRotatedSize => Rotation is BuildRotations.R90 or BuildRotations.R270 ? new Vector2Int(Size.y, Size.x) : Size;

    public Vector2Int[] GetOccupiedTiles() => BuildTransformCalculator.GetOccupiedTiles(Cell, Rotation, Size);
}

public static class BuildTransformCalculator
{
    public static Vector2Int[] GetOccupiedTiles(Vector2Int cell, BuildRotations rotation, Vector2Int size)
    {
        var realSize = rotation is BuildRotations.R90 or BuildRotations.R270
            ? new Vector2Int(size.y, size.x)
            : size;
        Vector2Int[] array = new Vector2Int[realSize.x * realSize.y];
        for (int x = 0; x < realSize.x; x++)
        for (int y = 0; y < realSize.y; y++)
            array[x * realSize.y + y] = cell + new Vector2Int(x, y);
        return array;
    }
}

public struct BuildPlacement
{
    public Vector2Int Cell;
    public BuildRotations Rotation;

    public BuildPlacement(int x, int y, BuildRotations rotation) => (Cell, Rotation) = (new Vector2Int(x, y), rotation);
    public Vector2Int[] GetOccupiedTiles(Vector2Int size) => BuildTransformCalculator.GetOccupiedTiles(Cell, Rotation, size);
}

public enum BuildRotations {R0, R90, R180, R270}