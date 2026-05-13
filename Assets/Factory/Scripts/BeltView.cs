using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Factory
{
    public class BeltView : MonoBehaviour
    {
        private Belt model;

        private Vector3[] itemPoints;
        
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
            CalculateItemPoints(belt, shape);
        }

        private void CalculateItemPoints(Belt belt, BeltShapes shape)
        {
            var pos = transform.position;
            var inDir = Utils.GetVectorDirection(belt.inputDirection);
            var outDir = Utils.GetVectorDirection(belt.outputDirection);
            float yPos = .3f;
            switch (shape)
            {
                case BeltShapes.Straight:
                    itemPoints = new Vector3[2];
                    itemPoints[0] = straightPoint(0);
                    itemPoints[1] = straightPoint(1);
                    Vector3 straightPoint(float t) => pos + new Vector3(inDir.x * t - inDir.x / 2f, yPos, inDir.y * t - inDir.y / 2f);
                    break;
                case BeltShapes.CornerRight:
                    itemPoints = new Vector3[4];
                    var centerv2 = (-inDir + outDir) / 2f;
                    for (int i = 0; i < itemPoints.Length; i++)
                    {
                        float t = (float)i / ((float)itemPoints.Length - 1f);
                        float angle = (t + (int)belt.inputDirection - 1) * Mathf.PI / 2;
                        
                        itemPoints[i] = cornerPoint(angle) + new Vector3(centerv2.x,0,centerv2.y);
                    }
                    
                    Vector3 cornerPoint(float angle) => pos + new Vector3(Mathf.Sin(angle)/2f, yPos, Mathf.Cos(angle)/2f);
                    break;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            for (var i = 0; i < itemPoints.Length; i++)
            {
                Gizmos.DrawSphere(itemPoints[i], .2f);
                Gizmos.color = Color.green;
            }
        }

        public Vector3 GetItemPosition(float t)
        {
            int count = itemPoints.Length;
            if (count == 0) return Vector3.zero;
            if (count == 1) return itemPoints[0];

            float scaled = t * (count - 1);

            int i = Mathf.Min((int)scaled, count - 2);

            float localT = scaled - i;
            return Vector3.Lerp(itemPoints[i], itemPoints[i + 1], localT);
        }
    }
}