using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;

    private Rigidbody rb;
    private Vector2 movementInput;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        movementInput = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)
                movementInput.y += 1f;

            if (Keyboard.current.sKey.isPressed)
                movementInput.y -= 1f;

            if (Keyboard.current.dKey.isPressed)
                movementInput.x += 1f;

            if (Keyboard.current.aKey.isPressed)
                movementInput.x -= 1f;
        }

        movementInput = Vector2.ClampMagnitude(movementInput, 1f);
    }

    private void FixedUpdate()
    {
        if (playerHealth != null && playerHealth.IsDead)
        {
            return;
        }
        Vector3 movement = new Vector3(
            movementInput.x,
            0f,
            movementInput.y
        );

        Vector3 targetPosition =
            rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(targetPosition);

        if (movement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(movement);

            rb.MoveRotation(
                Quaternion.Slerp(
                    rb.rotation,
                    targetRotation,
                    10f * Time.fixedDeltaTime
                )
            );
        }
    }
    public void IncreaseMoveSpeed()
    {
        moveSpeed += 0.5f;

        Debug.Log("Move Speed Increased: " + moveSpeed);
    }
}