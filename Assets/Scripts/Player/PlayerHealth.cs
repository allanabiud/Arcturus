using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
  [Header("Health Settings")]
  public int baseHealth = 2;      // Base health
  public int maxHealth;           // After upgrades
  public int currentHealth;
  public int upgradePerLevel = 2;

  [Header("UI Settings")]
  public Transform healthUIParent;          // Parent for health icons (bottom-left anchor)
  public GameObject fullHealthIconPrefab;   // Full health icon
  public GameObject damagedHealthIconPrefab; // Damaged health icon
  public float iconSpacing = 50f;
  public float xOffset = 20f;
  public float yOffset = 10f;
  private List<Image> healthIcons = new List<Image>();

  void Start()
  {
    int savedLevel = HealthUpgradeState.Level;

    maxHealth = baseHealth + (savedLevel * upgradePerLevel);
    maxHealth = Mathf.Min(maxHealth, 10);
    currentHealth = maxHealth;

    SetupHealthUI();
  }

  // Initialize health icons stacked vertically
  private void SetupHealthUI()
  {
    foreach (Transform child in healthUIParent)
      Destroy(child.gameObject);

    healthIcons.Clear();

    UpdateHealthUI();
  }

  public void TakeDamage(int amount)
  {
    currentHealth -= amount;

    UpdateHealthUI();

    if (currentHealth <= 0)
    {
      Die();
    }
  }

  private void UpdateHealthUI()
  {
    int hitsPerIcon = baseHealth;

    // Ensure enough icons for max health
    int requiredIcons = Mathf.CeilToInt((float)maxHealth / hitsPerIcon);
    while (healthIcons.Count < requiredIcons)
    {
      GameObject iconGO = Instantiate(fullHealthIconPrefab, healthUIParent);
      iconGO.transform.localPosition = new Vector3(xOffset, yOffset + healthIcons.Count * iconSpacing, 0);
      healthIcons.Add(iconGO.GetComponent<Image>());
    }

    int remainingHealth = currentHealth;

    for (int i = 0; i < healthIcons.Count; i++)
    {
      if (remainingHealth >= hitsPerIcon)
      {
        healthIcons[i].sprite = fullHealthIconPrefab.GetComponent<Image>().sprite;
        healthIcons[i].gameObject.SetActive(true);
      }
      else if (remainingHealth > 0)
      {
        // partially damaged icon
        healthIcons[i].sprite = damagedHealthIconPrefab.GetComponent<Image>().sprite;
        healthIcons[i].gameObject.SetActive(true);
      }
      else
      {
        // no health left for this icon → hide it
        healthIcons[i].gameObject.SetActive(false);
      }

      remainingHealth -= hitsPerIcon;
    }
  }


  private void Die()
  {
    // Remove all health icons
    foreach (Transform child in healthUIParent)
      Destroy(child.gameObject);

    healthIcons.Clear();

    // Destroy player
    Destroy(gameObject);

    // Trigger Game Over
    SceneUIManager.Instance.GameOver();
  }

  // Optional: add more health via upgrades
  // public void IncreaseMaxHealth(int amount)
  // {
  //   maxHealth += amount;
  //   currentHealth = maxHealth;
  //   SetupHealthUI();
  // }
}
