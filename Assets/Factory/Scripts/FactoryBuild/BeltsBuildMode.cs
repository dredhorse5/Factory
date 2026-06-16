using Factory.GhostBuild;
using Factory.Input;
using VContainer;
using VContainer.Unity;

namespace Factory.Scripts.FactoryBuild
{
    public class BeltsBuildMode : IBuildMode
    {
        public void Dispose()
        {
            // TODO release managed resources here
        }

        public int Version { get; }
        public BaseBuildSO SelectedBuild { get; }
        public bool CanBuild { get; }
        public void Enter(BaseBuildSO buildSO)
        {
            throw new System.NotImplementedException();
        }

        public void Tick()
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public BuildPlacement[] GetGhostPlacements()
        {
            throw new System.NotImplementedException();
        }
    }
}