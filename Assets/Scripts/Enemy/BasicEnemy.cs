using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
  [Header("Shooting Settings")]
  public GameObject bulletPrefab;   // Bullet to shoot
  public Transform firePoint;       // Where the bullet spawns
  public float fireRate = 5f;       // Time between shots
  private float nextFireTime;
  public float shootingDelay = 0.5f; // Time to wait before shooting after fly-in

  [HideInInspector]
  public bool canShoot = false; // Only shoot when allowed

  // Called by EnemyFlyIn when the enemy reaches its target
  public void EnableShooting()
  {
    StartCoroutine(WaitThenShoot());
  }

  private IEnumerator WaitThenShoot()
  {
    yield return new WaitForSeconds(shootingDelay);
    canShoot = true;
    nextFireTime = Time.time; // Start shooting immediately after delay
  }


  void Update()
  {
    if (canShoot && Time.time >= nextFireTime)
    {
      Shoot();
      nextFireTime = Time.time + fireRate;
    }
  }

  void Shoot()
  {
    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    bullet.GetComponent<Rigidbody2D>().velocity = firePoint.up * 5f; // Bullet moves in firePoint's forward direction
  }
}

