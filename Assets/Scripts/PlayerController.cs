using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference Primary;
    public InputActionReference Secondary;
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

    private Vector2 moveInput;

    void OnEnable()
    {
        // Enable input actions
        Primary.action.Enable();
        Secondary.action.Enable();
        Move.action.Enable();

        // Sub to input events
        Primary.action.started += OnPrimary;     // Space -> Bone
        Secondary.action.started += OnSecondary; // Ctrl -> Ball
        Move.action.performed += OnMove;
        Move.action.canceled += OnMove;
    }

    void OnDisable()
    {
        // Disable input actions
        Primary.action.Disable();
        Secondary.action.Disable();
        Move.action.Disable();

        // Unsub to avoid leaks
        Primary.action.started -= OnPrimary;
        Secondary.action.started -= OnSecondary;
        Move.action.performed -= OnMove;
        Move.action.canceled -= OnMove;
    }

    void Update()
    {
        MovePlayer();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void MovePlayer()
    {
        // Move player smoothly
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y) * maneuveringSpeed * Time.deltaTime;
        transform.position += move;

        // Lane boundaries
        float leftLimit = -0.9f;  // left lane edge
        float rightLimit = 0.9f;  // right lane edge

        // Lower 2/3 box bounds
        float bottomLimit = -6f;
        float topLimit = -3f;

        // Clamp position
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);
        pos.z = Mathf.Clamp(pos.z, bottomLimit, topLimit);
        transform.position = pos;
    }

    void OnPrimary(InputAction.CallbackContext context)
    {
        Debug.Log("Space pressed → throw Bone");
        ThrowProjectile(bonePrefab);
    }

    void OnSecondary(InputAction.CallbackContext context)
    {
        Debug.Log("Ctrl pressed → throw Ball");
        ThrowProjectile(ballPrefab);
    }

    void ThrowProjectile(GameObject prefabToThrow)
    {
        if (prefabToThrow == null)
        {
            Debug.LogWarning("⚠ No prefab assigned!");
            return;
        }

        // Spawn just above the player
        Vector3 spawnPos = transform.position + new Vector3(0f, 0.16f, spawnOffsetZ);

        Quaternion spawnRot = Quaternion.Euler(90f, 0f, 0f);
        GameObject projectile = Instantiate(prefabToThrow, spawnPos, spawnRot);
        projectile.AddComponent<TreatMover>().Init(throwSpeed);

        Debug.Log($"Spawned {projectile.name} at {spawnPos}");
    }
}

// Helper class for projectile motion
public class TreatMover : MonoBehaviour
{
    private float speed;

    public void Init(float moveSpeed)
    {
        speed = moveSpeed;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);

        // Ddestroy when off screen
        if (transform.position.z > 12f)
        {
            Destroy(gameObject);
        }
    }
}