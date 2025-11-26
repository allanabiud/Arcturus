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
    // Calculate targetY as top part of bottom half of screen
    Camera cam = Camera.main;
    float camBottom = cam.transform.position.y - cam.orthographicSize;
    float camTop = cam.transform.position.y + cam.orthographicSize;
    float camMiddle = (camTop + camBottom) / 2f;

    float startY = transform.position.y;
    float baseY = camBottom + (camMiddle - camBottom) * 0.95f;
    targetY = Mathf.Min(startY, FloatingPickupManager.GetNextYPosition(baseY));
  }

  private void Update()
  {
    if (!reachedTarget)
    {
      // Move downward toward targetY
      Vector3 pos = transform.position;
      pos.y = Mathf.MoveTowards(pos.y, targetY, moveSpeed * Time.deltaTime);
      transform.position = pos;

      if (Mathf.Abs(pos.y - targetY) < 0.05f) // slightly larger tolerance
        reachedTarget = true;
    }
    else
    {
      // Floating animation once target reached
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

