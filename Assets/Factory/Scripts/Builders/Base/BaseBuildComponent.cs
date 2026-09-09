
public class BaseBuildComponent : IBuildComponent
{
    private BaseBuild _parentBuild;

    BaseBuild IBuildComponent.parentBuild
    {
        get => _parentBuild;
        set => _parentBuild = value;
    }

    public void Initialize(BaseBuild build)
    {
        _parentBuild = build;
        OnInitialize();
    }

    protected virtual void OnInitialize()
    {
        
    }
}
