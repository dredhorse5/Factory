using UnityEngine;

namespace Factory.WorldGenerator
{
    public class FlatWorldGenerator : IWorldGenerator
    {
        private const uint Water = 0;
        private const uint Sand = 1;
        private const uint Grass = 2;

        public uint[,] GenerateBuilds(int width, int height)
        {
            return new uint[width, height];
        }

        public uint[,] GenerateTerrain(int width, int height)
        {
            var terrain = new uint[width, height];

            float centerX = width * 0.5f;
            float centerY = height * 0.5f;

            float maxRadius = Mathf.Min(width, height) * 0.65f;

            float terrainScale = 0.05f;
            float lakeScale = 0.15f;

            float terrainSeedX = Random.Range(0f, 10000f);
            float terrainSeedY = Random.Range(0f, 10000f);

            float lakeSeedX = Random.Range(0f, 10000f);
            float lakeSeedY = Random.Range(0f, 10000f);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float noise = Mathf.PerlinNoise(
                        x * terrainScale + terrainSeedX,
                        y * terrainScale + terrainSeedY);

                    float dx = x - centerX;
                    float dy = y - centerY;

                    float distance = Mathf.Sqrt(dx * dx + dy * dy);
                    float normalizedDistance = distance / maxRadius;

                    // остров исчезает к краям карты
                    float islandMask = 1f - normalizedDistance;

                    float value = noise * islandMask;

                    if (value < 0.22f)
                    {
                        terrain[x, y] = Water;
                    }
                    else if (value < 0.30f)
                    {
                        terrain[x, y] = Sand;
                    }
                    else
                    {
                        terrain[x, y] = Grass;
                    }
                }
            }

            // Генерация внутренних озёр
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    if (terrain[x, y] != Grass)
                        continue;

                    float lakeNoise = Mathf.PerlinNoise(
                        x * lakeScale + lakeSeedX,
                        y * lakeScale + lakeSeedY);

                    bool farFromEdge =
                        x > width * 0.15f &&
                        x < width * 0.85f &&
                        y > height * 0.15f &&
                        y < height * 0.85f;

                    if (farFromEdge && lakeNoise > 0.82f)
                    {
                        terrain[x, y] = Water;
                    }
                }
            }

            // Песчаный берег вокруг озёр и моря
            AddBeach(terrain, width, height);

            return terrain;
        }

        private void AddBeach(uint[,] terrain, int width, int height)
        {
            var copy = (uint[,])terrain.Clone();

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    if (copy[x, y] != Grass)
                        continue;

                    bool nearWater =
                        copy[x - 1, y] == Water ||
                        copy[x + 1, y] == Water ||
                        copy[x, y - 1] == Water ||
                        copy[x, y + 1] == Water;

                    if (nearWater)
                    {
                        terrain[x, y] = Sand;
                    }
                }
            }
        }
    }
}