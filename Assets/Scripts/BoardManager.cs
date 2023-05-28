using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    private List<GameObject> tiles = new List<GameObject>();
    private BoardSpawner boardSpawner;

    void Start()
    {
        boardSpawner = GetComponent<BoardSpawner>();
        tiles = boardSpawner.SpawnBoard();
    }

    public int GetBoardSize()
    {
        return tiles.Count;
    }

    public Vector3 GetPositionOfTile(int index)
    {
        return tiles[index].transform.position;
    }

    public int GetIndexOfTile(GameObject tile)
    {
        if (tile == null || !tiles.Contains(tile))
            return -1;

        return tiles.IndexOf(tile);
    }

    public void SpawnCollectables(List<int> playerIndexes)
    {
        CollectableSpawner collectableSpawner = GetComponent<CollectableSpawner>();

        if (collectableSpawner != null)
            collectableSpawner.SpawnCollectables(tiles, playerIndexes);
    }

    public List<GameObject> GetAdjacentsByIndex(int index)
    {
        if (index < 0 || index >= tiles.Count)
            return new List<GameObject>();

        return GetAdjacents(tiles[index]);
    }

    public List<GameObject> GetDiagonalsByIndex(int index)
    {
        if (index < 0 || index >= tiles.Count)
            return new List<GameObject>();

        return GetDiagonals(tiles[index]);
    }

    private List<GameObject> GetAdjacents(GameObject tile)
    {
        if (tile == null || !tiles.Contains(tile))
            return new List<GameObject>();
        
        int index = tiles.IndexOf(tile);
        int x = index % boardSpawner.GetBaseSize();
        int y = index / boardSpawner.GetBaseSize();

        List<GameObject> adjs = new List<GameObject>();

        if (x > 0)
            adjs.Add(tiles[index - 1]);
        if (x < boardSpawner.GetBaseSize() - 1)
            adjs.Add(tiles[index + 1]);
        if (y > 0)
            adjs.Add(tiles[index - boardSpawner.GetBaseSize()]);
        if (y < boardSpawner.GetBaseSize() - 1)
            adjs.Add(tiles[index + boardSpawner.GetBaseSize()]);

        return adjs;
    }

    private List<GameObject> GetDiagonals(GameObject tile)
    {
        if (tile == null || !tiles.Contains(tile))
            return new List<GameObject>();

        int index = tiles.IndexOf(tile);
        int x = index % boardSpawner.GetBaseSize();
        int y = index / boardSpawner.GetBaseSize();

        List<GameObject> diags = new List<GameObject>();

        if (x > 0 && y > 0)
            diags.Add(tiles[index - boardSpawner.GetBaseSize() - 1]);
        if (x < boardSpawner.GetBaseSize() - 1 && y > 0)
            diags.Add(tiles[index - boardSpawner.GetBaseSize() + 1]);
        if (x > 0 && y < boardSpawner.GetBaseSize() - 1)
            diags.Add(tiles[index + boardSpawner.GetBaseSize() - 1]);
        if (x < boardSpawner.GetBaseSize() - 1 && y < boardSpawner.GetBaseSize() - 1)
            diags.Add(tiles[index + boardSpawner.GetBaseSize() + 1]);

        return diags;
    }
}
