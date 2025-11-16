using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
  public float moveSpeed = 16f;

  private bool isDragging = false;
  private Vector3 offset;

  // Update is called once per frame
  void Update()
  {
    HandleTouchMovement();
    HandleMouseMovement();
  }

  void HandleTouchMovement()
  {
    if (Input.touchCount == 0)
      return;

    Touch touch = Input.GetTouch(0);

    if (IsTouchOverUI(touch))
      return;

    Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
    touchPos.z = 0;

    switch (touch.phase)
    {
      case TouchPhase.Began:
        isDragging = true;
        offset = transform.position - touchPos;
        break;

      case TouchPhase.Moved:
        if (isDragging)
        {
          Vector3 targetPos = touchPos + offset;
          targetPos = ClampToScreenBounds(targetPos);

          transform.position = Vector3.Lerp(
              transform.position,
              targetPos,
              moveSpeed * Time.deltaTime
          );
        }
        break;

      case TouchPhase.Ended:
      case TouchPhase.Canceled:
        isDragging = false;
        break;
    }
  }

  void HandleMouseMovement()
  {
    // Only run if playing in editor or standalone build
    if (Application.isMobilePlatform)
      return;

    if (IsMouseOverUI())
      return;

    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePos.z = 0;

    if (Input.GetMouseButtonDown(0))
    {
      isDragging = true;
      offset = transform.position - mousePos;
    }
    else if (Input.GetMouseButton(0) && isDragging)
    {
      Vector3 targetPos = mousePos + offset;
      targetPos = ClampToScreenBounds(targetPos);

      transform.position = Vector3.Lerp(
          transform.position,
          targetPos,
          moveSpeed * Time.deltaTime
      );
    }
    else if (Input.GetMouseButtonUp(0))
    {
      isDragging = false;
    }
  }

  Vector3 ClampToScreenBounds(Vector3 pos)
  {
    Camera cam = Camera.main;

    // Convert screen edges to world space boundaries
    Vector3 screenBottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, 0));
    Vector3 screenTopRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

    // Get ship size (half width & height)
    float halfWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2f;
    float halfHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2f;

    // Clamp inside camera view
    pos.x = Mathf.Clamp(pos.x, screenBottomLeft.x + halfWidth, screenTopRight.x - halfWidth);
    pos.y = Mathf.Clamp(pos.y, screenBottomLeft.y + halfHeight, screenTopRight.y - halfHeight);

    return pos;
  }

  bool IsTouchOverUI(Touch touch)
  {
    PointerEventData ped = new PointerEventData(EventSystem.current);
    ped.position = touch.position;

    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(ped, results);

    return results.Count > 0;
  }

  bool IsMouseOverUI()
  {
    PointerEventData ped = new PointerEventData(EventSystem.current);
    ped.position = Input.mousePosition;

    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(ped, results);

    return results.Count > 0;
  }
}
