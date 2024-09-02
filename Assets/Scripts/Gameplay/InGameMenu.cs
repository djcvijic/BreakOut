using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text levelNumberField;
    [SerializeField] private TMP_Text powerUpTimerField;
    [SerializeField] private TMP_Text livesField;
    [SerializeField] private TMP_Text scoreField;
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject overlay;
    [SerializeField] private TMP_Text overlayTitle;
    [SerializeField] private Button unPauseButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        overlay.SetActive(false);
        pauseButton.SetListener(Pause);
        unPauseButton.SetListener(UnPauseOrNextLevel);
        restartButton.SetListener(Restart);
        quitButton.SetListener(Quit);
    }

    private void Update()
    {
        switch (GameController.Instance.CurrentState)
        {
            case GameController.State.LevelStarting:
                LevelStarting();
                return;
            case GameController.State.LevelComplete:
                LevelComplete();
                return;
            case GameController.State.GameOver:
                GameOver();
                return;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause();
            return;
        }

        levelNumberField.text = $"LEVEL {Meta.LevelIndex + 1}";
        if (GameController.Instance.PowerUpSecondsRemaining > 0)
        {
            powerUpTimerField.gameObject.SetActive(true);
            powerUpTimerField.text = $"POWER UP: {GameController.Instance.PowerUpSecondsRemaining}";
        }
        else
        {
            powerUpTimerField.gameObject.SetActive(false);
        }

        livesField.text = $"LIVES: {new string('\u2665', GameController.Instance.CurrentLives)}";
        scoreField.text = $"SCORE: {GameController.Instance.CurrentScore}";

        if (GameController.Instance.CurrentState == GameController.State.Playing)
        {
            overlay.SetActive(false);
        }
    }

    private void LevelStarting()
    {
        overlay.SetActive(true);
        overlayTitle.text = $"LEVEL {GameController.Instance.LevelIndex + 1}\nGET READY";
        unPauseButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }

    private void Playing()
    {
        overlay.SetActive(false);
    }

    private void LevelComplete()
    {
        overlay.SetActive(true);
        overlayTitle.text = $"LEVEL {GameController.Instance.LevelIndex + 1}\nCOMPLETE";
        unPauseButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }

    private void GameOver()
    {
        overlay.SetActive(true);
        overlayTitle.text =
            $"GAME OVER\nLEVEL: {GameController.Instance.LevelIndex + 1}\nSCORE: {GameController.Instance.CurrentScore}";
        unPauseButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    private void Pause()
    {
        if (GameController.Instance.CurrentState != GameController.State.Playing) return;

        GameController.Instance.Pause();
        overlay.SetActive(true);
        overlayTitle.text = "PAUSED";
        unPauseButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    private void UnPauseOrNextLevel()
    {
        switch (GameController.Instance.CurrentState)
        {
            case GameController.State.Paused:
                overlay.SetActive(false);
                GameController.Instance.UnPause();
                break;
            case GameController.State.LevelComplete:
                overlay.SetActive(false);
                GameController.Instance.NextLevel();
                break;
        }
    }

    private static void Restart()
    {
        if (GameController.Instance.CurrentState == GameController.State.GameOver)
        {
            Meta.LevelIndex = 0;
        }

        SceneManager.LoadScene("Scenes/Gameplay");
    }

    private static void Quit()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}
