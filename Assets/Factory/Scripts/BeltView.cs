using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    public class BeltView : MonoBehaviour
    {
        public void SetBelt(Belt belt)
        {
            Build(belt);
        }

        private void Build(Belt belt)
        {
            var from = belt.topology.Start;
            var to = belt.topology.End;

            List<BeltTopology.Cell> cells;

            bool end = false;

            while (!end)
            {

                void CheckRotate()
                {
                    
                }

                void Step()
                {
                    
                }
            }
        }
    }
}