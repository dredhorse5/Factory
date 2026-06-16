namespace Factory.GhostBuild
{
    public interface IBuildModeFactory
    {
        public IBuildMode Create(BaseBuildSO buildSO);
    }
}