using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float bulletDamage = 20f;
    [SerializeField] private float damageIncreasePerLevel = 5f;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private AudioSource shootingAudio;

    private float nextFireTime;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {

        if (playerHealth != null && playerHealth.IsDead)
        {
            return;

        }
        if (Mouse.current != null &&
            Mouse.current.leftButton.isPressed &&
            Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        if (muzzleFlash != null)
        {
            ParticleSystem flash =
                muzzleFlash.GetComponent<ParticleSystem>();

            if (flash != null)
            {
                flash.Play();
            }
        }
        if (shootingAudio != null)
        {
            shootingAudio.Play();
        }

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.SetDamage(bulletDamage);
        }

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        if (bulletRb != null)
        {
            bulletRb.linearVelocity =
                firePoint.forward * bulletSpeed;
        }

        Destroy(bullet, 3f);
    }
    public void IncreaseDamage()
    {
        bulletDamage += damageIncreasePerLevel;

        Debug.Log("Bullet Damage Increased: " + bulletDamage);
    }
}