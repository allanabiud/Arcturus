using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : BasicEnemy
{
  [Header("Boss Settings")]
  public int bossHealth = 50;
  public float bossFireRate = 0.2f;

  [Header("Lateral Movement Settings")]
  public float moveAmplitude = 4f;
  public float moveSpeed = 2.5f;

  [Header("Fire Points")]
  public Transform firePoint1; // first shooting point
  public Transform firePoint2; // second shooting point

  private bool canMove = false;
  private Vector3 startPos;
  private float startTimeForMovement;
  private float phaseOffset = 0f;

  void Start()
  {
    // Set health and fire rate
    maxHealth = bossHealth;
    currentHealth = maxHealth;

    fireRate = bossFireRate;

    // Initialize lateral movement
    startPos = transform.position;
    startTimeForMovement = Time.time;

    // Force the fly-in target Y to the top of the screen ---
    // Get the fly-in component
    EnemyFlyIn flyIn = GetComponent<EnemyFlyIn>();

    if (flyIn != null)
    {
      Camera cam = Camera.main;
      float camTop = cam.transform.position.y + cam.orthographicSize;
      float camBottom = cam.transform.position.y - cam.orthographicSize;

      float targetYRatio = 0.85f; // 85% up from the bottom edge
      float topScreenY = cam.transform.position.y + cam.orthographicSize;

      // Calculate the desired final resting Y position
      float desiredBossY = camBottom + (2 * cam.orthographicSize * targetYRatio);

      Vector3 newTarget = flyIn.targetPosition;
      newTarget.y = desiredBossY;

      flyIn.targetPosition = newTarget;

      startPos.y = desiredBossY;
    }
  }

  protected override IEnumerator WaitThenShoot()
  {
    // Wait the usual fly-in delay
    yield return base.WaitThenShoot();

    // Enable lateral movement after fly-in
    canMove = true;

    startPos = transform.position;
    CalculatePhaseOffset();

    startTimeForMovement = Time.time;
  }

  private void CalculatePhaseOffset()
  {
    float normalizedX = Mathf.Clamp((startPos.x - Camera.main.transform.position.x) / moveAmplitude, -1f, 1f);

    phaseOffset = Mathf.Asin(normalizedX);
  }

  protected override void Update()
  {
    base.Update();

    if (canMove)
    {
      float timeElapsed = Time.time - startTimeForMovement;

      float offset = Mathf.Sin(timeElapsed * moveSpeed + phaseOffset) * moveAmplitude;

      float newX = Camera.main.transform.position.x + offset;

      // Clamp to camera bounds
      float padding = 0.5f;
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
    }

    // Shoot from firePoint2
    if (firePoint2 != null)
    {
      GameObject bullet2 = Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation);
    }

    // Play shooting sound manually
    if (SettingsManager.IsSfxEnabled && audioSource != null && shootSFX != null)
      audioSource.PlayOneShot(shootSFX, 0.3f);
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
