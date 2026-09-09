namespace Factory
{
    public class BeltComponent : ConnectorBuildComponent
    {
        public BeltItemBuffer ItemBuffer;
        
        public BeltComponent(Connection[] outputConnections, Connection[] inputConnections, int itemsInTile) : base(outputConnections, inputConnections)
        {
            ItemBuffer = new BeltItemBuffer(itemsInTile);
        }

        public void SetOutputConnection(Cell cell, CellDirections direction)
        {
            OutputConnections[0] = new Connection(cell, direction, Connection.Types.Output, ItemBuffer);
        }

        public void SetInputConnection(Cell cell, CellDirections direction)
        {
            InputConnections[0] = new Connection(cell, direction, Connection.Types.Input, ItemBuffer);
        }
    }
}