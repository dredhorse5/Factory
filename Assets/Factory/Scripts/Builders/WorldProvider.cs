namespace Factory
{
    public class WorldProvider
    {
        public World world { get; private set; }
        
        public void SetWorld(World world)
        {
            this.world = world;
        }
    }
}