using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    private int boardSize;
    private int currentPlayer = 0;
    private int currentPlayerMoves = 3;
    private BoardManager boardManager;

    public List<int> playersIndex = new List<int>();
    public List<GameObject> players = new List<GameObject>();    
    private List<GameObject> canMoveTo = new List<GameObject>();
    private List<GameObject> canAtackAt = new List<GameObject>();

    void Start()
    {
        boardManager = GameObject.Find("Board").GetComponent<BoardManager>();
        boardSize = boardManager.GetBoardSize();
        SpawnPlayers();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                if (canMoveTo.Contains(hit.collider.gameObject))
                    MovePlayerTo(hit.collider.gameObject);

                else if (canAtackAt.Contains(hit.collider.gameObject))
                    AtackPlayerAt(hit.collider.gameObject);
            }
        }
    }

    private void SpawnPlayers()
    {
        int randomIndex1 = Random.Range(0, boardSize);
        int randomIndex2 = Random.Range(0, boardSize);

        while (BadSpawnCombination(randomIndex1,randomIndex2))
            randomIndex2 = Random.Range(0, boardSize);

        SpawnPlayerAt(randomIndex1);
        SpawnPlayerAt(randomIndex2);

        UpdateMovePossibilities();
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
        playersIndex.Add(index);
    }

    private void UpdateMovePossibilities()
    {
        RemovePreviousColors();

        UpdateCanMoveTo();
        UpdateCanAtackAt();

        foreach (GameObject tile in canAtackAt)
            canMoveTo.Remove(tile);

        AddCurrentColors();
    }

    private void RemovePreviousColors()
    {
        foreach (GameObject tile in canMoveTo)
            tile.GetComponent<MeshRenderer>().material.color = Color.white;

        foreach (GameObject tile in canAtackAt)
            tile.GetComponent<MeshRenderer>().material.color = Color.white;
    }

    private void UpdateCanMoveTo()
    {
        canMoveTo = boardManager.GetAdjacentsByIndex(playersIndex[currentPlayer]);
    }

    private void UpdateCanAtackAt()
    {
        canAtackAt.Clear();

        foreach (GameObject tile in canMoveTo)
        {
            foreach (int index in playersIndex)
            {
                if (boardManager.GetIndexOfTile(tile) == index)
                    canAtackAt.Add(tile);
            }
        }
    }

    private void AddCurrentColors()
    {
        foreach (GameObject tile in canMoveTo)
            tile.GetComponent<MeshRenderer>().material.color = Color.green;

        foreach (GameObject tile in canAtackAt)
            tile.GetComponent<MeshRenderer>().material.color = Color.red;
    } 

    private void PlayerMadeMove()
    {
        currentPlayerMoves--;

        Debug.Log("Player " + currentPlayer + " made a move");
        Debug.Log("Player " + currentPlayer + " has " + currentPlayerMoves + " moves left");

        if (currentPlayerMoves <= 0)
            NextPlayer();

        UpdateMovePossibilities();
    }

    private void NextPlayer()
    {
        currentPlayer++;

        if (currentPlayer >= players.Count)
            currentPlayer = 0;

        currentPlayerMoves = 3;
    }

    private void MovePlayerTo(GameObject tile)
    {
        players[currentPlayer].transform.position = tile.transform.position;
        playersIndex[currentPlayer] = boardManager.GetIndexOfTile(tile);
        PlayerMadeMove();
    }

    private void AtackPlayerAt(GameObject tile)
    {
        int index = boardManager.GetIndexOfTile(tile);
        int playerIndex = playersIndex.IndexOf(index);

        Destroy(players[playerIndex]);

        players.RemoveAt(playerIndex);
        playersIndex.RemoveAt(playerIndex);

        if (playerIndex < currentPlayer)
            currentPlayer--;

        PlayerMadeMove();
    }
}
