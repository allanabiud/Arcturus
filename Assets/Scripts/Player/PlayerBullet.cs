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
      // Damage enemy
      BasicEnemy enemy = collision.GetComponent<BasicEnemy>();
      if (enemy != null)
      {
        enemy.TakeDamage(1); // damage value
      }

      // Spawn particles on impact
      Instantiate(hitEffectEnemy, transform.position, Quaternion.identity);

      Destroy(gameObject);
    }
  }

  // Update is called once per frame
  void Update()
  {
    {
      if (transform.position.y > 8f)
        Destroy(gameObject);
    }
  }
}
