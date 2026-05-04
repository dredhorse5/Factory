using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Belt
{
    public uint id;

    public float speed;
    public int[] items;

    public Vector2Int cell;
    public BeltDirections inputDirection;
    public BeltDirections outputDirection;

    public float progress;
}
public enum BeltDirections {NONE = -1, Up = 0, Right = 1, Down = 2, Left = 3 };