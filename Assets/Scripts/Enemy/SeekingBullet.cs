using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingBullet : MonoBehaviour
{
  [Header("Homing Settings")]
  public float speed = 4f;          // How fast the bullet moves
  public float rotateSpeed = 50f;  // Degrees per second

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
      Vector2 direction = (player.position - transform.position).normalized;

      float rotateStep = rotateSpeed * Time.deltaTime;
      float currentAngle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
      float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotateStep);
      Vector2 newDir = new Vector2(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad));
      rb.velocity = newDir * speed;
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
}

