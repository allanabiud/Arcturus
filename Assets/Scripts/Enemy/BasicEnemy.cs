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
  public float shieldDropChance = 0.1f;      // 10% chance to drop
  public float shieldDropCooldown = 5f;      // seconds before the same enemy can drop again
  public static bool ShieldActiveInScene = false;

  [Header("Ship Upgrade Drop Settings")]
  public GameObject shipUpgradeLv2Prefab;
  public float shipUpgradeLv2Chance = 0.08f;  // 8% chance to drop
  public GameObject shipUpgradeLv3Prefab;
  public float shipUpgradeLv3Chance = 0.03f;  // 3% chance to drop
  public float shipUpgradeDropCooldown = 10f; // delay before another upgrade can spawn
  // Track active upgrades in scene
  public static bool ShipUpgradeLv2ActiveInScene = false;
  public static bool ShipUpgradeLv3ActiveInScene = false;

  [HideInInspector]
  public float lastShieldDropTime = -999f;
  public static float lastShipUpgradeSpawnTime = -999f;

  [Header("Audio")]
  public AudioSource audioSource;
  public AudioClip shootSFX;


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

    // Play shooting sound if SFX is enabled
    if (SettingsManager.IsSfxEnabled && audioSource != null && shootSFX != null)
    {
      audioSource.PlayOneShot(shootSFX);
    }
  }

  public void TakeDamage(int damage)
  {
    currentHealth -= damage;
    bool didDropPickup = false;

    // Don't spawn pickups while flying in
    if (!canShoot)
      return;

    // --- Shield drop ---
    if (shieldPickupPrefab != null && CanDropShield() && !ShieldActiveInScene && Random.value <= shieldDropChance)
    {
      Instantiate(shieldPickupPrefab, transform.position, Quaternion.identity);
      RecordShieldDrop();
      ShieldActiveInScene = true;
      didDropPickup = true;
    }

    // --- Ship upgrade drop ---
    if (!didDropPickup)
    {
      // Only spawn if no upgrade is active and cooldown passed
      if (!ShipUpgradeLv2ActiveInScene && !ShipUpgradeLv3ActiveInScene &&
          Time.time >= lastShipUpgradeSpawnTime + shipUpgradeDropCooldown)
      {
        // Try Lv2 first
        if (shipUpgradeLv2Prefab != null && Random.value <= shipUpgradeLv2Chance)
        {
          Instantiate(shipUpgradeLv2Prefab, transform.position, Quaternion.identity);
          ShipUpgradeLv2ActiveInScene = true;
          lastShipUpgradeSpawnTime = Time.time;
          didDropPickup = true;
        }
        // If Lv2 didn't drop, try Lv3
        else if (shipUpgradeLv3Prefab != null && Random.value <= shipUpgradeLv3Chance)
        {
          Instantiate(shipUpgradeLv3Prefab, transform.position, Quaternion.identity);
          ShipUpgradeLv3ActiveInScene = true;
          lastShipUpgradeSpawnTime = Time.time;
          didDropPickup = true;
        }
      }
    }

    if (currentHealth <= 0)
    {
      Die(didDropPickup);
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

