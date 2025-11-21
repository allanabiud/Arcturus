using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShieldUI : MonoBehaviour
{
  public Image shieldIcon; // Image in top-left
  public TextMeshProUGUI countdownText;

  private ShieldManager shieldManager;

  private void Start()
  {
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    if (player != null)
      shieldManager = player.GetComponent<ShieldManager>();

    shieldIcon.enabled = false;
    countdownText.enabled = false;
  }

  private void Update()
  {
    if (shieldManager == null) return;

    if (shieldManager.IsShieldActive())
    {
      shieldIcon.enabled = true;
      countdownText.enabled = true;

      float remaining = shieldManager.GetRemainingShieldTime();
      countdownText.text = remaining.ToString("F1") + "s";
    }
    else
    {
      shieldIcon.enabled = false;
      countdownText.enabled = false;
    }
  }
}

