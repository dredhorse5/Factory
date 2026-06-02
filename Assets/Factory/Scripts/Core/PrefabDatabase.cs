using System;
using UnityEngine;

namespace Factory
{
    public class PrefabDatabase : MonoBehaviour
    {
        public BeltView BeltView;
        public BeltsModels BeltModels;

        [Serializable]
        public struct BeltsModels
        {
            public GameObject Forward;
            public GameObject LeftCorner;
            public GameObject RightCorner;
        }
    }
}