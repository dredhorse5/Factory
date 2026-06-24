using UnityEngine;

public static class BuildTransformUtility
{
    public static (Vector3, Quaternion) GetWorldTransform(BuildTransform transform, Map map)
    {
        var size = transform.GetRotatedSize;
        var cellOffset = new Vector2Int(transform.Rotation is BuildRotations.R180 or BuildRotations.R270 ? size.x - 1 : 0,
            transform.Rotation is BuildRotations.R90 or BuildRotations.R180 ? size.y - 1 : 0);
        var pos = map.GetPosition(transform.Cell + cellOffset);
                
        var rot = Quaternion.Euler(0, (int)transform.Rotation * 90f, 0);
        return (pos, rot);
    }
}