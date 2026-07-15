using System.Collections.Generic;

namespace Factory
{
    public class FactoryBuild : BaseBuild
    {
        public Connection[] OutputConnections { get; protected set; }
        public Connection[] InputConnections { get; protected set; }
        public FactoryBuild(uint id, string soId, IBuildData data) : base(id, soId, data) { }
    }
}