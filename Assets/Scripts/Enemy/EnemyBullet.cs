using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
  // Update is called once per frame
  void Update()
  {

    if (transform.position.y < -8f)
    {
      Destroy(gameObject);
    }

  }
}
