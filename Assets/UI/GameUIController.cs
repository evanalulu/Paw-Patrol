using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    // Images
    public Sprite hearts3;
    public Sprite hearts2;
    public Sprite hearts1;
    public Sprite hearts0;

    // UI references
    private Label scoreLabel;
    private Label healthLabel;
    private VisualElement heartsBar;

    private VisualElement gameOverScreen;
    private Label finalScoreLabel;
    private Button playAgainBtn;
    private Button backBtn;

    // Game stats
    private int score = 0;
    private int health = 100;
    private int lives = 3;

    public int Lives => lives;
    public int Health => health;

    void OnEnable()
    {
        // Get root visual element
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Find UI elements by name
        scoreLabel = root.Q<Label>("Score");
        healthLabel = root.Q<Label>("Health");
        heartsBar = root.Q<VisualElement>("HeartsBar");

        gameOverScreen = root.Q<VisualElement>("GameOverRoot");
        finalScoreLabel = root.Q<Label>("ScoreLabel");
        playAgainBtn = root.Q<Button>("PlayAgainBtn");
        backBtn = root.Q<Button>("BackBtn");

        //Hide game over on start
        if (gameOverScreen != null)
            gameOverScreen.style.display = DisplayStyle.None;
        
        // Hook up button clicks
        if (playAgainBtn != null)
            playAgainBtn.clicked += RestartGame;

        if (backBtn != null)
            backBtn.clicked += ReturnToMenu;

        UpdateUI();
        SetLives(lives);
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
        SetLives(lives);
    }

    private void UpdateUI()
    {
        UpdateScore();
        UpdateHealth();
        SetLives(lives);
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

    public void SetLives(int lives)
    {
        switch (lives)
        {
            case 3:
                heartsBar.style.backgroundImage = new StyleBackground(hearts3);
                break;
            case 2:
                heartsBar.style.backgroundImage = new StyleBackground(hearts2);
                break;
            case 1:
                heartsBar.style.backgroundImage = new StyleBackground(hearts1);
                break;
            default:
                heartsBar.style.backgroundImage = new StyleBackground(hearts0);
                break;
        }
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0f;

        gameOverScreen.style.display = DisplayStyle.Flex;
        finalScoreLabel.text = $"Score: {score}";
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ReturnToMenu()
    {
        Time.timeScale = 1f; // unpause
        SceneManager.LoadScene("MainMenu");
    }
}