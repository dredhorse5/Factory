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
        
        [Inject] BeltsSegmentSystem _beltsSegmentSystem;
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
            foreach (var beltsSegment in _beltsSegmentSystem.Segments)
            {
                for (var i = 0; i < beltsSegment.Data.items.Length; i++)
                {
                    var item = beltsSegment.Data.items[i];
                    if(item.Id == 0)
                        continue;
                    var cellProgress = (int)(item.Progress - 0.001f);
                    var beltID = beltsSegment.Belts[beltsSegment.Belts.Length - 1 - cellProgress].id;
                    var view = _beltViews[beltID];

                    RenderItem(item.Id, view.GetItemPosition(item.Progress - cellProgress));
                }
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