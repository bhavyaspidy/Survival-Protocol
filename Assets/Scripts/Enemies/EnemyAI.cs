using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private float attackRange = 1.6f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;

    private Rigidbody rb;
    private Transform player;
    private float nextAttackTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        FindPlayer();
    }

    private void FindPlayer()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError(
                "EnemyAI: No GameObject with Player tag found."
            );
        }
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }
        PlayerHealth playerHealth =
        player.GetComponent<PlayerHealth>();

        if (playerHealth != null && playerHealth.IsDead)
        {
            return;
        }

        Vector3 direction = player.position - rb.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            rb.MoveRotation(
                Quaternion.Slerp(
                    rb.rotation,
                    targetRotation,
                    10f * Time.fixedDeltaTime
                )
            );
        }

        if (distance > stopDistance)
        {
            direction.Normalize();

            Vector3 targetPosition =
                rb.position +
                direction * moveSpeed * Time.fixedDeltaTime;

            rb.MovePosition(targetPosition);
        }

        if (distance <= attackRange)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        if (Time.time < nextAttackTime)
            return;

        PlayerHealth playerHealth =
            player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            nextAttackTime =
                Time.time + attackCooldown;
        }
    }
}