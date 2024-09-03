using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private Button backButton;

    private void Awake()
    {
        backButton.SetListener(GoBack);
    }

    private void GoBack()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}
