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
        return tiles.IndexOf(tile);
    }

    public List<GameObject> GetAdjacentsByIndex(int index)
    {
        return GetAdjacents(tiles[index]);
    }

    public List<GameObject> GetAdjacents(GameObject tile)
    {
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

    public void SpawnCollectables(List<int> playerIndexes)
    {
        boardSpawner.SpawnCollectables(tiles, playerIndexes);
    }
}
