using UnityEngine;
using System.Collections.Generic;

public class BoardSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public Vector3 centralPos;
    public int baseSize = 8;
    public float spacing = 1.1f;
	
	private List<GameObject> tiles = new List<GameObject>();
	
	void Start()
    {
        tiles = SpawnBoard();
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
        if (baseSize < 1)
            baseSize = 2;
    }
}