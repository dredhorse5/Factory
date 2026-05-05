using System;
using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    public class RenderSystem : MonoBehaviour
    {
        public Mesh ItemMesh;
        public Material ItemMaterial;
        
        private Dictionary<uint, BeltView> belts = new Dictionary<uint, BeltView>();

        private Map map;
        private void OnEnable()
        {
            BuildSystem.OnBeltCreated += CreateBelt;
        }

        private void OnDestroy()
        {
            BuildSystem.OnBeltCreated -= CreateBelt;
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

        private void CreateBelt(uint obj)
        {
            var prefab = PrefabDatabase.Instance.BeltView;
            var belt = GameManager.Instance.World.Belts[obj];
            var view = Instantiate(prefab, Map.Instance.GetPosition(belt.cell), Quaternion.identity, transform);
            view.SetBelt(belt);
            belts.Add(obj, view);
        }

        private void RenderBelt(BeltView belt)
        {
            var beltModel = belt.Model;
            var beltPos = map.GetPosition(beltModel.cell);
            var direction = Utils.GetVectorDirection(beltModel.inputDirection);
            
            for (var i = 0; i < beltModel.items.Length; i++)
            {
                if(beltModel.items[i] == 0)
                    continue;
                var t = ((i + beltModel.progress) / beltModel.items.Length);
                Vector2 itemPos = beltPos + new Vector3(direction.x - direction.x / 2f, 0, direction.y - direction.y / 2f) * t;
                RenderItem(beltModel.items[i], itemPos);
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