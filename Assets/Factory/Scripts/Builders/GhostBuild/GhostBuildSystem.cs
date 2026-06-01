using System;
using Factory;
using UnityEngine;

public class GhostBuildSystem
{
    private readonly BuildSystem buildSystem;

    private BaseBuildSO selectedBuild;
    public BaseBuildSO SelectedBuild => selectedBuild;
    private BuildPlacement buildPlacement;
    public BuildPlacement BuildPlacement => buildPlacement;

    public event Action<BaseBuildSO> Selected;
    public event Action Canceled;
    public event Action<BuildPlacement> PlaceDataChanged;
    
    public bool IsSelected => selectedBuild != null;
    
    public GhostBuildSystem(BuildSystem buildSystem)
    {
        this.buildSystem = buildSystem;
    }

    public void SelectBuild(string buildName)
    {
        if (BuildsDatabase.Instance.GetBuild(buildName, out BaseBuildSO build))
            SelectBuild(build);
        else
            Debug.LogError($"Build {buildName} not found");
    }
    
    public void SelectBuild(BaseBuildSO buildSo)
    {
        selectedBuild = buildSo;
        buildPlacement = new BuildPlacement();
        Selected?.Invoke(selectedBuild);
    }
    
    public void SetGhostPosition(Vector2Int cell)
    {
        buildPlacement.Position = cell;
        PlaceDataChanged?.Invoke(buildPlacement);
    }

    public void SetGhostRotation(BuildRotations rotation)
    {
        buildPlacement.Rotation = rotation;
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
        Canceled?.Invoke();
    }
}