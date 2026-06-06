using System;
using Factory;
using UnityEngine;
using UnityEngine.Rendering;
using VContainer;

public class GhostBuildView : MonoBehaviour
{
    public Material ghostMaterial;
    public Material ghostMaterialErrorPlace;
    [Inject]
    private GhostBuildSystem system;

    private BaseBuildView _ghost;
    private MeshRenderer[] _meshRenderers;
    private bool _canPlace;

    [Inject]
    private Map map;
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
        UpdateMaterial(system.canBuild);
    }
    
    private void CreateGhost(BaseBuildSO obj)
    {
        if(_ghost != null)
            DeleteGhost();
        _ghost = Instantiate(obj.Prefab);
        UpdateGhostTransform();
        _meshRenderers = _ghost.GetComponentsInChildren<MeshRenderer>();
        UpdateMaterial(system.canBuild, true);
    }

    private void UpdateMaterial(bool canPlace, bool updateImmediate = false)
    {
        if(_meshRenderers == null || _meshRenderers.Length == 0)
            return;
        if (updateImmediate || canPlace != _canPlace)
        {
            _canPlace = canPlace;
            for (var i = 0; i < _meshRenderers.Length; i++)
                _meshRenderers[i].sharedMaterial = _canPlace? ghostMaterial : ghostMaterialErrorPlace;
        }
    }

    private void DeleteGhost()
    {
        if (_ghost == null)
            return;

        Destroy(_ghost.gameObject);
        _ghost = null;
        _meshRenderers = null;
    }
}
