using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private BoardManager boardManager;
    private int boardSize;
    private List<GameObject> players = new List<GameObject>();

    void Start()
    {
        boardManager = GameObject.Find("Board").GetComponent<BoardManager>();
        boardSize = boardManager.GetBoardSize();

        int randomIndex1 = Random.Range(0, boardSize);
        int randomIndex2 = Random.Range(0, boardSize);

        while (BadSpawnCombination(randomIndex1,randomIndex2))
            randomIndex2 = Random.Range(0, boardSize);

        SpawnPlayerAt(randomIndex1);
        SpawnPlayerAt(randomIndex2);
    }

    void Update()
    {
        
    }

    private bool BadSpawnCombination(int index1, int index2)
    {
        bool sameTile = index1 == index2;
        bool adjacentTiles = index1 == index2 + 1 || index1 == index2 - 1 || index1 == index2 + boardSize || index1 == index2 - boardSize;
        bool diagonalTiles = index1 == index2 + boardSize + 1 || index1 == index2 + boardSize - 1 || index1 == index2 - boardSize + 1 || index1 == index2 - boardSize - 1;

        return sameTile || adjacentTiles || diagonalTiles;
    }

    private void SpawnPlayerAt(int index)
    {
        Vector3 position = boardManager.GetPositionOfTile(index);
        position.y += 1f;
        GameObject player = Instantiate(playerPrefab, position, Quaternion.identity);
        players.Add(player);
        boardManager.TestTileByIndex(index);
    }    
}
