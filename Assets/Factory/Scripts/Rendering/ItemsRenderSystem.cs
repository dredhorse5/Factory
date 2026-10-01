using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Factory
{
    public class ItemsRenderSystem : MonoBehaviour
    {
        public Mesh ItemMesh;
        public Material ItemMaterial;
        
        [Inject] BeltSystem _beltsSystem;
        [Inject] RenderSystem _renderSystem;
        
        private Dictionary<uint, BeltView> _beltViews = new Dictionary<uint, BeltView>();

        private void Start()
        {
            _renderSystem.OnCreated += TryAddBeltView;
            _renderSystem.OnWillDestroy += TryRemoveBeltView;
        }
        
        private void OnDestroy()
        {
            _renderSystem.OnCreated -= TryAddBeltView;
            _renderSystem.OnWillDestroy -= TryRemoveBeltView;
        }

        private void TryAddBeltView(uint arg1, BaseBuildView arg2)
        {
            if(arg2 is BeltView beltView)
                _beltViews.Add(arg1, beltView);
        }
        
        private void TryRemoveBeltView(uint arg1, BaseBuildView arg2)
        {
            if(_beltViews.ContainsKey(arg1))
                _beltViews.Remove(arg1);
        }

        private void Update()
        {
            foreach (var belt in _beltsSystem.Belts)
            {
                var view = _beltViews[belt.id];
                for (var i = 0; i < belt.belt.ItemBuffer.items.Count; i++)
                {
                    var item = belt.belt.ItemBuffer.items[i];
                    RenderItem(item.Item, view.GetItemPosition(item.Progress));
                }
            }
        }
        
        
        private void RenderItem(Item item, Vector3 pos)
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