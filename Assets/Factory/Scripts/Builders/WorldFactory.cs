using Factory.WorldGenerator;

namespace Factory
{
    public class WorldFactory
    {
        private readonly IWorldGenerator _generator;

        public WorldFactory(IWorldGenerator generator)
        {
            _generator = generator;
        }

        public World Create(int width, int height)
        {
            var world = new World(width, height);

            world.terrain = _generator.GenerateTerrain(width, height);
            world.tiles = _generator.GenerateBuilds(width, height);

            return world;
        }
    }
}