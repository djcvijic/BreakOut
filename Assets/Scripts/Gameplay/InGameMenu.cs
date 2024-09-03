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
        switch (Gameplay.Instance.CurrentState)
        {
            case Gameplay.State.LevelStarting:
                LevelStarting();
                return;
            case Gameplay.State.LevelComplete:
                LevelComplete();
                return;
            case Gameplay.State.GameOver:
                GameOver();
                return;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause();
            return;
        }

        levelNumberField.text = $"LEVEL {Meta.LevelIndex + 1}";
        if (Gameplay.Instance.PowerUpSecondsRemaining > 0)
        {
            powerUpTimerField.gameObject.SetActive(true);
            powerUpTimerField.text =
                $"{Gameplay.Instance.ActivePowerUp.Name}: {Gameplay.Instance.PowerUpSecondsRemaining}";
        }
        else
        {
            powerUpTimerField.gameObject.SetActive(false);
        }

        livesField.text = $"LIVES: {new string('\u2665', Gameplay.Instance.CurrentLives)}";
        scoreField.text = $"SCORE: {Gameplay.Instance.CurrentScore}";

        if (Gameplay.Instance.CurrentState == Gameplay.State.Playing)
        {
            overlay.SetActive(false);
        }
    }

    private void LevelStarting()
    {
        overlay.SetActive(true);
        overlayTitle.text = $"LEVEL {Gameplay.Instance.LevelIndex + 1}\nGET READY";
        unPauseButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }

    private void LevelComplete()
    {
        overlay.SetActive(true);
        overlayTitle.text = $"LEVEL {Gameplay.Instance.LevelIndex + 1}\nCOMPLETE";
        unPauseButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }

    private void GameOver()
    {
        overlay.SetActive(true);
        overlayTitle.text =
            $"GAME OVER\nLEVEL: {Gameplay.Instance.LevelIndex + 1}\nSCORE: {Gameplay.Instance.CurrentScore}";
        unPauseButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    private void Pause()
    {
        if (Gameplay.Instance.CurrentState != Gameplay.State.Playing) return;

        Gameplay.Instance.Pause();
        overlay.SetActive(true);
        overlayTitle.text = "PAUSED";
        unPauseButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    private void UnPauseOrNextLevel()
    {
        switch (Gameplay.Instance.CurrentState)
        {
            case Gameplay.State.Paused:
                overlay.SetActive(false);
                Gameplay.Instance.UnPause();
                break;
            case Gameplay.State.LevelComplete:
                overlay.SetActive(false);
                Gameplay.Instance.NextLevel();
                break;
        }
    }

    private static void Restart()
    {
        if (Gameplay.Instance.CurrentState == Gameplay.State.GameOver)
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
