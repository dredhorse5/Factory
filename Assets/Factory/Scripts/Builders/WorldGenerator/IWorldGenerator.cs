namespace Factory.WorldGenerator
{
    public interface IWorldGenerator
    {
        public uint[,] GenerateTerrain(int width, int height);
        public uint[,] GenerateBuilds(int width, int height);
    }
}