// PlayerBullet.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
  public GameObject hitEffectEnemy;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Enemy"))
    {
      BasicEnemy enemy = collision.GetComponent<BasicEnemy>();
      if (enemy != null)
        enemy.TakeDamage(1); // Enemy handles shield drop

      // Spawn hit effect only if Enemy Particles are enabled
      if (SettingsManager.AreEnemyParticlesEnabled)
      {
        Instantiate(hitEffectEnemy, transform.position, Quaternion.identity);
      }
      Destroy(gameObject);
    }
  }

  void Update()
  {
    if (transform.position.y > 8f)
      Destroy(gameObject);
  }
}

