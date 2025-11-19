using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLevel4 : BasicEnemy
{
  [Header("Level4 Settings")]
  public int level4Health = 6;
  public float level4FireRate = 2.5f;


  [Header("Lateral Movement Settings")]
  public float moveAmplitude = 1.5f;
  public float moveSpeed = 2f;

  private bool canMove = false;
  private Vector3 startPos;
  private float startTimeForMovement;


  void Start()
  {
    // Set health
    maxHealth = level4Health;
    currentHealth = maxHealth;
    fireRate = level4FireRate;

    // Save start time if lateral movement is immediate
    startPos = transform.position;
    startTimeForMovement = Time.time;
  }

  // WaitThenShoot handles enabling shooting and movement after fly-in
  protected override IEnumerator WaitThenShoot()
  {
    // Wait base fly-in shooting delay
    yield return base.WaitThenShoot();

    // Enable lateral movement
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
    // Spawn homing bullet prefab (like Level 2)
    Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
  }
}
