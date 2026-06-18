using System;
using Factory;
using Factory.GhostBuild;
using Factory.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GhostBuildSystem : ITickable
{
    public event Action<BaseBuildSO> Selected;
    public event Action Canceled;
    public event Action PlaceDataChanged;
    
    
    public BaseBuildSO SelectedBuild => currentMode?.SelectedBuild;
    
    
    
    public bool IsSelected => currentMode != null;
    public bool CanBuild => currentMode.CanBuild;

    [Inject] BuildsDatabase buildsDatabase;
    [Inject] BuildSystem buildSystem;
    [Inject] IBuildModeFactory buildModeFactory;
    [Inject] IInputService input;
    
    private IBuildMode currentMode;
    private int lastBuildVersion;

    public void SelectBuild(string buildName)
    {
        if (buildsDatabase.GetBuild(buildName, out BaseBuildSO build))
            SelectBuild(build);
        else
            Debug.LogError($"Build {buildName} not found");
    }
    
    public void SelectBuild(BaseBuildSO buildSo)
    {
        currentMode = buildModeFactory.Create(buildSo);
        currentMode.Enter(buildSo);
        Selected?.Invoke(currentMode.SelectedBuild);
    }

    public void Build()
    {
        if(IsSelected && currentMode.CanBuild)
        {
            currentMode.Clear();
            var ghostPlacements = currentMode.GetGhostPlacements();
            for (int i = 0; i < ghostPlacements.Length; i++)
                buildSystem.CreateBuild(ghostPlacements[i], SelectedBuild);
        }
    }
    
    public BuildPlacement[] GetGhostPlacements() => currentMode.GetGhostPlacements();

    public void Cancel()
    {
        if(currentMode != null)
        {
            currentMode.Dispose();
            currentMode = null;
        }
        Canceled?.Invoke();
    }

    public void Tick()
    {
        if (IsSelected)
        {
            currentMode.Tick();
            if(lastBuildVersion != currentMode.Version)
                PlaceDataChanged?.Invoke();
            if (input.BuildPressed && !input.IsPointerOverUI)
                Build();
            if(input.CancelPressed)
                Cancel();
        }
    }
}