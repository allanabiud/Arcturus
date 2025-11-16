using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
  public GameObject bulletPrefab;
  public Transform firePoint;

  [Header("Fire Rate Settings")]
  public float baseFireRate = 0.6f;     // Starting slow rate
  public int upgradeLevel = 0;          // Current upgrade level
  public int maxUpgradeLevel = 5;       // Maximum upgrade level
  public float upgradeBonusPerLevel = 0.1f; // How much faster each upgrade makes you

  public float bulletSpeed = 15f;

  private float nextFireTime = 0f;

  void Update()
  {
    // Stop shooting while countdown is active
    if (!GameState.canPlayerShoot)
      return;

    AutoShoot();
  }

  float GetCurrentFireRate()
  {
    int clampedLevel = Mathf.Clamp(upgradeLevel, 0, maxUpgradeLevel);

    // Fire rate decreases as upgrades increase (faster shooting)
    float upgradedRate = baseFireRate - (clampedLevel * upgradeBonusPerLevel);

    // Prevent fire rate from ever going below 0.1 seconds
    return Mathf.Clamp(upgradedRate, 0.1f, baseFireRate);
  }

  void AutoShoot()
  {
    float currentRate = GetCurrentFireRate();

    if (Time.time >= nextFireTime)
    {
      Shoot();
      nextFireTime = Time.time + currentRate;
    }
  }

  void Shoot()
  {
    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
    rb.velocity = Vector2.up * bulletSpeed;
  }
}
