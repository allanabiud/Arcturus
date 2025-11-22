using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipUpgradeUI : MonoBehaviour
{
  public Image upgradeIcon;
  public TextMeshProUGUI countdownText;

  private ShipUpgradeManager upgradeManager;

  void Start()
  {
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    if (player != null)
      upgradeManager = player.GetComponent<ShipUpgradeManager>();

    upgradeIcon.enabled = false;
    countdownText.enabled = false;
  }

  void Update()
  {
    if (upgradeManager == null) return;

    if (upgradeManager.IsUpgradeActive())
    {
      upgradeIcon.enabled = true;
      countdownText.enabled = true;
      countdownText.text = upgradeManager.GetRemainingUpgradeTime().ToString("F1") + "s";
    }
    else
    {
      upgradeIcon.enabled = false;
      countdownText.enabled = false;
    }
  }
}

