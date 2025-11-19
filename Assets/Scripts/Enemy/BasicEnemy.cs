using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
  [Header("Shooting Settings")]
  public GameObject bulletPrefab;   // Bullet to shoot
  public Transform firePoint;       // Where the bullet spawns
  protected float fireRate = 4f;
  private float nextFireTime;
  public float flyInShootingDelay = 0.5f; // Time to wait before shooting after fly-in

  [Header("Health Settings")]
  protected int maxHealth = 2;
  protected int currentHealth;

  [HideInInspector]
  public bool canShoot = false; // Only shoot when allowed

  [Header("Pickup Settings")]
  public GameObject coinPrefab;
  public float coinDropChance = 0.6f;


  // Called by EnemyFlyIn when the enemy reaches its target
  public void EnableShooting()
  {
    StartCoroutine(WaitThenShoot());
  }

  protected virtual IEnumerator WaitThenShoot()
  {
    yield return new WaitForSeconds(flyInShootingDelay);
    canShoot = true;
    nextFireTime = Time.time; // Start shooting immediately after delay
  }

  void Start()
  {
    currentHealth = maxHealth;
  }

  protected virtual void Update()
  {
    if (canShoot && Time.time >= nextFireTime)
    {
      Shoot();
      nextFireTime = Time.time + fireRate;
    }
  }

  protected virtual void Shoot()
  {
    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    bullet.GetComponent<Rigidbody2D>().velocity = firePoint.up * 5f; // Bullet moves in firePoint's forward direction
  }

  public void TakeDamage(int damage)
  {
    currentHealth -= damage;

    if (currentHealth <= 0)
    {
      Die();
    }
  }

  void Die()
  {
    // Drop coin
    if (coinPrefab != null && Random.value <= coinDropChance)
    {
      Instantiate(coinPrefab, transform.position, Quaternion.identity);
    }

    // TODO: Add explosion VFX here later
    Destroy(gameObject);

    // Register enemy death stats
    SceneUIManager.Instance.RegisterEnemyDestroyed();

  }
}

