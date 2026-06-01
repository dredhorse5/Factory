using UnityEngine;

[CreateAssetMenu(fileName = "BaseBuildSO", menuName = "Builds/BaseBuildSO", order = 0)]
public abstract class BaseBuildSO : ScriptableObject
{
    public string ID;
    public Sprite Icon;
    public BaseBuildView Prefab;
    public Vector2Int Size;

    public virtual BaseBuild CreateBuild(uint id)
    {
        return new BaseBuild(id, ID);
    }
}
