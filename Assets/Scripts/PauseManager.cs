using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class PauseManager : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("The panel that is shown when the game is paused.")]
    [SerializeField] private GameObject pausePanel;
    [Tooltip("The text that shows the winner of the game.")]
    [SerializeField] private TextMeshProUGUI winnerText;

    void Start()
    {
        pausePanel.SetActive(false);
    }

    public void ShowPanel(string winner)
    {
        winnerText.text = "Congratulations player " + winner + "!";
        pausePanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}