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

    [Header("Buttons")] 
    [SerializeField] Button RotateRightBtn;
    [SerializeField] Button RotateLeftBtn;
    [SerializeField] Button BuildBtn;

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
        
        if(RotateRightBtn)
            RotateRightBtn.onClick.AddListener(RotateRight);
        if(RotateLeftBtn)
            RotateLeftBtn.onClick.AddListener(RotateLeft);
        if(BuildBtn)
            BuildBtn.onClick.AddListener(Build);
    }

    private void Update()
    {
        if(Window.General.CurrentState == StatesRead.Opened)
        {
            if(!ghostBuildSystem.SelectedBuild)
                return;
            ghostBuildSystem.SetGhostPosition(camera.GetLookAtCursorCell());
            if (inputService.BuildPressed && !inputService.IsPointerOverUI)
                Build();
            if (inputService.RotateBuildingPressed)
                RotateRight();
            if(inputService.CancelPressed)
                Close();
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
        ghostBuildSystem.SetGhostPosition(camera.GetLookAtCursorCell());
    }
}