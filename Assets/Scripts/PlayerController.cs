using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference Primary;
    public InputActionReference Move;

    [Header("Movement Settings")]
    public float maneuveringSpeed = 5f;
    public float topLimit = 3f;
    public float bottomLimit = -3f;
    public float sideLimit = 2.2f;

    private Vector2 moveInput;

    void OnEnable()
    {
        // Enable input actions
        Primary.action.Enable();
        Move.action.Enable();

        // Sub events
        Primary.action.started += OnPrimary;
        Move.action.performed += OnMove;
        Move.action.canceled += OnMove;
    }

    void OnDisable()
    {
        // Disable input actions
        Primary.action.Disable();
        Move.action.Disable();

        // Unsub events
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
				// Move player smoothly
				Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y) * maneuveringSpeed * Time.deltaTime;
				transform.position += move;

				// Lane boundaries
				float leftLimit = -0.9f;  // left lane edge
				float rightLimit = 0.9f;  // right lane edge

				// Lower 2/3 box bounds
				float bottomLimit = -6f;
				float topLimit = -3f;

				// Clamp pos
				Vector3 pos = transform.position;
				pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);
				pos.z = Mathf.Clamp(pos.z, bottomLimit, topLimit);
				transform.position = pos;
		}

    void OnPrimary(InputAction.CallbackContext context)
    {
        Debug.Log("Press the mf space");
    }
}