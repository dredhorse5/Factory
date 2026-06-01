using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class BuildsDatabase : MonoBehaviour
{
    [SerializeField] 
    private BaseBuildSO[] builds;
    
    
    private Dictionary<string, BaseBuildSO> buildsDictionary;
    public BaseBuildSO[] AllBuilds => builds;

    public static BuildsDatabase Instance { get; private set; }
    private void Awake()
    {
        buildsDictionary = new Dictionary<string, BaseBuildSO>();
        for (var i = 0; i < builds.Length; i++)
        {
            if (!buildsDictionary.TryAdd(builds[i].ID, builds[i]))
                Debug.LogError("build with id " + builds[i].ID + " already exists");
        }
        
        Instance = this;
    }

    public bool GetBuild(string id, out BaseBuildSO build) => buildsDictionary.TryGetValue(id, out build);
}