namespace Factory
{
    public class Connection
    {
        public readonly Cell Cell;
        public readonly CellDirections Direction;
        public readonly Types Type;
        public readonly IItemEndpoint Endpoint;
        public enum Types { Input, Output }
        
        public Connection Connected;

        public Connection(Cell cell, CellDirections direction, Types type, IItemEndpoint endpoint) { Cell = cell; Direction = direction; Type = type; Endpoint = endpoint; }
    }
}