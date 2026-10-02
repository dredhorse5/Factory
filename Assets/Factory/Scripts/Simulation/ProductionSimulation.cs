namespace Factory.Simulation
{
    public class ProductionSimulation : ISimulation
    {
        private readonly WorldProvider worldProvider;
        public ProductionSimulation(WorldProvider worldProvider)
        {
            this.worldProvider = worldProvider;
        }
        
        
        public void Tick(float dt)
        {
            var builds = worldProvider.world.Builds.Values;
            foreach (var build in builds)
            {
                if(build.TryGetComponent(out ProductionComponent prod))
                {
                    ProcessProduction(prod);
                }
            }
        }

        private void ProcessProduction(ProductionComponent comp)
        {
            switch (comp.State)
            {
                case ProductionComponent.ProductionStates.Idle:
                    for (int i = 0; i < comp.Recipe.InputItems.Count; i++)
                    {
                        for (var j = 0; j < comp.InputConnections.Length; j++)
                        {
                            //if(comp.InputConnections[j].Endpoint.CanExtract(comp.Recipe.InputItems[i]))
                        }
                    }
                    break;
            }
        }
    }
}