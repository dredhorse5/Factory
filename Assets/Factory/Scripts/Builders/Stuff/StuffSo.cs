using UnityEngine;

namespace Factory.Scripts.Builders.Stuff
{
    [CreateAssetMenu(fileName = "New Stuff", menuName = "Builds/Stuff", order = 0)]
    public class StuffSo : BaseBuildSO
    {
        public override BaseBuild CreateBuild(uint id)
        {
            return new Stuff(id, ID);
        }
    }
}