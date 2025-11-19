using System.Collections;
using UnityEngine;

public class EnemyLevel3 : BasicEnemy
{
  [Header("Level3 Settings")]
  public int level3Health = 4;
  public float level3FireRate = 3f;

  [Header("Lateral Movement Settings")]
  public float moveAmplitude = 1.5f;
  public float moveSpeed = 2f;

  private bool canMove = false;
  private Vector3 startPos;
  private float startTimeForMovement;

  void Start()
  {
    maxHealth = level3Health;
    currentHealth = maxHealth;
    fireRate = level3FireRate;

    // Save start time if lateral movement is immediate
    startTimeForMovement = Time.time;
    startPos = transform.position;
  }

  // Override WaitThenShoot to also start lateral movement
  protected override IEnumerator WaitThenShoot()
  {
    // Wait the normal flyIn delay
    yield return base.WaitThenShoot();

    // Now start lateral movement
    canMove = true;
    startPos = transform.position;
    startTimeForMovement = Time.time;
  }

  protected override void Update()
  {
    base.Update();

    if (canMove)
    {
      float timeElapsed = Time.time - startTimeForMovement;
      float offset = Mathf.Sin(timeElapsed * moveSpeed) * moveAmplitude;

      // Tentative new X
      float newX = startPos.x + offset;

      // Clamp to camera bounds
      float padding = 0.5f; // optional, so enemy doesn't touch edges
      float camHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
      float minX = Camera.main.transform.position.x - camHalfWidth + padding;
      float maxX = Camera.main.transform.position.x + camHalfWidth - padding;
      newX = Mathf.Clamp(newX, minX, maxX);

      transform.position = new Vector3(newX, startPos.y, startPos.z);
    }
  }
}

