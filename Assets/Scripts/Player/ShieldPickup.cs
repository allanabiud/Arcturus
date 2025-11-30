using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : MonoBehaviour
{
  public float floatSpeed = 1f;   // Floating speed
  public float moveSpeed = 3f;    // Speed to move to float zone
  private bool reachedTarget = false;
  private float targetY;

  [Header("Audio")]
  public AudioClip shieldPickupSFX;
  public float shieldVolume = 1f;

  private void Start()
  {
    Camera cam = Camera.main;
    // Calculate screen boundaries
    float camBottom = cam.transform.position.y - cam.orthographicSize;
    float camMiddle = cam.transform.position.y;
    float desiredBaseY = camBottom + (cam.orthographicSize * 0.65f);

    // Get the final Y position using the manager to prevent overlaps
    targetY = FloatingPickupManager.GetNextYPosition(desiredBaseY);

    targetY = Mathf.Min(transform.position.y, targetY);
  }

  private void Update()
  {
    // Check if the pickup is significantly above its target, indicating it just spawned
    if (transform.position.y > targetY + 1f && !reachedTarget)
    {
      // Add a strong initial push downwards to clear the enemy wreckage zone.
      transform.Translate(Vector3.down * (moveSpeed * 1.5f) * Time.deltaTime, Space.World);
    }
    else if (!reachedTarget)
    {
      // Controlled MoveTowards logic
      Vector3 pos = transform.position;
      pos.y = Mathf.MoveTowards(pos.y, targetY, moveSpeed * Time.deltaTime);
      transform.position = pos;

      if (Mathf.Abs(pos.y - targetY) < 0.05f) // slightly larger tolerance
        reachedTarget = true;
    }
    else
    {
      // Floating animation once target reached (unchanged)
      Vector3 floatPos = transform.position;
      floatPos.y += Mathf.Sin(Time.time * floatSpeed) * 0.002f;
      transform.position = floatPos;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.transform.root.CompareTag("Player"))
    {
      ShieldManager shield = collision.transform.root.GetComponent<ShieldManager>();
      if (shield != null)
      {
        shield.ActivateShield();
      }
      BasicEnemy.ShieldActiveInScene = false; // allow next shield to spawn
      FloatingPickupManager.ReleaseYPosition(targetY);

      // Play coin pickup sound
      if (SettingsManager.IsSfxEnabled)
      {
        if (shieldPickupSFX != null)
        {

          AudioSource.PlayClipAtPoint(shieldPickupSFX, transform.position, shieldVolume);
        }
      }

      Destroy(gameObject);
    }
  }
}

