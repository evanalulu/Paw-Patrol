using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string levelSelectSceneName = "LevelSelect";

    void OnEnable()
    {
        // Get UI Document root
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Find button by name
        Button playButton = root.Q<Button>("PlayBtn");
        // Hook event
        playButton.clicked += OnPlayClicked;
    }

    void OnPlayClicked()
    {
        Debug.Log("Play Button Clicked!");
        SceneManager.LoadScene(levelSelectSceneName);
    }
}