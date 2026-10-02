using System.Collections.Generic;
using Factory;

public class ProductionComponent : ConnectorBuildComponent
{
    protected readonly List<ItemsStorage> itemsInputs;
    protected readonly List<ItemsStorage> itemsOutputs;
    public Recipe Recipe { get; protected set; }

    public enum ProductionStates { Idle, Running, }
    public ProductionStates State;
    public float Progress;
    
    public ProductionComponent(Connection[] outputConnections, Connection[] inputConnections) : base(outputConnections, inputConnections)
    {
        itemsInputs = new List<ItemsStorage>();
        itemsOutputs = new List<ItemsStorage>();
    }

    public void SetRecipe(Recipe recipe) => Recipe = recipe;
}
