using UnityEngine;

[CreateAssetMenu(fileName = "BaseBuildSO", menuName = "Builds/BaseBuildSO", order = 0)]
public abstract class BaseBuildSO : ScriptableObject
{
    public string ID;
    public Sprite Icon;
    public Vector2Int Size;
    public BaseBuildView Prefab;

    public virtual BaseBuild CreateBuild(uint id, BaseBuildData data)
    {
        return new BaseBuild(id, ID, data);
    }

    public virtual BaseBuildView GetPrefabView(BaseBuildData data) => Prefab;
}
