using System;
using UnityEngine;

[Serializable]
public class BaseBuild
{
    public readonly uint id;
    public readonly string SoId;
    public BuildTransform transform;
    protected IBuildData data;


    public BaseBuild(uint id, string soId, IBuildData data)
    {
        this.id = id;
        this.SoId = soId;
        SetData(data);
    }

    public T GetData<T>() where T : class, IBuildData => data as T;
    public IBuildData GetData() => data;
    public virtual void SetData(IBuildData data) => this.data = data;
    
    public virtual void OnPlaced() { }
    public virtual void OnDestroyed() { }
    public virtual void OnReconfigured(){}

}

public interface IBuildData { }