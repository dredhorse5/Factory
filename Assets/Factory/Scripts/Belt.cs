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
public enum BeltDirections { Up, Right, Down, Left };