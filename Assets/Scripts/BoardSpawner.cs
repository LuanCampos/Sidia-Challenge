using UnityEngine;
using System.Collections.Generic;

public class BoardSpawner : MonoBehaviour
{
    [Header("Board Settings")]
    [Tooltip("The tile prefab to spawn.")]
    [SerializeField] private GameObject tilePrefab;
    [Tooltip("The position of the center of the board.")]
    [SerializeField] private Vector3 centralPos;
    [Tooltip("The size of the board.")]
    [SerializeField] private int baseSize = 8;
    [Tooltip("The spacing between tiles.")]
    [SerializeField] private float spacing = 1.1f;

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