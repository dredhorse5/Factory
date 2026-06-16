using System;

namespace Factory.GhostBuild
{
    public interface IBuildMode : IDisposable
    {
        public int Version { get; }
        public BaseBuildSO SelectedBuild { get; }
        public bool CanBuild { get; }
        public void Enter(BaseBuildSO buildSO);
        public void Tick();
        
        public void Clear();
        public BuildPlacement[] GetGhostPlacements();


    }
}