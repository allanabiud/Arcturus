using UnityEngine;

public class ShipUpgradePickupLv2 : MonoBehaviour
{
  public float floatSpeed = 1f;
  public float moveSpeed = 3f;
  public GameObject shipPrefab;

  private bool reachedTarget = false;
  private float targetY;

  [Header("Audio")]
  public AudioClip shipUpgradePickupSFX;
  public float shipUpgradeVolume = 1f;


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
      ShipUpgradeManager manager = collision.transform.root.GetComponent<ShipUpgradeManager>();
      if (manager != null && shipPrefab != null)
      {
        manager.UpgradeShip(shipPrefab);
        BasicEnemy.ShipUpgradeLv2ActiveInScene = false;
        FloatingPickupManager.ReleaseYPosition(targetY);
      }

      // Play coin pickup sound
      if (SettingsManager.IsSfxEnabled)
      {
        if (shipUpgradePickupSFX != null)
        {
          AudioSource.PlayClipAtPoint(shipUpgradePickupSFX, transform.position, shipUpgradeVolume);
        }
      }

      Destroy(gameObject);
    }
  }
}

