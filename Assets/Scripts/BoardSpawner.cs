using UnityEngine;
using System.Collections.Generic;

public class BoardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Vector3 centralPos;
    [SerializeField] private int baseSize = 8;
    [SerializeField] private float spacing = 1.1f;

    [SerializeField] private List<GameObject> collectablePrefabs = new List<GameObject>();
    [SerializeField] [Range(0, 1)] private float collectableSpawnChance = 0.5f;

    public int GetBaseSize()
    {
        return baseSize;
    }

    public List<GameObject> SpawnBoard()
    {
        List<GameObject> tiles = new List<GameObject>();
        Vector3 offset = centralPos - new Vector3(baseSize * spacing / 2f, 0, baseSize * spacing / 2f);

        for (int x = 0; x < baseSize; x++)
        {
            for (int y = 0; y < baseSize; y++)
            {
                GameObject tile = Instantiate(tilePrefab, offset + new Vector3(x * spacing, 0, y * spacing), Quaternion.identity);
                tile.transform.SetParent(transform);
                tiles.Add(tile);
            }
        }

        return tiles;
    }

    public void SpawnCollectables(List<GameObject> tiles, List<int> playerIndexes)
    {
        if (collectablePrefabs.Count == 0)
            return;

        for (int i = 0; i < tiles.Count; i++)
        {
            if (playerIndexes.Contains(i))
                continue;

            if (Random.Range(0f, 1f) < collectableSpawnChance)
            {
                Vector3 position = tiles[i].transform.position;
                position.y += 0.5f;
                GameObject collectable = Instantiate(collectablePrefabs[Random.Range(0, collectablePrefabs.Count)], position, Quaternion.identity);
                collectable.transform.SetParent(tiles[i].transform);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 offset = centralPos - new Vector3(spacing / 2f, 0, spacing / 2f);
        Gizmos.DrawWireCube(offset, new Vector3(baseSize * spacing, 0, baseSize * spacing));
    }

    private void OnValidate()
    {
        if (baseSize < 8)
            baseSize = 8;
    }
}