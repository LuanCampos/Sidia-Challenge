using UnityEngine;
using System.Collections.Generic;

public class CollectableSpawner : MonoBehaviour
{
    [Header("Collectables Settings")]
    [Tooltip("The collectables that can be spawned on the board.")]
    [SerializeField] private List<GameObject> collectablePrefabs = new List<GameObject>();
    [Tooltip("The chance of a collectable spawning on a tile.")]
    [SerializeField] [Range(0, 1)] private float collectableSpawnChance = 0.5f;

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
                position.y += 0.4f;
                GameObject collectable = Instantiate(collectablePrefabs[Random.Range(0, collectablePrefabs.Count)], position, Quaternion.identity);
                collectable.transform.SetParent(tiles[i].transform);
            }
        }
    }
}
