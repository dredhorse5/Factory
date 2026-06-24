using System;
using UnityEngine;

[Serializable]
public class BaseBuild
{
    public readonly uint id;
    public readonly string SoId;
    public BuildTransform transform;

    public BaseBuild(uint id, string soId)
    {
        this.id = id;
        SoId = soId;
    }
    
    public virtual void OnPlaced(){}
}