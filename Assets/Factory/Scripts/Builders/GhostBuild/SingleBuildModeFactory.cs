using Factory.Input;
using VContainer;

namespace Factory.GhostBuild
{
    public class SingleBuildModeFactory : IBuildModeFactory
    {
        private readonly IObjectResolver resolver;

        public SingleBuildModeFactory(IObjectResolver resolver)
        {
            this.resolver = resolver;
        }

        public IBuildMode Create(BaseBuildSO buildSO)
        {
            var mode = resolver.Resolve<SingleBuildMode>();
            return mode;
        }
    }
}