
public interface IBuildComponent
{
    public BaseBuild parentBuild { get; protected set; }
    
    public void Initialize(BaseBuild build);
}