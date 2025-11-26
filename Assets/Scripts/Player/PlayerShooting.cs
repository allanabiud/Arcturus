using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
  [Header("Shooting Settings")]
  public GameObject bulletPrefab;
  public float baseFireRate = 0.6f;
  public int upgradeLevel => FireRateUpgradeState.Level;
  public int maxUpgradeLevel = 5;
  public float upgradeBonusPerLevel = 0.1f;
  public float bulletSpeed = 15f;
  private float nextFireTime = 0f;
  private Transform[] firePoints;

  [Header("Audio")]
  public AudioSource audioSource;
  public AudioClip shootSFX;

  void Start()
  {
    RefreshFirePoints();
  }

  void Update()
  {
    if (!SceneUIManager.canPlayerShoot) return;

    if (Time.time >= nextFireTime)
    {
      Shoot();
      nextFireTime = Time.time + GetCurrentFireRate();
    }
  }

  float GetCurrentFireRate()
  {
    int clampedLevel = Mathf.Clamp(FireRateUpgradeState.Level, 0, maxUpgradeLevel);
    float upgradedRate = baseFireRate - (clampedLevel * upgradeBonusPerLevel);
    return Mathf.Clamp(upgradedRate, 0.1f, baseFireRate);
  }

  void Shoot()
  {
    if (firePoints == null || firePoints.Length == 0) return;

    foreach (Transform fp in firePoints)
    {
      if (fp == null) continue; // Safety check
      GameObject bullet = Instantiate(bulletPrefab, fp.position, fp.rotation);
      Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
      if (rb != null) rb.velocity = fp.up * bulletSpeed;
    }

    // Play shooting sound if enabled
    if (SettingsManager.IsSfxEnabled && audioSource != null && shootSFX != null)
    {
      audioSource.PlayOneShot(shootSFX);
    }
  }

  public void RefreshFirePoints()
  {
    List<Transform> fpList = new List<Transform>();
    foreach (Transform t in GetComponentsInChildren<Transform>())
    {
      if (t.CompareTag("FirePoint"))
        fpList.Add(t);
    }
    firePoints = fpList.ToArray();
  }
}

