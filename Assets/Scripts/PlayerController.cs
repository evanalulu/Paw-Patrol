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

    [Header("Player Health")]
    public int hitPoints = 3; // start with 3 lives

    [Header("FX Settings")]
    public GameObject hitEffect; // drag FX001_01.prefab here in Inspector

    private Vector2 moveInput;

    void OnEnable()
    {
        Primary.action.Enable();
        Move.action.Enable();

        Primary.action.started += OnPrimary;
        Move.action.performed += OnMove;
        Move.action.canceled += OnMove;
    }

    void OnDisable()
    {
        Primary.action.Disable();
        Move.action.Disable();

        Primary.action.started -= OnPrimary;
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
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y) * maneuveringSpeed * Time.deltaTime;
        transform.position += move;

        float leftLimit = -0.9f;
        float rightLimit = 0.9f;
        float bottomLimit = -6f;
        float topLimit = -3f;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);
        pos.z = Mathf.Clamp(pos.z, bottomLimit, topLimit);
        transform.position = pos;
    }

    void OnPrimary(InputAction.CallbackContext context)
    {
        Debug.Log("Space pressed");
        GameObject prefabToThrow = Random.value > 0.5f ? bonePrefab : ballPrefab;
        if (prefabToThrow == null) return;

        Vector3 spawnPos = transform.position + new Vector3(0f, 0.16f, spawnOffsetZ);
        Quaternion spawnRot = Quaternion.Euler(90f, 0f, 0f);

        GameObject projectile = Instantiate(prefabToThrow, spawnPos, spawnRot);
        projectile.AddComponent<TreatMover>().Init(throwSpeed);
    }

    // Collision detection — lose a life if hit by a car
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            hitPoints--;
            Debug.Log($"💥 Player hit! Remaining lives: {hitPoints}");

            // Spawn FX
            if (hitEffect != null)
            {
                Quaternion fxRotation = Quaternion.Euler(90f, 0f, 0f);
                GameObject fx = Instantiate(hitEffect, transform.position + new Vector3(0f, 0.1f, 0f), fxRotation);
                Destroy(fx, 0.417f);
            }

            // Stop the game if out of lives
            if (hitPoints <= 0)
            {
                Debug.Log("🚨 Game Over! Out of lives.");
                Time.timeScale = 0.0f;
            }

            // Destroy the car that player hit
            Destroy(collision.gameObject);
        }
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

        // Destroy when off screen
        if (transform.position.z > 12f)
        {
            Destroy(gameObject);
        }
    }
}