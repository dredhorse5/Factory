using System;
using Factory;

public class MinerBuild : BaseBuild, IProduction
{
    public Connection[] OutputConnections { get; set; }
    public Connection[] InputConnections { get; set; }

    public MinerBuild(uint id, string soId, IBuildData data) : base(id, soId, data)
    {
    }


    public void Update(float dt)
    {

    }
}
