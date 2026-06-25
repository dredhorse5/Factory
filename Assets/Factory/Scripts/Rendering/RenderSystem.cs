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
        
        private Dictionary<uint, BeltView> belts = new Dictionary<uint, BeltView>();
        private Dictionary<uint, BaseBuildView> builds = new Dictionary<uint, BaseBuildView>();

        [Inject] Map map;
        [Inject] WorldProvider worldProvider;
        [Inject] BuildsDatabase buildsDatabase;
        [Inject] BuildSystem  buildSystem;
        
        public event Action<uint, BaseBuildView> OnCreated;
        
        private void Start()
        {
            buildSystem.OnBuildCreated += CreateBuild;
        }

        private void OnDestroy()
        {
            buildSystem.OnBuildCreated -= CreateBuild;
        }

        private void Update()
        {
            foreach (var belt in belts.Values)
                RenderBelt(belt);
        }

        private void CreateBuild(uint obj, BaseBuild build)
        {
            if (buildsDatabase.GetBuild(build.SoId, out var buildSo))
            {
                var (pos, rot) = BuildTransformUtility.GetWorldTransform(build.transform, map);
                var view = Instantiate(buildSo.Prefab, pos, rot);
                view.transform.SetParent(transform);
                builds.Add(obj, view);
                OnCreated?.Invoke(obj, view);
            }
        }
        
        /*private void CreateBelt(uint obj)
        {
            var prefab = PrefabDatabase.Instance.BeltView;
            var belt = GameManager.Instance.World.Belts[obj];
            var view = Instantiate(prefab, Map.Instance.GetPosition(belt.cell), Quaternion.identity, transform);
            view.SetBelt(belt);
            belts.Add(obj, view);
        }*/

        private void RenderBelt(BeltView belt)
        {
            /*var beltModel = belt.Model;
            
            Vector3 itemPos = new Vector3();
            for (var i = 0; i < beltModel.items.Length; i++)
            {
                if(beltModel.items[i] == 0)
                    continue;
                
                var t = ((float)(i + beltModel.progress[i]) / (float)beltModel.items.Length);
                itemPos = belt.GetItemPosition(t);
                RenderItem(beltModel.items[i], itemPos);
            }

            if (beltModel.itemToTransfer != 0)
            {
                var t = ((float)(beltModel.items.Length + beltModel.progress[^1]) / (float)beltModel.items.Length);
                itemPos = belt.GetItemPosition(t);
                RenderItem(beltModel.itemToTransfer, itemPos);
            }*/
        }

        private void RenderItem(int item, Vector3 pos)
        {
            Graphics.DrawMesh(
                ItemMesh,
                Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one),
                ItemMaterial,
                0
            );
        }
    }
}