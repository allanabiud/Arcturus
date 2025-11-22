using UnityEngine;

public class ShipUpgradePickupLv3 : MonoBehaviour
{
  public float floatSpeed = 1f;
  public float moveSpeed = 3f;
  public GameObject shipPrefab;

  private bool reachedTarget = false;
  private float targetY;

  private void Start()
  {
    Camera cam = Camera.main;
    float camBottom = cam.transform.position.y - cam.orthographicSize;
    float camTop = cam.transform.position.y + cam.orthographicSize;
    float camMiddle = (camTop + camBottom) / 2f;

    float baseY = camBottom + (camMiddle - camBottom) * 0.95f;
    targetY = FloatingPickupManager.GetNextYPosition(baseY);
  }

  private void Update()
  {
    if (!reachedTarget)
    {
      Vector3 pos = transform.position;
      pos.y = Mathf.MoveTowards(pos.y, targetY, moveSpeed * Time.deltaTime);
      transform.position = pos;

      if (Mathf.Abs(pos.y - targetY) < 0.01f)
        reachedTarget = true;
    }
    else
    {
      Vector3 floatPos = transform.position;
      floatPos.y += Mathf.Sin(Time.time * floatSpeed) * 0.002f;
      transform.position = floatPos;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.transform.root.CompareTag("Player"))
    {
      ShipUpgradeManager manager = collision.transform.root.GetComponent<ShipUpgradeManager>();
      if (manager != null && shipPrefab != null)
      {
        manager.UpgradeShip(shipPrefab);
        BasicEnemy.ShipUpgradeLv3ActiveInScene = false;
        FloatingPickupManager.ReleaseYPosition(targetY);
      }

      Destroy(gameObject);
    }
  }
}

