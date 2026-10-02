using System.Collections.Generic;
using Factory;

public class ProductionComponent : ConnectorBuildComponent
{
    protected readonly List<BeltItemBuffer> itemsInputs;
    protected readonly List<BeltItemBuffer> itemsOutputs;
    public Recipe Recipe { get; protected set; }
    
    public ProductionComponent(Connection[] outputConnections, Connection[] inputConnections) : base(outputConnections, inputConnections)
    {
        itemsInputs = new List<BeltItemBuffer>();
        itemsOutputs = new List<BeltItemBuffer>();
    }

    public void SetRecipe(Recipe recipe) => Recipe = recipe;
}
