using System;
using Factory;
using UnityEngine;
using VContainer;

public class GhostBuildSystem
{

    private BaseBuildSO selectedBuild;
    public BaseBuildSO SelectedBuild => selectedBuild;
    private BuildPlacement buildPlacement;
    public BuildPlacement BuildPlacement => buildPlacement;

    public event Action<BaseBuildSO> Selected;
    public event Action Canceled;
    public event Action<BuildPlacement> PlaceDataChanged;
    
    public bool IsSelected => selectedBuild != null;
    public bool canBuild => IsSelected && isPlaceFree;
    
    private bool isPlaceFree;

    [Inject]
    private BuildsDatabase buildsDatabase;
    [Inject]
    private MainCamera mainCamera;
    [Inject]
    private BuildSystem buildSystem;

    public void SelectBuild(string buildName)
    {
        if (buildsDatabase.GetBuild(buildName, out BaseBuildSO build))
            SelectBuild(build);
        else
            Debug.LogError($"Build {buildName} not found");
    }
    
    public void SelectBuild(BaseBuildSO buildSo)
    {
        selectedBuild = buildSo;
        Selected?.Invoke(selectedBuild);
    }
    
    public void SetGhostPosition(Vector2Int cell)
    {
        if(!selectedBuild)
            return;
        buildPlacement.Position = cell;
        isPlaceFree = buildSystem.CanPlaceBuild(buildPlacement, selectedBuild);
        PlaceDataChanged?.Invoke(buildPlacement);
    }

    public void SetGhostRotation(BuildRotations rotation)
    {
        if(!selectedBuild)
            return;
        buildPlacement.Rotation = rotation;
        isPlaceFree = buildSystem.CanPlaceBuild(buildPlacement, selectedBuild);
        PlaceDataChanged?.Invoke(buildPlacement);
    }

    public void RotateGhost(bool right = true)
    {
        int i = 0;
        if (right)
        {
            i = ((int)(buildPlacement.Rotation));
            i++;
        }
        else
        {
            i = ((int)(buildPlacement.Rotation));
            i--;
            if(i < 0) i = 3;
        }
        i %= 4;
        SetGhostRotation((BuildRotations)i);
    }

    public void BuildGhost()
    {
        buildSystem.CreateBuild(buildPlacement, selectedBuild);
    }

    public void CancelGhost()
    {
        selectedBuild = null;
        buildPlacement = new BuildPlacement();
        Canceled?.Invoke();
    }
}