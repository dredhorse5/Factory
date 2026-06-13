using UnityEngine;

namespace Factory
{
    [CreateAssetMenu(fileName = "Belt", menuName = "Builds/Belt", order = 0)]
    public class BeltSO : BaseBuildSO
    {
        public override BaseBuild CreateBuild(uint id)
        {
            return new Belt(id, ID, BeltDirections.Down, BeltDirections.Down);
        }
    }
}