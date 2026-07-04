using System;
using UnityEngine;

[Serializable]
public class BaseBuild
{
    public readonly uint id;
    public readonly string SoId;
    public BuildTransform transform;
    private readonly BaseBuildData data;
    

    public BaseBuild(uint id, string soId, BaseBuildData data)
    {
        this.id = id;
        this.SoId = soId;
        this.data = data;
    }

    public T GetData<T>() where T : BaseBuildData => data as T;
    public BaseBuildData GetData() => data;
    
    public virtual void OnPlaced(){}
    public virtual void OnDestroyed(){}
}