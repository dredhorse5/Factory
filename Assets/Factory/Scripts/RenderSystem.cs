using System;
using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    public class RenderSystem : MonoBehaviour
    {
        private Dictionary<uint, BeltView> belts = new Dictionary<uint, BeltView>();
        private void OnEnable()
        {
            BuildSystem.OnBeltCreated += CreateBelt;
        }

        private void OnDestroy()
        {
            BuildSystem.OnBeltCreated -= CreateBelt;
        }

        private void CreateBelt(uint obj)
        {
            var prefab = PrefabDatabase.Instance.BeltView;
            var belt = GameManager.Instance.World.Belts[obj];
            var view = Instantiate(prefab, Map.Instance.GetPosition(belt.cell), Quaternion.identity, transform);
            view.SetBelt(belt);
            belts.Add(obj, view);
        }
    }
}