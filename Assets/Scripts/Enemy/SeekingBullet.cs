using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingBullet : MonoBehaviour
{
  [Header("Homing Settings")]
  public float speed = 3.5f;          // How fast the bullet moves
  public float rotateSpeed = 60f;  // Degrees per second

  private Transform player;
  private Rigidbody2D rb;

  [Header("Effects")]
  public GameObject hitEffectPlayer;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    player = GameObject.FindGameObjectWithTag("Player")?.transform;

    // Initial forward movement
    rb.velocity = transform.up * speed;
  }

  void Update()
  {
    if (player != null)
    {
      // Direction from bullet to player
      Vector2 direction = (player.position - transform.position).normalized;

      // Current velocity direction
      Vector2 currentDir = rb.velocity.normalized;

      // Calculate the max rotation step (radians)
      float rotateStep = rotateSpeed * Mathf.Deg2Rad * Time.deltaTime;

      // Smoothly rotate current direction toward target
      Vector2 newDir = Vector2.Lerp(currentDir, direction, rotateStep).normalized;

      // Apply new velocity
      rb.velocity = newDir * speed;

      // Optional: make bullet face direction
      float angle = Mathf.Atan2(newDir.y, newDir.x) * Mathf.Rad2Deg - 90f; // adjust depending on sprite orientation
      transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // Destroy if outside camera bounds
    Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
    if (viewportPos.x < 0f || viewportPos.x > 1f || viewportPos.y < 0f || viewportPos.y > 1f)
    {
      Destroy(gameObject);
    }
  }


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
      Instantiate(hitEffectPlayer, transform.position, Quaternion.identity);

      // Vibrate device (mild vibration)
#if UNITY_ANDROID || UNITY_IOS
      Handheld.Vibrate();
#endif

      Destroy(gameObject);
    }
  }
}

