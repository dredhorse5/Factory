using System;

[Serializable]
public readonly struct Cell : IEquatable<Cell>
{
    public readonly int x;
    public readonly int y;

    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    

    public static readonly Cell Zero = new(0, 0);

    public static Cell operator +(Cell a, Cell b) => new(a.x + b.x, a.y + b.y);

    public static Cell operator -(Cell a, Cell b) => new(a.x - b.x, a.y - b.y);

    public static Cell operator *(Cell a, int value) => new(a.x * value, a.y * value);

    public static Cell operator /(Cell a, int value) => new(a.x / value, a.y / value);

    public static bool operator ==(Cell left, Cell right) => left.Equals(right);

    public static bool operator !=(Cell left, Cell right) => !left.Equals(right);

    public bool Equals(Cell other) => x == other.x && y == other.y;

    public override bool Equals(object obj) => obj is Cell other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(x, y);

    public override string ToString() => $"({x}, {y})";
}

public enum CellDirections {NONE = -1, Up = 0, Right = 1, Down = 2, Left = 3 };