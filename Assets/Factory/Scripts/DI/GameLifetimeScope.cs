using Factory;
using Factory.GhostBuild;
using Factory.Input;
using Factory.Scripts.FactoryBuild;
using Factory.WorldGenerator;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IInputService, InputService>(Lifetime.Singleton);
        
        builder.Register<TickSystem>(Lifetime.Singleton);
        
        builder.Register<CursorWorldPositionProvider>(Lifetime.Singleton);
        builder.RegisterComponentInHierarchy<MainCamera>();
        
        ConfigureBuild(builder);
        
        builder.Register<BeltsSegmentSystem>(Lifetime.Singleton);
        builder.Register<BeltSystem>(Lifetime.Singleton);
        
        builder.RegisterComponentInHierarchy<RenderSystem>();
        builder.RegisterComponentInHierarchy<PrefabDatabase>();
        
        builder.Register<IWorldGenerator, FlatWorldGenerator>(Lifetime.Singleton);
        builder.Register<WorldFactory>(Lifetime.Singleton);
        builder.Register<WorldProvider>(Lifetime.Singleton);
        builder.RegisterComponentInHierarchy<Map>();
        

        builder.RegisterEntryPoint<GameEntryPoint>();
    }

    private void ConfigureBuild(IContainerBuilder builder)
    {
        builder.Register<SingleBuildMode>(Lifetime.Transient);
        builder.Register<BeltsBuildMode>(Lifetime.Transient);
        builder.Register<IBuildModeFactory, SingleBuildModeFactory>(Lifetime.Singleton);
        
        builder.RegisterComponentInHierarchy<BuildsDatabase>();
        builder.Register<BuildSystem>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        builder.Register<GhostBuildSystem>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        
        builder.RegisterComponentInHierarchy<GhostBuildView>();
        builder.RegisterComponentInHierarchy<BuildWindow>();
    }
}
