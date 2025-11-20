using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class BackstoryManager : MonoBehaviour
{
    private VisualElement background;
    private Button nextButton;

    private string selectedLevel;
    private int step = 0;

    [Header("Neighborhood Screens")]
    public Texture2D neighborhoodBackstory;
    public Texture2D neighborhoodHowToPlay;

    [Header("Downtown Screens")]
    public Texture2D downtownHowToPlay;

    void OnEnable()
    {
        selectedLevel = PlayerPrefs.GetString("SelectedLevel", "NeighborhoodLevel");

        var root = GetComponent<UIDocument>().rootVisualElement;
        background = root.Q<VisualElement>("Background");
        nextButton = root.Q<Button>("NextBtn");

        nextButton.clicked += OnNext;
        
        // Add sound effects to button
        UIButtonSoundHandler.AddSoundEffects(nextButton);

        if (selectedLevel == "NeighborhoodLevel")
            ShowNeighborhood(0);
        else
            ShowDowntown(0);
    }

    void OnNext()
    {
        step++;

        if (selectedLevel == "NeighborhoodLevel")
            ShowNeighborhood(step);
        else
            ShowDowntown(step);
    }

    void ShowNeighborhood(int step)
    {
        switch (step)
        {
            case 0:
                background.style.backgroundImage = new StyleBackground(neighborhoodBackstory);
                break;

            case 1:
                background.style.backgroundImage = new StyleBackground(neighborhoodHowToPlay);
                break;

            case 2:
                SceneManager.LoadScene("NeighborhoodStreet"); 
                break;
        }
    }

    void ShowDowntown(int step)
    {
        switch (step)
        {
            case 0:
                background.style.backgroundImage = new StyleBackground(downtownHowToPlay);
                break;

            case 1:
                SceneManager.LoadScene("NeighborhoodStreet");
                break;
        }
    }
}