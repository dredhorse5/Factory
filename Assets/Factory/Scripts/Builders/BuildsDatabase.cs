using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildsDatabase : MonoBehaviour
{
    [SerializeField] 
    private BaseBuildSO[] Builds;
    
    
    private Dictionary<string, BaseBuildSO> builds;

    public static BuildsDatabase Instance { get; private set; }
    private void Awake()
    {
        
        builds = new Dictionary<string, BaseBuildSO>();
        for (var i = 0; i < Builds.Length; i++)
        {
            if (!builds.TryAdd(Builds[i].ID, Builds[i]))
                Debug.LogError("build with id " + Builds[i].ID + " already exists");
        }
        
        Instance = this;
    }

    public bool GetBuild(string id, out BaseBuildSO build) => builds.TryGetValue(id, out build);
}