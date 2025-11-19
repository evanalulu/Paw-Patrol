using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference Primary;   // Ball
    public InputActionReference Secondary; // Bone
    public InputActionReference Move;

    [Header("Movement Settings")]
    public float maneuveringSpeed = 5f;
    public float topLimit = 3f;
    public float bottomLimit = -3f;
    public float sideLimit = 2.2f;

    [Header("Projectile Settings")]
    public GameObject bonePrefab;
    public GameObject ballPrefab;
    public float throwSpeed = 1f;
    public float spawnOffsetZ = 1f;
    public float fireCooldown = 0.25f;

    [Header("FX Settings")]
    public GameObject hitEffect;

    // [Header("UI Settings")]
    private GameUIController gameUI;

    private LevelConfig config;
    private float leftLimit;
    private float rightLimit;

    private Vector2 moveInput;
    private float lastFireTimePrimary = 0f;
    private float lastFireTimeSecondary = 0f;
    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        gameUI = FindObjectOfType<GameUIController>();

        string selected = PlayerPrefs.GetString("SelectedLevel", "NeighborhoodLevel");
        config = Resources.Load<LevelConfig>($"Levels/{selected}");

        if (config == null)
        {
            Debug.LogError($"❌ PlayerController could NOT load LevelConfig for: {selected}. Using fallback.");
            leftLimit = -0.9f;
            rightLimit = 0.9f;
        }
        else
        {
            // Use lane positions to determine movement boundaries
            float minLane = Mathf.Min(config.lanePositions);
            float maxLane = Mathf.Max(config.lanePositions);

            // Add small padding so player doesn't sit on lane edge
            leftLimit = minLane - 0.2f;
            rightLimit = maxLane + 0.2f;

            Debug.Log($"🎮 Movement bounds updated → {leftLimit} to {rightLimit}");
        }
    }

    void OnEnable()
    {
        Primary.action.Enable();
        Secondary.action.Enable();
        Move.action.Enable();

        Primary.action.started += OnPrimary;     // Space → Ball
        Secondary.action.started += OnSecondary; // Ctrl → Bone
        Move.action.performed += OnMove;
        Move.action.canceled += OnMove;
    }

    void OnDisable()
    {
        Primary.action.Disable();
        Secondary.action.Disable();
        Move.action.Disable();

        Primary.action.started -= OnPrimary;
        Secondary.action.started -= OnSecondary;
        Move.action.performed -= OnMove;
        Move.action.canceled -= OnMove;
    }

    void Update() => MovePlayer();

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void MovePlayer()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y) * maneuveringSpeed * Time.deltaTime;
        transform.position += move;

        float bottomLimit = -6f;
        float topLimit = -3f;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);
        pos.z = Mathf.Clamp(pos.z, bottomLimit, topLimit);
        transform.position = pos;
    }

    // SPACE → Ball
    void OnPrimary(InputAction.CallbackContext context)
    {
        if (Time.time - lastFireTimePrimary < fireCooldown) return;
        lastFireTimePrimary = Time.time;
        ThrowProjectile(ballPrefab, "Ball");

        audioManager?.Play(audioManager.AttackSound);
    }

    // CTRL → Bone
    void OnSecondary(InputAction.CallbackContext context)
    {
        if (Time.time - lastFireTimeSecondary < fireCooldown) return;
        lastFireTimeSecondary = Time.time;
        ThrowProjectile(bonePrefab, "Bone");

        audioManager?.Play(audioManager.AttackSound);
    }

    void ThrowProjectile(GameObject prefab, string type)
    {
        if (prefab == null) return;

        Vector3 spawnPos = transform.position + new Vector3(0f, 0.16f, spawnOffsetZ);
        Quaternion spawnRot = Quaternion.Euler(90f, 0f, 0f);

        GameObject projectile = Instantiate(prefab, spawnPos, spawnRot);
        projectile.AddComponent<TreatMover>().Init(throwSpeed, type, audioManager, gameUI);
    }

    // Collision detection — lose a life if hit by a car
    void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Enemy") && !collision.collider.CompareTag("Firetruck") &&
            !collision.collider.CompareTag("Rescue"))
            return;

        int newHealth = gameUI.Health; // start with current health

        if (collision.collider.CompareTag("Enemy")) { newHealth -= 34; }
        else if (collision.collider.CompareTag("Rescue")) { newHealth -= 34;}
        else if (collision.collider.CompareTag("Firetruck")) { newHealth = 0;}

        gameUI.SetHealth(newHealth);

        audioManager?.Play(audioManager.CollisionSound);

        if (hitEffect != null)
        {
            Quaternion fxRotation = Quaternion.Euler(90f, 180f, 0f);
            GameObject fx = Instantiate(hitEffect, transform.position + new Vector3(0f, 0.1f, 0f), fxRotation);
            Destroy(fx, 0.417f);
        }

        // Check if health depleted
        if (gameUI.Health <= 0)
        {
            gameUI.LoseLife();
            gameUI.SetHealth(100); // Reset for next life
            Debug.Log($"❤️ Lost a Life! Lives left: {gameUI.Lives}");
        }

        // Game over
        if (gameUI.Lives <= 0)
        {
            Debug.Log("🚨 GAME OVER!");
            Time.timeScale = 0f;
            audioManager?.Play(audioManager.GameOverSound);
            audioManager.Music.Stop();
            gameUI.ShowGameOver();
        }
    }
}

// Helper class for projectile motion
public class TreatMover : MonoBehaviour
{
    private float speed;
    private string type; // Ball or Bone
    private AudioManager audioManager;
    private GameUIController gameUI;

    public void Init(float moveSpeed, string projectileType, AudioManager manager, GameUIController ui)
    {
        speed = moveSpeed;
        type = projectileType;
        audioManager = manager;
        gameUI = ui;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);

        if (transform.position.z > 12f)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Bullet hit: {collision.collider.name}");

        // Mega rescue
        MegaRescueTarget mega = collision.collider.GetComponent<MegaRescueTarget>();
        if (mega != null)
        {
            mega.TakeHit(type);
            Destroy(gameObject);
            audioManager?.PlayRandomPetSound();
            Debug.Log($"🚌 Hit mega rescue with {type}");
            return;
        }

        // Normal rescue
        if (collision.collider.CompareTag("Rescue"))
        {
            gameUI?.AddScore(10);
            RescueTarget rescue = collision.collider.GetComponent<RescueTarget>();
            rescue.TakeHit(type);
            audioManager?.PlayRandomPetSound();
            
        }
        
        Destroy(gameObject);
    }
}
