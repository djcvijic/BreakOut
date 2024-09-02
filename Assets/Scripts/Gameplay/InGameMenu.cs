using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject overlay;
    [SerializeField] private Button unPauseButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        overlay.SetActive(false);
        pauseButton.SetListener(Pause);
        unPauseButton.SetListener(UnPause);
        restartButton.SetListener(Restart);
        quitButton.SetListener(Quit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
    }

    private void Pause()
    {
        GameController.Instance.Pause();
        overlay.SetActive(true);
    }

    private void UnPause()
    {
        overlay.SetActive(false);
        GameController.Instance.UnPause();
    }

    private static void Restart()
    {
        SceneManager.LoadScene("Scenes/Gameplay");
    }

    private static void Quit()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}
