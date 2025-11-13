using UnityEngine;
using UnityEngine.UIElements;

public class GameUIController : MonoBehaviour
{
    // UI references
    private Label scoreLabel;
    private Label healthLabel;
    private VisualElement livesContainer;

    // Game stats
    private int score = 0;
    private int health = 100;
    private int lives = 3;

    void OnEnable()
    {
        // Get root visual element
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Find UI elements by name
        scoreLabel = root.Q<Label>("Score");
        healthLabel = root.Q<Label>("Health");
        livesContainer = root.Q<VisualElement>("LivesContainer");

        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScore();
    }

    public void SetHealth(int amount)
    {
        health = Mathf.Clamp(amount, 0, 100);
        UpdateHealth();
    }

    public void LoseLife()
    {
        lives = Mathf.Max(0, lives - 1);
        UpdateLives();
    }

    private void UpdateUI()
    {
        UpdateScore();
        UpdateHealth();
        UpdateLives();
    }

    private void UpdateScore()
    {
        if (scoreLabel != null)
            scoreLabel.text = $"Score: {score}";
    }

    private void UpdateHealth()
    {
        if (healthLabel != null)
            healthLabel.text = $"Health: {health}";
    }

    private void UpdateLives()
    {
        if (livesContainer != null)
        {
            livesContainer.Clear();

            for (int i = 0; i < lives; i++)
            {
                VisualElement lifeIcon = new VisualElement();
                lifeIcon.AddToClassList("life-icon");
                livesContainer.Add(lifeIcon);
            }
        }
    }
}