using Factory;
using Factory.WorldGenerator;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<BeltSystem>(Lifetime.Singleton);
        builder.Register<BuildSystem>(Lifetime.Singleton);
        builder.Register<GhostBuildSystem>(Lifetime.Singleton);
        builder.Register<TickSystem>(Lifetime.Singleton);
        
        builder.RegisterComponentInHierarchy<BuildsDatabase>();
        builder.RegisterComponentInHierarchy<RenderSystem>();
        builder.RegisterComponentInHierarchy<PrefabDatabase>();
        builder.RegisterComponentInHierarchy<GhostBuildView>();
        
        builder.RegisterComponentInHierarchy<BuildWindow>();
        
        builder.Register<IWorldGenerator, FlatWorldGenerator>(Lifetime.Singleton);
        builder.Register<WorldFactory>(Lifetime.Singleton);
        builder.Register<WorldProvider>(Lifetime.Singleton);
        builder.RegisterComponentInHierarchy<Map>();

        builder.RegisterEntryPoint<GameEntryPoint>();
    }
}
