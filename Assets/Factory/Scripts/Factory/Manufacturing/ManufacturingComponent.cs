using System.Collections.Generic;
using Factory;

public class ManufacturingComponent : ConnectorBuildComponent
{
    protected readonly List<BeltItemBuffer> itemsInputs;
    protected readonly List<BeltItemBuffer> itemsOutputs;
    public Receipt Receipt { get; protected set; }
    
    public ManufacturingComponent(Connection[] outputConnections, Connection[] inputConnections) : base(outputConnections, inputConnections)
    {
        itemsInputs = new List<BeltItemBuffer>();
        itemsOutputs = new List<BeltItemBuffer>();
    }

    public void SetReceipt(Receipt receipt) => Receipt = receipt;
}
