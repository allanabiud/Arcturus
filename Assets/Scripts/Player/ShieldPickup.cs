using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : MonoBehaviour
{
  public float floatSpeed = 1f;   // Floating speed
  public float moveSpeed = 3f;    // Speed to move to float zone
  private bool reachedTarget = false;
  private float targetY;

  private void Start()
  {
    // Calculate targetY as top part of bottom half of screen
    Camera cam = Camera.main;
    float camBottom = cam.transform.position.y - cam.orthographicSize;
    float camTop = cam.transform.position.y + cam.orthographicSize;
    float camMiddle = (camTop + camBottom) / 2f;

    // Top of bottom half = midpoint between camBottom and camMiddle
    targetY = camBottom + (camMiddle - camBottom) * 0.6f;
  }

  private void Update()
  {
    if (!reachedTarget)
    {
      // Move downward toward targetY
      Vector3 pos = transform.position;
      pos.y = Mathf.MoveTowards(pos.y, targetY, moveSpeed * Time.deltaTime);
      transform.position = pos;

      if (Mathf.Abs(pos.y - targetY) < 0.01f)
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
    if (collision.CompareTag("Player"))
    {
      ShieldManager shield = collision.GetComponent<ShieldManager>();
      if (shield != null)
      {
        shield.ActivateShield();
      }
      BasicEnemy.ShieldActiveInScene = false; // allow next shield to spawn
      Destroy(gameObject);

    }
  }
}

