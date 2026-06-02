using System;
using DredPack.UIWindow;
using Factory;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class BuildWindow : WindowBehaviour
{
    [Header("Build Item")]
    [SerializeField]
    private GameObject itemContainer;
    [SerializeField]
    private BuildItem itemPrefab;

    [Header("Buttons")] 
    public Button RotateRightBtn;
    public Button RotateLeftBtn;
    public Button BuildBtn;

    private BuildItem[] items;
    [Inject]
    private GhostBuildSystem ghostBuildSystem;
    [Inject]
    private BuildsDatabase buildsDatabase;
    private void Start()
    {
        itemPrefab.gameObject.SetActive(false);
        var builds = buildsDatabase.AllBuilds;
        items = new BuildItem[builds.Length];
        for (var i = 0; i < builds.Length; i++)
            items[i] = CreateItem(builds[i]);
        
        RotateRightBtn.onClick.AddListener(RotateRight);
        RotateLeftBtn.onClick.AddListener(RotateLeft);
        BuildBtn.onClick.AddListener(Build);
    }

    private void Update()
    {
        if(Window.General.CurrentState == StatesRead.Opened)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                ghostBuildSystem.SetGhostPosition(new Vector2Int(ghostBuildSystem.BuildPlacement.Position.x, ghostBuildSystem.BuildPlacement.Position.y + 1));
            if (Input.GetKeyDown(KeyCode.DownArrow))
                ghostBuildSystem.SetGhostPosition(new Vector2Int(ghostBuildSystem.BuildPlacement.Position.x, ghostBuildSystem.BuildPlacement.Position.y - 1));
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                ghostBuildSystem.SetGhostPosition(new Vector2Int(ghostBuildSystem.BuildPlacement.Position.x - 1, ghostBuildSystem.BuildPlacement.Position.y));
            if (Input.GetKeyDown(KeyCode.RightArrow))
                ghostBuildSystem.SetGhostPosition(new Vector2Int(ghostBuildSystem.BuildPlacement.Position.x + 1, ghostBuildSystem.BuildPlacement.Position.y));
        }
    }
    
    private BuildItem CreateItem(BaseBuildSO buildSo)
    {
        var buildItem = Instantiate(itemPrefab, itemContainer.transform);
        buildItem.gameObject.SetActive(true);
        buildItem.Init(buildSo, this);
        return buildItem;
    }

    private void RotateRight() => ghostBuildSystem.RotateGhost();
    private void RotateLeft() => ghostBuildSystem.RotateGhost(false);
    private void Build() => ghostBuildSystem.BuildGhost();

    public override void OnStartClose()
    {
        ghostBuildSystem.CancelGhost();
    }

    public void Select(BaseBuildSO buildSo)
    {
        ghostBuildSystem.SelectBuild(buildSo);
    }
}