using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBossEnemy : BasicEnemy
{
  [Header("Boss Settings")]
  public int bossHealth = 20;
  public float bossFireRate = 0.3f;

  [Header("Lateral Movement Settings")]
  public float moveAmplitude = 1.5f;
  public float moveSpeed = 2f;

  [Header("Fire Points")]
  public Transform firePoint1; // first shooting point
  public Transform firePoint2; // second shooting point

  private bool canMove = false;
  private Vector3 startPos;
  private float startTimeForMovement;


  void Start()
  {
    // Set health and fire rate
    maxHealth = bossHealth;
    currentHealth = maxHealth;

    fireRate = bossFireRate;

    // Initialize lateral movement like Level4
    startPos = transform.position;
    startTimeForMovement = Time.time;
  }

  protected override IEnumerator WaitThenShoot()
  {
    // Wait the usual fly-in delay
    yield return base.WaitThenShoot();

    // Enable lateral movement after fly-in
    canMove = true;
    startPos = transform.position;
    startTimeForMovement = Time.time;
  }

  protected override void Update()
  {
    base.Update();

    if (canMove)
    {
      float timeElapsed = Time.time - startTimeForMovement;
      float offset = Mathf.Sin(timeElapsed * moveSpeed) * moveAmplitude;

      // Tentative new X
      float newX = startPos.x + offset;

      // Clamp to camera bounds
      float padding = 0.5f; // optional, so enemy doesn't touch edges
      float camHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
      float minX = Camera.main.transform.position.x - camHalfWidth + padding;
      float maxX = Camera.main.transform.position.x + camHalfWidth - padding;
      newX = Mathf.Clamp(newX, minX, maxX);

      transform.position = new Vector3(newX, startPos.y, startPos.z);
    }
  }


  protected override void Shoot()
  {
    // Shoot from firePoint1
    if (firePoint1 != null)
    {
      GameObject bullet1 = Instantiate(bulletPrefab, firePoint1.position, firePoint1.rotation);
      bullet1.GetComponent<Rigidbody2D>().velocity = firePoint1.up * 5f;

    }

    // Shoot from firePoint2
    if (firePoint2 != null)
    {
      GameObject bullet2 = Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation);
      bullet2.GetComponent<Rigidbody2D>().velocity = firePoint1.up * 5f;
    }
  }

  // Override Die to skip shield drop and always spawn coin
  protected override void Die(bool droppedShield)
  {
    // Always spawn coin on death
    if (coinPrefab != null)
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

}
