using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ShopManager : MonoBehaviour
{
  [Header("UI")]
  public TMP_Text coinsAmountText;
  public TMP_Text healthUpgradeValueText;
  public TMP_Text fireRateUpgradeValueText;
  public TMP_Text shieldDurationUpgradeValueText;
  public TMP_Text shipUpgradeDurationValueText;

  [Header("ProgressUI")]
  public UpgradeProgressUI healthProgressUI;
  public UpgradeProgressUI fireRateProgressUI;
  public UpgradeProgressUI shieldDurationProgressUI;
  public UpgradeProgressUI shipUpgradeDurationProgressUI;

  [Header("Upgrade Prices")]
  public int[] healthUpgradePrices = { 20, 50, 100, 150, 200 };
  public int[] fireRateUpgradePrices = { 20, 50, 100, 150, 200 };
  public int[] shieldDurationUpgradePrices = { 20, 50, 100, 150, 200 };
  public int[] shipUpgradeDurationPrices = { 20, 50, 100, 150, 200 };

  [Header("Messages")]
  public TMP_Text messageText;
  public float messageDuration = 1.5f;

  private void Start()
  {
    // Update the coin amount when the shop opens
    UpdateCoinsUI();

    // Update UpgradeProgressUI
    if (healthProgressUI != null)
      healthProgressUI.SetProgress(HealthUpgradeState.Level);
    if (fireRateProgressUI != null)
      fireRateProgressUI.SetProgress(FireRateUpgradeState.Level);
    if (shieldDurationProgressUI != null)
      shieldDurationProgressUI.SetProgress(ShieldDurationUpgradeState.Level);
    if (shipUpgradeDurationProgressUI != null)
      shipUpgradeDurationProgressUI.SetProgress(ShipUpgradeDurationState.Level);

    // Update healthUpgrade price UI
    UpdateHealthUpgradePriceUI();
    // Update fireRateUpgrade price UI
    UpdateFireRateUpgradePriceUI();
    // Update shield price UI
    UpdateShieldDurationUpgradePriceUI();
    // Update shipUpgrade price UI
    UpdateShipUpgradeDurationPriceUI();
  }

  public void BackToMenu()
  {
    SceneManager.LoadScene("MainMenu");
  }

  private IEnumerator ShowMessage(string message, Color color)
  {
    if (messageText == null) yield break;

    messageText.text = message;
    messageText.color = color;
    messageText.gameObject.SetActive(true);

    yield return new WaitForSeconds(messageDuration);

    messageText.gameObject.SetActive(false);
  }


  public void UpdateCoinsUI()
  {
    if (coinsAmountText != null && PersistentGameState.Instance != null)
    {
      coinsAmountText.text = PersistentGameState.Instance.Coins.ToString();
    }
  }

  private void UpdateHealthUpgradePriceUI()
  {
    int level = HealthUpgradeState.Level;

    if (level >= healthUpgradePrices.Length)
    {
      healthUpgradeValueText.text = "MAX";
      return;
    }

    healthUpgradeValueText.text = healthUpgradePrices[level].ToString();
  }

  public void BuyHealthUpgrade()
  {
    int level = HealthUpgradeState.Level;

    // Check max level
    if (level >= 5)
    {
      Debug.Log("Health already maxed!");
      UpdateHealthUpgradePriceUI();
      return;
    }

    int cost = healthUpgradePrices[level];

    // Try spending coins
    if (PersistentGameState.Instance.SpendCoins(cost))
    {
      HealthUpgradeState.Level++;   // Increase permanent level

      UpdateCoinsUI();
      UpdateHealthUpgradePriceUI();

      if (healthProgressUI != null)
        healthProgressUI.SetProgress(HealthUpgradeState.Level);

      StartCoroutine(ShowMessage("Purchase successful!", Color.green));
      Debug.Log("Health upgrade purchased! New level: " + HealthUpgradeState.Level);
    }
    else
    {
      StartCoroutine(ShowMessage("Not enough coins!", Color.red));
      Debug.Log("Not enough coins!");
    }
  }

  private void UpdateFireRateUpgradePriceUI()
  {
    int level = FireRateUpgradeState.Level;

    if (level >= fireRateUpgradePrices.Length)
    {
      fireRateUpgradeValueText.text = "MAX";
      return;
    }

    fireRateUpgradeValueText.text = fireRateUpgradePrices[level].ToString();
  }

  public void BuyFireRateUpgrade()
  {
    int level = FireRateUpgradeState.Level;

    // Max check
    if (level >= 5)
    {
      Debug.Log("Fire rate already maxed!");
      UpdateFireRateUpgradePriceUI();
      return;
    }

    int cost = fireRateUpgradePrices[level];

    // Spend coins
    if (PersistentGameState.Instance.SpendCoins(cost))
    {
      FireRateUpgradeState.Level++;   // Save permanently

      UpdateCoinsUI();
      UpdateFireRateUpgradePriceUI();

      if (fireRateProgressUI != null)
        fireRateProgressUI.SetProgress(FireRateUpgradeState.Level);

      StartCoroutine(ShowMessage("Purchase successful!", Color.green));
      Debug.Log("Fire Rate upgraded! Level: " + FireRateUpgradeState.Level);
    }
    else
    {
      StartCoroutine(ShowMessage("Not enough coins!", Color.red));
      Debug.Log("Not enough coins!");
    }
  }

  private void UpdateShieldDurationUpgradePriceUI()
  {
    int level = ShieldDurationUpgradeState.Level;

    if (level >= shieldDurationUpgradePrices.Length)
    {
      shieldDurationUpgradeValueText.text = "MAX";
      return;
    }

    shieldDurationUpgradeValueText.text = shieldDurationUpgradePrices[level].ToString();
  }

  public void BuyShieldDurationUpgrade()
  {
    int level = ShieldDurationUpgradeState.Level;

    if (level >= 5)
    {
      Debug.Log("Shield Duration already maxed!");
      UpdateShieldDurationUpgradePriceUI();
      return;
    }

    int cost = shieldDurationUpgradePrices[level];

    if (PersistentGameState.Instance.SpendCoins(cost))
    {
      ShieldDurationUpgradeState.Level++;

      UpdateCoinsUI();
      UpdateShieldDurationUpgradePriceUI();

      if (shieldDurationProgressUI != null)
        shieldDurationProgressUI.SetProgress(ShieldDurationUpgradeState.Level);

      StartCoroutine(ShowMessage("Purchase successful!", Color.green));
      Debug.Log("Shield Duration upgraded! Level: " + ShieldDurationUpgradeState.Level);
    }
    else
    {
      StartCoroutine(ShowMessage("Not enough coins!", Color.red));
      Debug.Log("Not enough coins!");
    }
  }

  private void UpdateShipUpgradeDurationPriceUI()
  {
    int level = ShipUpgradeDurationState.Level;

    if (level >= shipUpgradeDurationPrices.Length)
    {
      shipUpgradeDurationValueText.text = "MAX";
      return;
    }

    shipUpgradeDurationValueText.text = shipUpgradeDurationPrices[level].ToString();
  }

  public void BuyShipUpgradeDuration()
  {
    int level = ShipUpgradeDurationState.Level;

    if (level >= 5)
    {
      Debug.Log("Ship Upgrade Duration already maxed!");
      UpdateShipUpgradeDurationPriceUI();
      return;
    }

    int cost = shipUpgradeDurationPrices[level];

    if (PersistentGameState.Instance.SpendCoins(cost))
    {
      ShipUpgradeDurationState.Level++;

      UpdateCoinsUI();
      UpdateShipUpgradeDurationPriceUI();

      if (shipUpgradeDurationProgressUI != null)
        shipUpgradeDurationProgressUI.SetProgress(ShipUpgradeDurationState.Level);

      StartCoroutine(ShowMessage("Purchase successful!", Color.green));
      Debug.Log("Ship Upgrade Duration purchased! Level: " + ShipUpgradeDurationState.Level);
    }
    else
    {
      StartCoroutine(ShowMessage("Not enough coins!", Color.red));
      Debug.Log("Not enough coins!");
    }
  }
}
