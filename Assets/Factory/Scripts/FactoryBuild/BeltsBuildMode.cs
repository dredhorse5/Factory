using Factory.GhostBuild;
using Factory.Input;
using VContainer;
using VContainer.Unity;

namespace Factory.Scripts.FactoryBuild
{
    public class BeltsBuildMode : IBuildMode
    {

        public int Version { get; }
        public BaseBuildSO SelectedBuild => selectedBuild;
        private BaseBuildSO selectedBuild;
        public bool CanBuild { get; }
        
        
        private readonly IInputService input;
        private readonly CursorWorldPositionProvider cursor;
        private readonly BuildSystem buildSystem;
        
        
        
        [Inject]
        public BeltsBuildMode(IInputService input, CursorWorldPositionProvider cursor, BuildSystem buildSystem)
        {
            this.input = input;
            this.cursor = cursor;
            this.buildSystem = buildSystem;
        }
        
        public void Enter(BaseBuildSO buildSO)
        {
            selectedBuild = buildSO;
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
        
        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}