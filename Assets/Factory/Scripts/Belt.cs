using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Belt
{
    public float speed;
    public float progress;
    public int[] items;
    
    public uint id;
    public uint previousBeltId;
    public uint nextBeltId;

    public BeltTopology topology;
}

[Serializable]
public struct BeltTopology
{
    public Cell Start;
    public Cell End;

    [Serializable]
    public struct Cell
    {
        public Vector2Int pos;
        public enum Rotations { R0, R90, R180, R270 };
        public Rotations Rotation;
    }
}
