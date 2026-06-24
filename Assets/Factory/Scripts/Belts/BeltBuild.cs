using System;
using UnityEngine.Serialization;

[Serializable]
public class BeltBuild : BaseBuild
{
    public uint SegmentID;
    public BeltDirections InputDirection;
    public BeltDirections OutputDirection;

    public BeltBuild(uint id, string soId, BeltDirections inputDirection, BeltDirections outputDirection) : base(id, soId)
    {
        this.InputDirection = inputDirection;
        this.OutputDirection = outputDirection;
    }
}
public enum BeltDirections {NONE = -1, Up = 0, Right = 1, Down = 2, Left = 3 };