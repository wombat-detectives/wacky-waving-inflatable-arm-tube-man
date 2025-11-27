using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Win Condition")]
    public float winDistance = 100f;   

    [Header("UI Screens")]
    public GameObject gameOverScreen;
    public GameObject gameWinScreen;

    [Header("Text Fields")]
    public TextMeshProUGUI finalDistanceText;
    public TextMeshProUGUI finalTimeText;

    [Header("References")]
    public DistanceTracker2D distanceTracker;

    private float startTime;
    private bool isGameEnded = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        startTime = Time.time;
    }

    void Update()
    {
        if (!isGameEnded)
            CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (distanceTracker == null) return;

        if (distanceTracker.distanceTraveled >= winDistance)
        {
            GameWin();
        }
    }

    public void GameOver()
    {
        if (isGameEnded) return;

        isGameEnded = true;
        Time.timeScale = 0f;

        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);

        if (finalDistanceText != null && distanceTracker != null)
            finalDistanceText.text = "Distance: " + distanceTracker.distanceTraveled.ToString("F1");
    }

    public void GameWin()
    {
        if (isGameEnded) return;

        isGameEnded = true;
        Time.timeScale = 0f;

        if (gameWinScreen != null)
            gameWinScreen.SetActive(true);

        float finalTime = Time.time - startTime;

        if (finalTimeText != null)
            finalTimeText.text = "Time: " + finalTime.ToString("F2") + "s";

        if (finalDistanceText != null && distanceTracker != null)
            finalDistanceText.text = "Distance: " + distanceTracker.distanceTraveled.ToString("F1");
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
