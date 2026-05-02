using System;
using UnityEngine;

namespace Factory
{
    public class Map : MonoBehaviour
    {
        public float CellSize = 1f;
        public static Map Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }

        public Vector3 GetPosition(Vector2Int cell)
        {
            return new Vector3(cell.x * CellSize, 0, cell.y * CellSize);
        }

    }
}