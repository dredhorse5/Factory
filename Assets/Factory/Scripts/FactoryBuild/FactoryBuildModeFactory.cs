using Factory.GhostBuild;
using Factory.Input;
using VContainer;
using VContainer.Unity;

namespace Factory.Scripts.FactoryBuild
{
    public class FactoryBuildModeFactory : IBuildModeFactory
    {
        private readonly IObjectResolver resolver;

        public FactoryBuildModeFactory(IObjectResolver resolver)
        {
            this.resolver = resolver;
        }

        public IBuildMode Create(BaseBuildSO buildSO)
        {
            if (buildSO is BeltSO)
                return resolver.Resolve<BeltsBuildMode>();
            return resolver.Resolve<SingleBuildMode>();
        }
    }
}