using UnityEngine;
using UnityEngine.UI;

public class UpgradeProgressUI : MonoBehaviour
{
  [Header("Level Icons (5 Images)")]
  public Image[] levelIcons;     // Assign Lv1–Lv5 in Inspector

  [Header("Sprites")]
  public Sprite filledSprite;    // Filled circle
  public Sprite emptySprite;     // Empty circle

  public void SetProgress(int upgradeLevel)
  {
    // Make sure the level does not go out of range
    upgradeLevel = Mathf.Clamp(upgradeLevel, 0, levelIcons.Length - 1);

    for (int i = 0; i < levelIcons.Length; i++)
    {
      if (i < upgradeLevel)
        levelIcons[i].sprite = filledSprite;   // Fill this level
      else
        levelIcons[i].sprite = emptySprite;    // Keep empty
    }
  }
}

