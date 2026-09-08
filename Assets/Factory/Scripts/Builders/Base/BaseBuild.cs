using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseBuild
{
    public readonly uint id;
    public readonly string SoId;
    public BuildTransform transform;
    protected IBuildData data;

    private Dictionary<Type, IBuildComponent> _components = new Dictionary<Type, IBuildComponent>();

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

    #region Component
    
    
    public T GetComponent<T>() where T : class, IBuildComponent
    {
        TryGetComponent(out T component);
        return component;
    }

    public bool HasComponent<T>() where T : class, IBuildComponent
    {
        return _components.ContainsKey(typeof(T));
    }

    public bool TryGetComponent<T>(out T component) where T : class, IBuildComponent
    {
        if(_components.TryGetValue((typeof(T)), out var _component))
        {
            component = _component as T;
            return true;
        }
        component = null;
        return false;
    }

    protected bool AddComponent<T>(T component) where T : class, IBuildComponent
    {
        if (_components.ContainsKey(typeof(T)))
        {
            Debug.LogError($"Component {typeof(T)} is already registered. Object id: {id}");
            return false;
        }
        _components.Add(typeof(T), component);
        return true;
    }
    
    
    #endregion
}

public interface IBuildData { }