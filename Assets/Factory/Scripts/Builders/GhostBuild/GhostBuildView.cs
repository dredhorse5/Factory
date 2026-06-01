using System;
using Factory;
using UnityEngine;
using UnityEngine.Rendering;

public class GhostBuildView : MonoBehaviour
{
    public Material ghostMaterial;
    private GhostBuildSystem system;

    private BaseBuildView _ghost;

    private Map map;
    private void Start()
    {
        system = GameManager.Instance.GhostBuildSystem;
        system.Selected += OnBuildSelected;
        system.Canceled += OnBuildCanceled;
        system.PlaceDataChanged += OnPlaceDataChanged;
        
        map = Map.Instance;
    }

    private void OnDestroy()
    {
        if (system == null)
            return;

        system.Selected -= OnBuildSelected;
        system.Canceled -= OnBuildCanceled;
        system.PlaceDataChanged -= OnPlaceDataChanged;
    }


    private void OnPlaceDataChanged(BuildPlacement obj)
    {
        UpdateGhostTransform();
    }

    private void OnBuildCanceled()
    {
        DeleteGhost();
    }

    private void OnBuildSelected(BaseBuildSO obj)
    {
        CreateGhost(obj);
    }

    private void UpdateGhostTransform()
    {
        if (_ghost == null)
            return;
        var placement = system.BuildPlacement;
        var buildSo = system.SelectedBuild;
        
        var (pos, rot) = BuildTransformUtility.GetWorldTransform(new BuildTransform(placement.Position,placement.Rotation, buildSo.Size), map);
        _ghost.transform.position = pos;
        _ghost.transform.rotation = rot;
    }
    
    private void CreateGhost(BaseBuildSO obj)
    {
        if(_ghost != null)
            DeleteGhost();
        _ghost = Instantiate(obj.Prefab);
        UpdateGhostTransform();
        var meshRenderers = _ghost.GetComponentsInChildren<MeshRenderer>();
        for (var i = 0; i < meshRenderers.Length; i++)
            meshRenderers[i].sharedMaterial = ghostMaterial;
    }

    private void DeleteGhost()
    {
        if (_ghost == null)
            return;

        Destroy(_ghost.gameObject);
        _ghost = null;
    }
}
