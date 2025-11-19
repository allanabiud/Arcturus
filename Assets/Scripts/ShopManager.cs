using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ShopManager : MonoBehaviour
{
  [Header("UI")]
  public TMP_Text coinsAmountText;

  private void Start()
  {
    // Update the coin amount when the shop opens
    UpdateCoinsUI();
  }

  public void BackToMenu()
  {
    SceneManager.LoadScene("MainMenu");
  }

  public void UpdateCoinsUI()
  {
    if (coinsAmountText != null && PersistentGameState.Instance != null)
    {
      coinsAmountText.text = PersistentGameState.Instance.Coins.ToString();
    }
  }

  // TODO: Spend coins
  public void BuyItem(int cost)
  {
    if (PersistentGameState.Instance.SpendCoins(cost))
    {
      Debug.Log("Purchase successful!");
      UpdateCoinsUI();
    }
    else
    {
      Debug.Log("Not enough coins!");
    }
  }
}
