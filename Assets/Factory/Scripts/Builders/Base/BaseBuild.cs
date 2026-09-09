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

    private List<IBuildComponent> _components = new List<IBuildComponent>();

    public BaseBuild(uint id, string soId, IBuildData data)
    {
        this.id = id;
        this.SoId = soId;
        SetData(data);
    }
    
    public T GetData<T>() where T : class, IBuildData => data as T;
    public IBuildData GetData() => data;
    public virtual void SetData(IBuildData data) => this.data = data;
    

    public void Initialize()
    {
        foreach (var comp in _components)
            comp.Initialize(this);
        OnInitialize();
    }



    public virtual void OnInitialize() { }
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
        return TryGetComponent<T>(out _);
    }

    public bool TryGetComponent<T>(out T component) where T : class, IBuildComponent
    {
        foreach (var _component in _components)
        {
            if (_component is T typedComponent)
            {
                component = typedComponent;
                return true;
            }
        }
        component = null;
        return false;
    }

    protected bool AddComponent<T>(T component) where T : class, IBuildComponent
    {
        if (HasComponent<T>())
        {
            Debug.LogError($"Component {typeof(T)} is already registered. Object id: {id}");
            return false;
        }
        _components.Add(component);
        return true;
    }
    
    
    #endregion
}

public interface IBuildData { }