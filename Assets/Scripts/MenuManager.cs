using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject optionsPanel;
    public GameObject quitPanel;

    private const float animationDuration = 0.5f;
    private const float animationDistance = 500f;

    private void Start()
    {
        menuPanel.transform.position = new Vector3(0, menuPanel.transform.position.y, menuPanel.transform.position.z);
        gamePanel.transform.position = new Vector3(animationDistance, gamePanel.transform.position.y, gamePanel.transform.position.z);
        optionsPanel.transform.position = new Vector3(-animationDistance, optionsPanel.transform.position.y, optionsPanel.transform.position.z);
        quitPanel.transform.position = new Vector3(animationDistance, quitPanel.transform.position.y, quitPanel.transform.position.z);
    }

    public void ShowGamePanel()
    {
        HideMenuPanel();
        StartCoroutine(AnimatePanel(gamePanel, true));
    }

    public void ShowOptionsPanel()
    {
        HideMenuPanel();
        StartCoroutine(AnimatePanel(optionsPanel, true));
    }

    public void ShowQuitConfirmation()
    {
        HideMenuPanel();
        StartCoroutine(AnimatePanel(quitPanel, true));
    }

    public void HideGamePanel()
    {
        StartCoroutine(AnimatePanel(gamePanel, false));
        ShowMenuPanel();
    }

    public void HideOptionsPanel()
    {
        StartCoroutine(AnimatePanel(optionsPanel, false));
        ShowMenuPanel();
    }

    public void HideQuitConfirmation()
    {
        StartCoroutine(AnimatePanel(quitPanel, false));
        ShowMenuPanel();
    }

    public void ConfirmQuit()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void EnableSound(bool enable)
    {
        // TODO: Implement sound toggle logic
    }

    private void ShowMenuPanel()
    {
        StartCoroutine(AnimatePanel(menuPanel, true));
    }

    private void HideMenuPanel()
    {
        StartCoroutine(AnimatePanel(menuPanel, false));
    }

    private IEnumerator AnimatePanel(GameObject panel, bool show)
    {
        float t = 0f;
        Vector3 startPos = panel.transform.position;
        Vector3 endPos = new Vector3(0, panel.transform.position.y, panel.transform.position.z);

        if (!show)
            endPos = new Vector3(startPos.x < 0 ? -animationDistance : animationDistance, startPos.y, startPos.z);

        while (t < animationDuration)
        {
            t += Time.deltaTime;
            panel.transform.position = Vector3.Lerp(startPos, endPos, t / animationDuration);
            yield return null;
        }

        if (show)
            panel.transform.position = new Vector3(0, panel.transform.position.y, panel.transform.position.z);
    }
}