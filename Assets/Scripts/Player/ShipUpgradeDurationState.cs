using UnityEngine;

public class ShipUpgradeDurationState
{
  private const string KEY = "ShipUpgradeDurationLevel";

  public static int Level
  {
    get => PlayerPrefs.GetInt(KEY, 0); // 0–5
    set
    {
      PlayerPrefs.SetInt(KEY, Mathf.Clamp(value, 0, 5));
      PlayerPrefs.Save();
    }
  }
}

