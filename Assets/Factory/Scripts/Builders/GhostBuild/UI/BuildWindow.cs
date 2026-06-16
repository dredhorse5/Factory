using System;
using DredPack.UIWindow;
using Factory;
using Factory.Input;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class BuildWindow : WindowBehaviour
{
    [Header("Build Item")]
    [SerializeField] GameObject itemContainer;
    [SerializeField] BuildItem itemPrefab;

    private BuildItem[] items;
    
    [Inject] GhostBuildSystem ghostBuildSystem;
    [Inject] BuildsDatabase buildsDatabase;
    [Inject] MainCamera camera;
    [Inject] IInputService inputService;
    private void Start()
    {
        itemPrefab.gameObject.SetActive(false);
        var builds = buildsDatabase.AllBuilds;
        items = new BuildItem[builds.Length];
        for (var i = 0; i < builds.Length; i++)
            items[i] = CreateItem(builds[i]);
    }

    private BuildItem CreateItem(BaseBuildSO buildSo)
    {
        var buildItem = Instantiate(itemPrefab, itemContainer.transform);
        buildItem.gameObject.SetActive(true);
        buildItem.Init(buildSo, this);
        return buildItem;
    }

    public void Select(BaseBuildSO buildSo)
    {
        ghostBuildSystem.SelectBuild(buildSo);
    }
}