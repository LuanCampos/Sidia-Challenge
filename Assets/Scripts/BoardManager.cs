using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    private List<GameObject> tiles = new List<GameObject>();

    void Start()
    {
        tiles = GetComponent<BoardSpawner>().SpawnBoard();
    }

    void Update()
    {
        // TestTilesByClicking();
    }

    public int GetBoardSize()
    {
        return tiles.Count;
    }

    public Vector3 GetPositionOfTile(int index)
    {
        return tiles[index].transform.position;
    }

    public void TestTileByIndex(int index)
    {
        TestTile(tiles[index]);
    }

    private void TestTilesByClicking()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
                if (tiles.Contains(hit.collider.gameObject))
                    TestTile(hit.collider.gameObject);
        }
    }

    private void TestTile(GameObject tile)
    {
        tile.GetComponent<MeshRenderer>().material.color = Color.red;

        List<GameObject> adjs = GetAdjacents(tile);
        foreach (GameObject adj in adjs)
            adj.GetComponent<MeshRenderer>().material.color = Color.blue;

        List<GameObject> diags = GetDiagonals(tile);
        foreach (GameObject diag in diags)
            diag.GetComponent<MeshRenderer>().material.color = Color.green;
    }

    private List<GameObject> GetAdjacents(GameObject tile)
    {
        int index = tiles.IndexOf(tile);
        int x = index % GetComponent<BoardSpawner>().baseSize;
        int y = index / GetComponent<BoardSpawner>().baseSize;

        List<GameObject> adjs = new List<GameObject>();

        if (x > 0)
            adjs.Add(tiles[index - 1]);
        if (x < GetComponent<BoardSpawner>().baseSize - 1)
            adjs.Add(tiles[index + 1]);
        if (y > 0)
            adjs.Add(tiles[index - GetComponent<BoardSpawner>().baseSize]);
        if (y < GetComponent<BoardSpawner>().baseSize - 1)
            adjs.Add(tiles[index + GetComponent<BoardSpawner>().baseSize]);

        return adjs;
    }

    private List<GameObject> GetDiagonals(GameObject tile)
    {
        int index = tiles.IndexOf(tile);
        int x = index % GetComponent<BoardSpawner>().baseSize;
        int y = index / GetComponent<BoardSpawner>().baseSize;

        List<GameObject> diags = new List<GameObject>();

        if (x > 0 && y > 0)
            diags.Add(tiles[index - GetComponent<BoardSpawner>().baseSize - 1]);
        if (x < GetComponent<BoardSpawner>().baseSize - 1 && y > 0)
            diags.Add(tiles[index - GetComponent<BoardSpawner>().baseSize + 1]);
        if (x > 0 && y < GetComponent<BoardSpawner>().baseSize - 1)
            diags.Add(tiles[index + GetComponent<BoardSpawner>().baseSize - 1]);
        if (x < GetComponent<BoardSpawner>().baseSize - 1 && y < GetComponent<BoardSpawner>().baseSize - 1)
            diags.Add(tiles[index + GetComponent<BoardSpawner>().baseSize + 1]);

        return diags;
    }
}
