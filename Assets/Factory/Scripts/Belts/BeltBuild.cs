using System;
using UnityEngine.Serialization;

[Serializable]
public class BeltBuild : BaseBuild
{
    public uint SegmentID;
    public CellDirections InputDirection;
    public CellDirections OutputDirection;
    
    public BeltBuildData beltData => GetData<BeltBuildData>();

    public BeltBuild(uint id, string soId, IBuildData data) : base(id, soId, data) { }

    public override void SetData(IBuildData data)
    {
        if(data == null) this.data = new BeltBuildData();
        else if (data is BeltBuildData beltData) this.data = beltData;
        else this.data = new BeltBuildData();
    }

    public override void OnPlaced() => UpdateDirections();
    public override void OnReconfigured() => UpdateDirections();

    private void UpdateDirections()
    {
        InputDirection = Rotate(CellDirections.Down, (int)transform.Rotation);
        OutputDirection = Rotate(CellDirections.Down, (int)transform.Rotation + (int)beltData.Shape);
    }
    private CellDirections Rotate(CellDirections dir, int rotation)
    {
        return (CellDirections)(((int)dir + rotation) % 4);
    }
}

[Serializable]
public class BeltBuildData : IBuildData
{
    public BeltShapes Shape;
}