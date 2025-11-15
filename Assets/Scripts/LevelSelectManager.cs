using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    private Button neighborhoodButton;
    private Button downtownButton;

    void OnEnable()
    {
        // Get UI Document root
        var uiDoc = GetComponent<UIDocument>();
        var root = uiDoc.rootVisualElement;

        // Find Buttons by their name in UXML
        neighborhoodButton = root.Q<Button>("NeighborhoodBtn");
        downtownButton = root.Q<Button>("DowntownBtn");

        // Hook up events
        if (neighborhoodButton != null)
            neighborhoodButton.clicked += () => LoadLevel("NeighborhoodStreet");

        if (downtownButton != null)
            downtownButton.clicked += () => LoadLevel("NeighborhoodStreet");
    }

    private void LoadLevel(string sceneName)
    {
        Debug.Log("Loading: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}