using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
  public GameObject hitEffectPlayer;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Player"))
    {
      // Damage player
      collision.GetComponent<PlayerHealth>()?.TakeDamage(1);

      // Spawn sparks at impact
      Instantiate(hitEffectPlayer, transform.position, Quaternion.identity);

      // Vibrate device (mild vibration)
#if UNITY_ANDROID || UNITY_IOS
      Handheld.Vibrate();
#endif

      Destroy(gameObject);
    }
  }

  // Update is called once per frame
  void Update()
  {

    if (transform.position.y < -8f)
    {
      Destroy(gameObject);
    }

  }
}
