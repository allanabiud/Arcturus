using UnityEngine;
using System.Collections;

public class ShipUpgradeManager : MonoBehaviour
{
  [Header("Current Ship Instance in Scene")]
  public GameObject currentShip; // Assigned in scene at start (ShipLv1)

  [Header("Default Ship Prefab")]
  public GameObject defaultShipPrefab; // ShipLv1 prefab to revert to

  [Header("Upgrade Settings")]
  public float upgradeDuration = 10f; // Seconds for temporary upgrades

  private PlayerShooting shootingScript;
  private Coroutine upgradeRoutine;

  private float upgradeEndTime;

  void Awake()
  {
    shootingScript = GetComponent<PlayerShooting>();
    if (shootingScript == null)
      Debug.LogWarning("PlayerShooting not found on player root!");
  }

  public void UpgradeShip(GameObject newShipPrefab)
  {
    if (newShipPrefab == null) return;

    if (upgradeRoutine != null)
      StopCoroutine(upgradeRoutine);

    if (currentShip != null)
      Destroy(currentShip);

    GameObject newShip = Instantiate(newShipPrefab, transform);
    newShip.transform.localPosition = Vector3.zero;
    newShip.transform.localRotation = Quaternion.identity;
    currentShip = newShip;

    shootingScript?.RefreshFirePoints();
    GetComponent<PlayerController>()?.RefreshShipRenderer();

    // Start duration timer
    upgradeEndTime = Time.time + upgradeDuration;
    upgradeRoutine = StartCoroutine(UpgradeDurationTimer());
  }


  private IEnumerator UpgradeDurationTimer()
  {
    float timer = upgradeDuration;
    while (timer > 0f)
    {
      timer -= Time.deltaTime;
      yield return null;
    }

    // Revert back to default ship
    RevertToDefaultShip();
  }

  private void RevertToDefaultShip()
  {
    if (currentShip != null)
      Destroy(currentShip);

    if (defaultShipPrefab != null)
    {
      GameObject defaultShip = Instantiate(defaultShipPrefab, transform);
      defaultShip.transform.localPosition = Vector3.zero;
      defaultShip.transform.localRotation = Quaternion.identity;
      currentShip = defaultShip;

      shootingScript?.RefreshFirePoints();
      GetComponent<PlayerController>()?.RefreshShipRenderer();
    }
  }

  public float GetRemainingUpgradeTime()
  {
    return Mathf.Max(0f, upgradeEndTime - Time.time);
  }

  public bool IsUpgradeActive()
  {
    return Time.time < upgradeEndTime;
  }
}

