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
            neighborhoodButton.clicked += () => SelectLevel("NeighborhoodLevel");

        if (downtownButton != null)
            downtownButton.clicked += () => SelectLevel("DowntownLevel");
    }

    public void SelectLevel(string configName)
    {
        PlayerPrefs.SetString("SelectedLevel", configName);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Backstory");
    }
}