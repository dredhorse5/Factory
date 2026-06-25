using UnityEngine;

public class BaseBuildView : MonoBehaviour
{
    public MeshRenderer[] renderers;

    protected uint id;

    public uint Id => id;
    public virtual void Construct(uint id, BaseBuild build)
    {
        this.id = id;
    }
}