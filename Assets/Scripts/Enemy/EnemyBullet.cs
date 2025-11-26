using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
  public GameObject hitEffectPlayer;

  [Header("Audio")]
  public AudioClip hitPlayerSFX;
  public float hitVolume = 1f;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Shield"))
    {
      // Shield absorbs the bullet
      Destroy(gameObject);
      return;
    }

    if (collision.transform.root.CompareTag("Player"))
    {
      // Damage player
      collision.GetComponentInParent<PlayerHealth>()?.TakeDamage(1);

      // Spawn sparks at impact
      if (SettingsManager.ArePlayerHitEffectsEnabled)
      {
        Instantiate(hitEffectPlayer, transform.position, Quaternion.identity);
      }

      // Play sound effect
      if (hitPlayerSFX != null)
        AudioSource.PlayClipAtPoint(hitPlayerSFX, transform.position, hitVolume);

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
