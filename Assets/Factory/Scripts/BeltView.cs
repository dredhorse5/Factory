using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Factory
{
    public class BeltView : MonoBehaviour
    {
        private Belt model;
        
        public Belt Model => model;
        public void SetBelt(Belt belt)
        {
            model = belt;
            var shape = Utils.GetTurn(belt);

            GameObject prefab = null;
            switch (shape)
            {
                case BeltShapes.Straight:
                    prefab = PrefabDatabase.Instance.BeltModels.Forward;
                    break;
                case BeltShapes.CornerLeft:
                    prefab = PrefabDatabase.Instance.BeltModels.LeftCorner;
                    break;
                case BeltShapes.CornerRight:
                    prefab = PrefabDatabase.Instance.BeltModels.RightCorner;
                    break;
            }

            var mesh = Instantiate(prefab, transform.position, Quaternion.Euler(0,(int)belt.inputDirection * 90f, 0), transform);
        }
    }
}