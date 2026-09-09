using System;
using System.Collections.Generic;
using Factory;
using UnityEngine.Serialization;

[Serializable]
public class BeltBuild : BaseBuild
{
    public BeltBuildData beltData => GetData<BeltBuildData>();
    
    public BeltComponent belt { get; private set; }

    public BeltBuild(uint id, string soId, IBuildData data) : base(id, soId, data)
    {
        belt = new BeltComponent(new Connection[1], new Connection[1], BeltSettings.MaxItemsInTile);
        AddComponent(belt);
    }

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
        var input = Rotate(CellDirections.Down, (int)transform.Rotation);
        var output = Rotate(CellDirections.Down, (int)transform.Rotation + (int)beltData.Shape);
        belt.SetOutputConnection(transform.Cell, output);
        belt.SetInputConnection(transform.Cell, input);
        CellDirections Rotate(CellDirections dir, int rotation) => (CellDirections)(((int)dir + rotation) % 4);
    }

}

[Serializable]
public class BeltBuildData : IBuildData
{
    public BeltShapes Shape;
}