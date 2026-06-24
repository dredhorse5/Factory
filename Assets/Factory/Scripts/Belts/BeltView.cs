using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VContainer;

namespace Factory
{
    public class BeltView : MonoBehaviour
    {
        private BeltBuild model;

        private Vector3[] itemPoints;
        
        public BeltBuild Model => model;

        [Inject]
        private PrefabDatabase prefabDatabase;
        public void SetBelt(BeltBuild beltBuild)
        {
            model = beltBuild;
            var shape = BeltUtils.GetTurn(beltBuild);

            GameObject prefab = null;
            switch (shape)
            {
                case BeltShapes.Straight:
                    prefab = prefabDatabase.BeltModels.Forward;
                    break;
                case BeltShapes.CornerLeft:
                    prefab = prefabDatabase.BeltModels.LeftCorner;
                    break;
                case BeltShapes.CornerRight:
                    prefab = prefabDatabase.BeltModels.RightCorner;
                    break;
            }

            var mesh = Instantiate(prefab, transform.position, Quaternion.Euler(0,(int)beltBuild.InputDirection * 90f, 0), transform);
            CalculateItemPoints(beltBuild, shape);
        }

        private void CalculateItemPoints(BeltBuild beltBuild, BeltShapes shape)
        {
            var pos = transform.position;
            var inDir = BeltUtils.GetVectorDirection(beltBuild.InputDirection);
            var outDir = BeltUtils.GetVectorDirection(beltBuild.OutputDirection);
            float yPos = .3f;
            switch (shape)
            {
                case BeltShapes.Straight:
                    itemPoints = new Vector3[2];
                    itemPoints[0] = straightPoint(0);
                    itemPoints[1] = straightPoint(1);

                    Vector3 straightPoint(float t) => pos + new Vector3(inDir.x * t - inDir.x / 2f, yPos, inDir.y * t - inDir.y / 2f);

                    break;
                
                case BeltShapes.CornerRight: Corner(true); break;
                case BeltShapes.CornerLeft: Corner(false); break;
                    
                void Corner(bool isRight)
                {
                    itemPoints = new Vector3[4];
                    var centerv2 = (-inDir + outDir) / 2f;
                    float t = 0f;
                    float angle = 0f;
                    for (int i = 0; i < itemPoints.Length; i++)
                    {
                        if(isRight)
                        {
                            t = (float)i / ((float)itemPoints.Length - 1f);
                            angle = (t + (int)beltBuild.InputDirection - 1) * Mathf.PI / 2;
                        }
                        else
                        {
                            t = 1f - ((float)i / ((float)itemPoints.Length - 1f));
                            angle = (t + (int)beltBuild.InputDirection - 1) * Mathf.PI / 2 + Mathf.PI/2f;
                        }
                        
                        itemPoints[i] = cornerPointRight(angle) + new Vector3(centerv2.x, 0, centerv2.y);
                    }

                    Vector3 cornerPointRight(float angle) => pos + new Vector3(Mathf.Sin(angle) / 2f, yPos, Mathf.Cos(angle) / 2f);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if(itemPoints == null)
                return;
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