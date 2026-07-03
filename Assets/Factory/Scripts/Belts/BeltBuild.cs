using System;
using UnityEngine.Serialization;

[Serializable]
public class BeltBuild : BaseBuild
{
    public uint SegmentID;
    public BeltDirections InputDirection;
    public BeltDirections OutputDirection;

    public BeltBuild(uint id, string soId, BaseBuildData data) : base(id, soId, data) { }
    public override void OnPlaced()
    {
        InputDirection = Rotate(BeltDirections.Down, (int)transform.Rotation);
        OutputDirection = Rotate(BeltDirections.Down, (int)transform.Rotation);
    }
    private BeltDirections Rotate(BeltDirections dir, int rotation)
    {
        return (BeltDirections)(((int)dir + rotation) % 4);
    }
}
public enum BeltDirections {NONE = -1, Up = 0, Right = 1, Down = 2, Left = 3 };

[Serializable]
public class BeltBuildData : BaseBuildData
{
    
}