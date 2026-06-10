using System;
using Factory;
using UnityEngine;
using VContainer;

public class Map : MonoBehaviour
{
    public float CellSize = 1f;

    private PrefabDatabase prefabDatabase;
    private WorldProvider worldProvider;
    
    public Vector3 GetPosition(Vector2Int cell)
    {
        return new Vector3(cell.x * CellSize, 0, cell.y * CellSize);
    }

    public Vector2Int GetCellByPosition(Vector3 position)
    {
        return new Vector2Int(Mathf.RoundToInt(position.x / CellSize), Mathf.RoundToInt(position.z / CellSize));
    }


    [Inject]
    public void Construct(WorldProvider worldProvider, PrefabDatabase prefabDatabase)
    {
        this.prefabDatabase = prefabDatabase;
        this.worldProvider = worldProvider;
    }

    public void GenerateMap()
    {
        var prefabs = prefabDatabase.TerrainTilesPrefab;
        for (int x = 0; x < worldProvider.world.terrain.GetLength(0); x++)
        {
            for (int y = 0; y < worldProvider.world.terrain.GetLength(1); y++)
            {
                var id = worldProvider.world.terrain[x,y];
                var prefab = prefabs[id];
                Instantiate(prefab, GetPosition(new Vector2Int(x, y)), prefab.transform.rotation, transform);
            }
        }
    }

}