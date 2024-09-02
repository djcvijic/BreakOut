using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Transform buttonHolder;
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private Button levelEditorButton;

    private void Awake()
    {
        ClearButtons();
        SetUpLevels();
        SetUpLevelEditor();
    }

    private void ClearButtons()
    {
        buttonHolder.ClearTransform(levelEditorButton.transform);
    }

    private void SetUpLevels()
    {
        var levelCount = LevelLoader.CountAllLevels();
        for (var i = 0; i < levelCount; i++)
        {
            var button = Instantiate(buttonPrefab, buttonHolder);
            button.GetComponentInChildren<TMP_Text>()
                    .text = $"Level {i + 1}";

            var levelIndex = i;
            button.SetListener(() => LoadLevel(levelIndex));
        }
    }

    private static void LoadLevel(int levelIndex)
    {
        Meta.LevelIndex = levelIndex;
        SceneManager.LoadScene("Scenes/Gameplay");
    }

    private void SetUpLevelEditor()
    {
        levelEditorButton.transform.SetAsLastSibling();
        levelEditorButton.SetListener(LoadLevelEditor);
    }

    private static void LoadLevelEditor()
    {
        SceneManager.LoadScene("Scenes/LevelEditor");
    }
}
