using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using VContainer;

namespace Factory
{
    public class RenderSystem : MonoBehaviour
    {
        public Mesh ItemMesh;
        public Material ItemMaterial;
        
        private Dictionary<uint, BaseBuildView> builds = new Dictionary<uint, BaseBuildView>();

        [Inject] Map map;
        [Inject] WorldProvider worldProvider;
        [Inject] BuildsDatabase buildsDatabase;
        [Inject] BuildSystem  buildSystem;
        
        public event Action<uint, BaseBuildView> OnCreated;
        public event Action<uint, BaseBuildView> OnWillDestroy;
        public event Action<uint> OnDestroyed;
        
        private void Start()
        {
            buildSystem.OnBuildCreated += CreateBuild;
            buildSystem.OnBuildWillDestroy += DestroyBuild;
        }
        private void OnDestroy()
        {
            buildSystem.OnBuildCreated -= CreateBuild;
            buildSystem.OnBuildWillDestroy -= DestroyBuild;
        }

        private void CreateBuild(uint obj, BaseBuild build)
        {
            if (buildsDatabase.GetBuild(build.SoId, out var buildSo))
            {
                var (pos, rot) = BuildTransformUtility.GetWorldTransform(build.transform, map);
                var view = Instantiate(buildSo.GetPrefabView(build.GetData()), pos, rot);
                view.transform.SetParent(transform);
                builds.Add(obj, view);
                OnCreated?.Invoke(obj, view);
            }
        }
        
        private void DestroyBuild(uint arg1, BaseBuild arg2)
        {
            if (builds.TryGetValue(arg1, out var view))
            {
                OnWillDestroy?.Invoke(arg1, view);
                Destroy(view.gameObject);
                builds.Remove(arg1);
                OnDestroyed?.Invoke(arg1);
            }
            else
                Debug.LogWarning($"Build {arg1} not found");
        }
    }
}