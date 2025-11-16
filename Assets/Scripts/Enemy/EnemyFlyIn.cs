using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyIn : MonoBehaviour
{
  public Vector3 targetPosition;   // Where the enemy should stop
  public float speed = 3f;         // Fly-in speed

  private bool moving = true;

  void Update()
  {
    if (moving)
    {
      transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

      if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
      {
        transform.position = targetPosition;
        moving = false; // Stop moving once reached

        // Enable shooting when fly-in finishes
        BasicEnemy enemy = GetComponent<BasicEnemy>();
        if (enemy != null)
          enemy.EnableShooting();
      }
    }
  }
}
