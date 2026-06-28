using System;
using System.Collections.Generic;
using Factory;
using UnityEngine;
using VContainer;

public class GhostBuildView : MonoBehaviour
{
    public Material ghostMaterial;
    public Material ghostMaterialErrorPlace;

    [Inject] private GhostBuildSystem system;
    [Inject] private Map map;

    private List<BaseBuildView> _ghosts;
    private BaseBuildSO _currentBuild;
    private int _canPlaceMaterial = 0; // -1 - cant, 0 - not inited, 1 - can

    private void Start()
    {
        system.Selected += OnBuildSelected;
        system.Canceled += OnBuildCanceled;
        system.PlaceDataChanged += OnPlaceDataChanged;
    }

    private void OnDestroy()
    {
        if (system == null)
            return;

        system.Selected -= OnBuildSelected;
        system.Canceled -= OnBuildCanceled;
        system.PlaceDataChanged -= OnPlaceDataChanged;
    }

    private void OnBuildSelected(BaseBuildSO obj)
    {
        OnBuildCanceled();
        _currentBuild = obj;
        _ghosts = new List<BaseBuildView>();
        UpdateGhosts();
    }

    private void OnBuildCanceled()
    {
        _currentBuild = null;
        DeleteAllGhosts();
        _canPlaceMaterial = 0;
    }

    private void OnPlaceDataChanged()
    {
        UpdateGhosts();
    }

    private void UpdateGhosts()
    {
        if (_ghosts == null)
            return;
        
        var placements = system.GetGhostPlacements();
        if (placements == null || placements.Length == 0)
            return;
        
        UpdateGhostsCount(placements);
        UpdateGhostsPlacements(placements);
        UpdateGhostsMaterials();
    }

    private void UpdateGhostsCount(BuildPlacement[] placements)
    {
        if (_ghosts.Count != placements.Length)
        {
            if (_ghosts.Count > placements.Length)
            {
                for (var i = _ghosts.Count - 1; i >= placements.Length; i--)
                {
                    DeleteGhost(_ghosts[i]);
                    _ghosts.RemoveAt(i);
                }
            }
            else
            {
                for (int i = _ghosts.Count; i < placements.Length; i++)
                {
                    var ghost = CreateGhost(system.SelectedBuild);
                    _ghosts.Add(ghost);
                    UpdateMaterial(system.CanBuild, ghost);
                }
            }
        }
    }
    
    private void DeleteAllGhosts()
    {
        if(_ghosts == null)
            return;
        for (int i = 0; i < _ghosts.Count; i++)
            DeleteGhost(_ghosts[i]);
        _ghosts.Clear();
    }

    private BaseBuildView CreateGhost(BaseBuildSO build)
    {
        var view = Instantiate(build.Prefab, transform);
        view.renderers = view.GetComponentsInChildren<MeshRenderer>();
        return view;
    }

    private void DeleteGhost(BaseBuildView ghost)
    {
        ghost.renderers = null;
        Destroy(ghost.gameObject);
    }
    

    
    
    private void UpdateGhostsPlacements(BuildPlacement[] placements)
    {
        for (var i = 0; i < _ghosts.Count; i++)
        {
            var placement = placements[i];
            var (pos, rot) = BuildTransformUtility.GetWorldTransform(
                new BuildTransform(placement.Cell, placement.Rotation, _currentBuild.Size),
                map);
            _ghosts[i].transform.position = pos;
            _ghosts[i].transform.rotation = rot;
        }
    }

    
    
    private void UpdateGhostsMaterials()
    {
        if((_canPlaceMaterial == 1) == system.CanBuild)
            return;
        _canPlaceMaterial = system.CanBuild ? 1 : -1;
        for (var i = 0; i < _ghosts.Count; i++)
            UpdateMaterial(system.CanBuild, _ghosts[i]);
    }

    private void UpdateMaterial(bool canPlace, BaseBuildView ghost)
    {
        var mat = canPlace ? ghostMaterial : ghostMaterialErrorPlace;
        for (int i = 0; i < ghost.renderers.Length; i++)
            ghost.renderers[i].sharedMaterial = mat;
    }

}