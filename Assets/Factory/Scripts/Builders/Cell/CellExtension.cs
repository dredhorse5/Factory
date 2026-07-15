using System;
using System.Collections.Generic;

public static class CellExtensions
{
    public static readonly Cell Up = new(0, 1);
    public static readonly Cell Right = new(1, 0);
    public static readonly Cell Down = new(0, -1);
    public static readonly Cell Left = new(-1, 0);

    public static readonly Cell[] Directions4 = { Up, Right, Down, Left };
    
    public static readonly Cell UpLeft = Up + Left;
    public static readonly Cell UpRight = Up + Right;
    public static readonly Cell DownLeft = Down + Left;
    public static readonly Cell DownRight = Down + Right;

    public static readonly Cell[] Directions8 = { Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft };

    public static Cell UpCell(this Cell cell) => cell + Up;
    public static Cell UpRightCell(this Cell cell) => cell + UpRight;
    public static Cell RightCell(this Cell cell) => cell + Right;
    public static Cell DownRightCell(this Cell cell) => cell + DownRight;
    public static Cell DownCell(this Cell cell) => cell + Down;
    public static Cell DownLeftCell(this Cell cell) => cell + DownLeft;
    public static Cell LeftCell(this Cell cell) => cell + Left;
    public static Cell UpLeftCell(this Cell cell) => cell + UpLeft;

    public static Cell[] GetNeighbours4(this Cell cell)
    {
        return new[]
        {
            cell + Up,
            cell + Right,
            cell + Down,
            cell + Left
        };
    }

    public static Cell GetNeighbour(this Cell cell, CellDirections direction, bool reverse = false)
    {
        if (reverse)
        {
            return direction switch
            {
                CellDirections.Up => cell - Up,
                CellDirections.Right => cell - Right,
                CellDirections.Down => cell - Down,
                CellDirections.Left => cell - Left,
                _ => cell
            };
        }
        return direction switch
        {
            CellDirections.Up => cell + Up,
            CellDirections.Right => cell + Right,
            CellDirections.Down => cell + Down,
            CellDirections.Left => cell + Left,
            _ => cell
        };
    }
    
    /// <summary>
    /// A lot of allocations, don't recommended to use
    /// </summary>
    public static Cell[] GetNeighbours(this Cell cell, bool diagonals = false)
    {
        Cell[] cells = new Cell[diagonals ? 8 : 4];
        var dirs = diagonals ? Directions8 : Directions4;

        for (int i = 0; i < dirs.Length; i++)
            cells[i] = cell + dirs[i];
        return cells;
    }
    
    public static void ForEachNeighbour(this Cell cell, bool diagonals, Action<Cell> action)
    {
        action(cell.UpCell());
        if(diagonals) action(cell.UpRightCell());
        action(cell.RightCell());
        if(diagonals) action(cell.DownRightCell());
        action(cell.DownCell());
        if(diagonals) action(cell.DownLeftCell());
        action(cell.LeftCell());
        if(diagonals) action(cell.UpLeftCell());
    }
}