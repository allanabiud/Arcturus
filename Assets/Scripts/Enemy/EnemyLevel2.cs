using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLevel2 : BasicEnemy
{
  [Header("Level2 Settings")]
  public int level2Health = 5;
  public float level2FireRate = 2.5f;

  void Start()
  {
    maxHealth = level2Health;
    currentHealth = maxHealth;
    fireRate = level2FireRate;
  }

  protected override void Shoot()
  {
    // Spawn Seeking Bullet
    Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
  }
}

