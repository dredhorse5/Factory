using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

namespace Factory
{
    public class RenderSystem : MonoBehaviour
    {
        public Mesh ItemMesh;
        public Material ItemMaterial;
        
        private Dictionary<uint, BeltView> belts = new Dictionary<uint, BeltView>();
        private Dictionary<uint, BaseBuildView> builds = new Dictionary<uint, BaseBuildView>();

        private Map map;
        private void OnEnable()
        {
            BuildSystem.OnBuildCreated += CreateBuild;
        }

        private void OnDestroy()
        {
            BuildSystem.OnBuildCreated -= CreateBuild;
        }


        private void Start()
        {
            map = Map.Instance;
        }

        private void Update()
        {
            foreach (var belt in belts.Values)
                RenderBelt(belt);
        }

        private void CreateBuild(uint obj)
        {
            GameManager.Instance.World.GetBuild(obj, out var build);
            if (BuildsDatabase.Instance.GetBuild(build.SoId, out var buildSo))
            {
                var size = build.transform.GetRotatedSize;
                var cellOffset = new Vector2Int(build.transform.Rotation is BuildRotations.R180 or BuildRotations.R270 ? size.x - 1 : 0,
                    build.transform.Rotation is BuildRotations.R90 or BuildRotations.R180 ? size.y - 1 : 0);
                var pos = map.GetPosition(build.transform.Position + cellOffset);
                
                var rot = Quaternion.Euler(0, (int)build.transform.Rotation * 90f, 0);
                
                var view = Instantiate(buildSo.Prefab, pos, rot);
                builds.Add(obj, view);
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
            var beltModel = belt.Model;
            
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
            }
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