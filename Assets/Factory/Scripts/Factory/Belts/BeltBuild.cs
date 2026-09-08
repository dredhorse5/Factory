using System;
using System.Collections.Generic;
using Factory;
using UnityEngine.Serialization;

[Serializable]
public class BeltBuild : BaseBuild, IConnectable
{
    public CellDirections InputDirection;
    public CellDirections OutputDirection;
    public BeltItemBuffer ItemBuffer;
    
    public BeltBuildData beltData => GetData<BeltBuildData>();
    private ConnectorBuildComponent _connector;
    
    public Connection[] OutputConnections
    {
        get => _connector.OutputConnections;
        set => _connector.OutputConnections = value;
    }
    public Connection[] InputConnections 
    {
        get => _connector.InputConnections;
        set => _connector.InputConnections = value;
    }

    public BeltBuild(uint id, string soId, IBuildData data) : base(id, soId, data)
    {
        _connector = new ConnectorBuildComponent(new Connection[1], new Connection[1]);
        AddComponent(_connector);
        ItemBuffer = new BeltItemBuffer(BeltSettings.MaxItemsInTile);
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
        _connector.OutputConnections[0] = new Connection(transform.Cell, OutputDirection, Connection.Types.Output, ItemBuffer);
        _connector.InputConnections[0] = new Connection(transform.Cell, InputDirection, Connection.Types.Input, ItemBuffer);
        CellDirections Rotate(CellDirections dir, int rotation) => (CellDirections)(((int)dir + rotation) % 4);
    }

}

[Serializable]
public class BeltBuildData : IBuildData
{
    public BeltShapes Shape;
}