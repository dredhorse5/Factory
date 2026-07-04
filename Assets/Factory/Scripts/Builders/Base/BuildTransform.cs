
using UnityEngine;

public struct BuildTransform
{
    public Cell Cell;
    public BuildRotations Rotation;
    public Vector2Int Size;

    public BuildTransform(Cell position, BuildRotations rotation, Vector2Int size) =>
        (Cell, Rotation, Size) = (position, rotation, size);
    
    public Vector2Int GetRotatedSize => Rotation is BuildRotations.R90 or BuildRotations.R270 ? new Vector2Int(Size.y, Size.x) : Size;

    public Cell[] GetOccupiedTiles() => BuildTransformCalculator.GetOccupiedTiles(Cell, Rotation, Size);
}

public static class BuildTransformCalculator
{
    public static Cell[] GetOccupiedTiles(Cell cell, BuildRotations rotation, Vector2Int size)
    {
        var realSize = rotation is BuildRotations.R90 or BuildRotations.R270
            ? new Vector2Int(size.y, size.x)
            : size;
        Cell[] array = new Cell[realSize.x * realSize.y];
        for (int x = 0; x < realSize.x; x++)
        for (int y = 0; y < realSize.y; y++)
            array[x * realSize.y + y] = cell + new Cell(x, y);
        return array;
    }
}

public struct BuildPlacement
{
    public Cell Cell;
    public BuildRotations Rotation;

    public BuildPlacement(int x, int y, BuildRotations rotation) => (Cell, Rotation) = (new Cell(x, y), rotation);
    public Cell[] GetOccupiedTiles(Vector2Int size) => BuildTransformCalculator.GetOccupiedTiles(Cell, Rotation, size);
}

public enum BuildRotations {R0, R90, R180, R270}