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
}
