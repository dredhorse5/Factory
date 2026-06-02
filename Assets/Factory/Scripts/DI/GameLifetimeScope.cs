using Factory;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<World>(Lifetime.Singleton)
            .WithParameter("sizex", 256)
            .WithParameter("sizey", 256);

        builder.Register<BeltSystem>(Lifetime.Singleton);
        builder.Register<BuildSystem>(Lifetime.Singleton);
        builder.Register<GhostBuildSystem>(Lifetime.Singleton);
        builder.Register<TickSystem>(Lifetime.Singleton);
        
        builder.RegisterComponentInHierarchy<BuildsDatabase>();
        builder.RegisterComponentInHierarchy<RenderSystem>();
        builder.RegisterComponentInHierarchy<PrefabDatabase>();
        builder.RegisterComponentInHierarchy<GhostBuildView>();
        
        builder.RegisterComponentInHierarchy<BuildWindow>();
        
        builder.RegisterComponentInHierarchy<Map>();

        builder.RegisterEntryPoint<GameEntryPoint>();
    }
}
