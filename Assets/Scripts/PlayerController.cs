using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
  public float moveSpeed = 10f;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    HandleTouchMovement();
  }

  void HandleTouchMovement()
  {
    if (Input.touchCount > 0)
    {
      Touch touch = Input.GetTouch(0);

      // Much more accurate UI detection
      if (IsTouchOverUI(touch))
        return;

      Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
      touchPos.z = 0;
      transform.position = Vector3.Lerp(transform.position, touchPos, Time.deltaTime * moveSpeed);

    }
  }

  bool IsTouchOverUI(Touch touch)
  {
    PointerEventData ped = new PointerEventData(EventSystem.current);
    ped.position = touch.position;

    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(ped, results);

    return results.Count > 0;
  }
}
