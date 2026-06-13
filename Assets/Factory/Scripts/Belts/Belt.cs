using System;

[Serializable]
public class Belt : BaseBuild
{
    public BeltDirections inputDirection;
    public BeltDirections outputDirection;

    public Belt(uint id, string soId, BeltDirections inputDirection, BeltDirections outputDirection) : base(id, soId)
    {
        this.inputDirection = inputDirection;
        this.outputDirection = outputDirection;
    }
}
public enum BeltDirections {NONE = -1, Up = 0, Right = 1, Down = 2, Left = 3 };