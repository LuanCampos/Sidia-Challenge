using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Tooltip("The prefab of the player")]
    [SerializeField] private GameObject playerPrefab;

    private int boardSize;
    private int currentPlayer = 0;
    private int currentPlayerMoves = 3;
    private BoardManager boardManager;

    private List<int> playersIndex = new List<int>();
    private List<GameObject> players = new List<GameObject>();    
    private List<GameObject> canMoveTo = new List<GameObject>();
    private List<GameObject> canAttackAt = new List<GameObject>();

    private TMPro.TextMeshProUGUI logText;

    void Start()
    {
        GetBoard();
        GetLogText();
        SpawnPlayers();
        SpawnCollectables();
        LogPlayersInfo();
    }

    void Update()
    {
        HandleInput();
    }

    public void AddMovesToCurrentPlayer(int moves)
    {
        currentPlayerMoves += moves;
    }

    public Vector3 GetPositionOfCurrentPlayer()
    {
        return players[currentPlayer].transform.position;
    }

    private void GetBoard()
    {
        boardManager = FindObjectOfType<BoardManager>();
        boardSize = boardManager.GetBoardSize();
    }

    private void GetLogText()
    {
        logText = GameObject.Find("LogText").GetComponent<TextMeshProUGUI>();
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
        
        if (players.Count == 1)
            player.GetComponent<MeshRenderer>().material.color = Color.blue;
        else
            player.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    private void UpdateMovePossibilities()
    {
        RemovePreviousColors();

        UpdateCanMoveTo();
        UpdateCanAttackAt();

        foreach (GameObject tile in canAttackAt)
            canMoveTo.Remove(tile);

        AddCurrentColors();
    }

    private void RemovePreviousColors()
    {
        foreach (GameObject tile in canMoveTo)
            tile.GetComponent<MeshRenderer>().material.color = Color.white;

        foreach (GameObject tile in canAttackAt)
            tile.GetComponent<MeshRenderer>().material.color = Color.white;
    }

    private void UpdateCanMoveTo()
    {
        canMoveTo = boardManager.GetAdjacentsByIndex(playersIndex[currentPlayer]);
    }

    private void UpdateCanAttackAt()
    {
        canAttackAt.Clear();

        List<GameObject> possibleAttack = new List<GameObject>();
        
        foreach (GameObject tile in canMoveTo)
            possibleAttack.Add(tile);

        List<GameObject> diagonals = boardManager.GetDiagonalsByIndex(playersIndex[currentPlayer]);

        foreach (GameObject tile in diagonals)
            possibleAttack.Add(tile);

        foreach (GameObject tile in possibleAttack)
        {
            foreach (int index in playersIndex)
            {
                if (boardManager.GetIndexOfTile(tile) == index)
                    canAttackAt.Add(tile);
            }
        }
    }

    private void AddCurrentColors()
    {
        foreach (GameObject tile in canMoveTo)
            tile.GetComponent<MeshRenderer>().material.color = Color.green;

        foreach (GameObject tile in canAttackAt)
            tile.GetComponent<MeshRenderer>().material.color = Color.red;
    } 

    private void PlayerMadeMove()
    {
        currentPlayerMoves--;

        if (currentPlayerMoves <= 0)
            NextPlayer();

        UpdateMovePossibilities();
        LogPlayersInfo();
    }

    private void NextPlayer()
    {
        currentPlayer++;

        if (currentPlayer >= players.Count)
            currentPlayer = 0;

        currentPlayerMoves = 3;
    }

    private void SpawnCollectables()
    {
        boardManager.SpawnCollectables(playersIndex);
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                if (canMoveTo.Contains(hit.collider.gameObject))
                    MovePlayerTo(hit.collider.gameObject);

                else if (canAttackAt.Contains(hit.collider.gameObject))
                    AttackPlayerAt(hit.collider.gameObject);
            }
        }
    }

    private void MovePlayerTo(GameObject tile)
    {
        players[currentPlayer].transform.position = tile.transform.position;
        playersIndex[currentPlayer] = boardManager.GetIndexOfTile(tile);
        LookForCollectable(tile);
        PlayerMadeMove();
    }

    private void LookForCollectable(GameObject tile)
    {
        if (tile.transform.childCount > 0)
        {
            GameObject collectableObj = tile.transform.GetChild(0).gameObject;
            Collectable collectable = collectableObj.GetComponent<Collectable>();

            if (collectable != null)
                collectable.CollectedBy(players[currentPlayer].GetComponent<Player>());

            Destroy(collectableObj);
        }
    }

    private void AttackPlayerAt(GameObject tile)
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

    private void LogPlayersInfo()
    {
        logText.text = "Players Info:\n";
        for (int i = 0; i < players.Count; i++)
        {
            Player player = players[i].GetComponent<Player>();
            logText.text += "\nPlayer " + (i + 1) + ":\n";
            logText.text += "  Current player: " + (i == currentPlayer ? "Yes" : "No") + "\n";
            logText.text += "  Remaining moves: " + (i == currentPlayer ? currentPlayerMoves : 0) + "\n";
            logText.text += "  Health points: " + player.GetHealthPoints() + "\n";
            logText.text += "  Attack points: " + player.GetAttackPoints() + "\n";
        }
    }
}
