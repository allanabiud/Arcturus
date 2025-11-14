using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
  private float scrollSpeed = 1;
  private float backgroundHeight;

  // Start is called before the first frame update
  void Start()
  {
    backgroundHeight = GetComponent<BoxCollider2D>().size.y;

  }

  // Update is called once per frame
  void Update()
  {
    transform.Translate(Vector2.down * Time.deltaTime * scrollSpeed);
    if (transform.position.y <= -backgroundHeight)
    {
      transform.position += new Vector3(0f, 2 * backgroundHeight, 0f);
    }
  }
}
