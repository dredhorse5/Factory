namespace Factory
{
    public class BuildSystem
    {
        private readonly World world;

        public BuildSystem(World world)
        {
            this.world = world;
        }
        public uint CreateBelt(int size, uint startBeltID, uint endBeltID)
        {
            if (startBeltID > 0 && world.Belts[startBeltID].nextBeltId != 0)
                return 0;
            if (endBeltID > 0 && world.Belts[endBeltID].previousBeltId != 0)
                return 0;

            var belt = new Belt()
            {
                id = world.NextBeltId++,
                nextBeltId = endBeltID > 0 ? world.Belts[endBeltID].id : 0,
                previousBeltId = startBeltID > 0 ? world.Belts[startBeltID].id : 0,
                items = new int[size],
                speed = 1
            };
            
            if(startBeltID > 0)
                world.Belts[startBeltID].nextBeltId = belt.id;
            if(endBeltID > 0)
                world.Belts[endBeltID].previousBeltId = belt.id;
            
            world.Belts.Add(belt.id, belt);
            return belt.id;
        }
    }
}