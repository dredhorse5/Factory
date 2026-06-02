using System;
using UnityEngine;

public class Map : MonoBehaviour
{
    public float CellSize = 1f;

    public Vector3 GetPosition(Vector2Int cell)
    {
        return new Vector3(cell.x * CellSize, 0, cell.y * CellSize);
    }

}