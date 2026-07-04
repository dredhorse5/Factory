using UnityEngine;

namespace Factory
{
    [CreateAssetMenu(fileName = "Belt", menuName = "Builds/Belt", order = 0)]
    public class BeltSO : BaseBuildSO
    {
        public BaseBuildView CornerLeftPrefab;
        public BaseBuildView CornerRightPrefab;
        
        public override BaseBuild CreateBuild(uint id, BaseBuildData data)
        {
            return new BeltBuild(id, ID, data);
        }

        public override BaseBuildView GetPrefabView(BaseBuildData data)
        {
            if (data is BeltBuildData beltData)
            {
                switch (beltData.Shape)
                {
                    case BeltShapes.CornerLeft: return CornerLeftPrefab;
                    case BeltShapes.CornerRight: return CornerRightPrefab;
                }
            }
            return Prefab;
        }
    }
}