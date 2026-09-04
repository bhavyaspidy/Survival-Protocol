using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;

    public void SetDamage(float amount)
    {
        damage = amount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyHealth enemyHealth =
            collision.gameObject.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}