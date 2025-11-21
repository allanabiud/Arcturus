using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
  [Header("Shooting Settings")]
  public GameObject bulletPrefab;   // Bullet to shoot
  public Transform firePoint;       // Where the bullet spawns
  protected float fireRate = 3.5f;
  private float nextFireTime;
  public float flyInShootingDelay = 0.5f; // Time to wait before shooting after fly-in

  [Header("Health Settings")]
  protected int maxHealth = 3;
  protected int currentHealth;

  [HideInInspector]
  public bool canShoot = false; // Only shoot when allowed

  [Header("Coin Drop Settings")]
  public GameObject coinPrefab;
  public float coinDropChance = 0.6f;
  public int coinValue = 1;

  [Header("Shield Drop Settings")]
  public GameObject shieldPickupPrefab;
  public float shieldDropChance = 0.1f;      // chance to drop
  public float shieldDropCooldown = 5f;      // seconds before the same enemy can drop again
  public static bool ShieldActiveInScene = false;

  [HideInInspector]
  public float lastShieldDropTime = -999f;   // track last drop time


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

    bool didDropShield = false;

    // Only try to drop shield if enemy survives the hit, cooldown allows, AND no shield already exists
    if (shieldPickupPrefab != null && CanDropShield() && !ShieldActiveInScene && Random.value <= shieldDropChance)
    {
      Instantiate(shieldPickupPrefab, transform.position, Quaternion.identity);
      RecordShieldDrop();
      ShieldActiveInScene = true;
      didDropShield = true;
    }

    if (currentHealth <= 0)
    {
      Die(didDropShield);
    }
  }

  protected virtual void Die(bool droppedShield)
  {
    // Only drop coin if shield was not dropped on this hit
    if (!droppedShield && coinPrefab != null && Random.value <= coinDropChance)
    {
      GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);

      // Set coin value dynamically
      CoinPickup coinScript = coin.GetComponent<CoinPickup>();
      if (coinScript != null)
      {
        coinScript.coinValue = coinValue;
      }
    }

    Destroy(gameObject);
    SceneUIManager.Instance.RegisterEnemyDestroyed();
  }

  public bool CanDropShield()
  {
    return Time.time >= lastShieldDropTime + shieldDropCooldown;
  }

  public void RecordShieldDrop()
  {
    lastShieldDropTime = Time.time;
  }
}

