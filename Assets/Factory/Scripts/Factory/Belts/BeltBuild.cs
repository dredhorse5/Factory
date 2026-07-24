using System;
using System.Collections.Generic;
using Factory;
using UnityEngine.Serialization;

[Serializable]
public class BeltBuild : BaseBuild, IConnectable
{
    public uint SegmentID;
    public CellDirections InputDirection;
    public CellDirections OutputDirection;
    public BeltItemBuffer ItemBuffer;
    public Connection OutputConnection => OutputConnections[0];
    public Connection InputConnection => InputConnections[0];
    
    public Connection[] OutputConnections { get; set; }
    public Connection[] InputConnections { get; set; }
    
    public BeltBuildData beltData => GetData<BeltBuildData>();

    public BeltBuild(uint id, string soId, IBuildData data) : base(id, soId, data)
    {
        ItemBuffer = new BeltItemBuffer(BeltSettings.MaxItemsInTile);
        OutputConnections = new Connection[1];
        InputConnections = new Connection[1];
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
        InputDirection = Rotate(CellDirections.Down, (int)transform.Rotation);
        OutputDirection = Rotate(CellDirections.Down, (int)transform.Rotation + (int)beltData.Shape);
        OutputConnections[0] = new Connection(transform.Cell, OutputDirection, Connection.Types.Output, ItemBuffer);
        InputConnections[0] = new Connection(transform.Cell, InputDirection, Connection.Types.Input, ItemBuffer);
        CellDirections Rotate(CellDirections dir, int rotation) => (CellDirections)(((int)dir + rotation) % 4);
    }

}

[Serializable]
public class BeltBuildData : IBuildData
{
    public BeltShapes Shape;
}