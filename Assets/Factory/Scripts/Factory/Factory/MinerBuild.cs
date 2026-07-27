using System;

namespace Factory.Factory.Factory
{
    public class MinerBuild : BaseBuild, IConnectable, IFactory
    {
        public CellDirections OutputDirection;
        public BeltItemBuffer ItemBuffer;
        
        public Connection[] OutputConnections { get; set; }
        public Connection[] InputConnections { get; set; }
        
        public MinerBuild(uint id, string soId, IBuildData data) : base(id, soId, data)
        {
            ItemBuffer = new BeltItemBuffer(20);
            OutputConnections = new Connection[1];
            InputConnections = Array.Empty<Connection>();
        }

        
        public void Update(float dt)
        {
            
        }
    }
}