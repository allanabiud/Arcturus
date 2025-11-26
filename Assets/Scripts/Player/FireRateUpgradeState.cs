using UnityEngine;

public class FireRateUpgradeState
{
  private const string KEY = "FireRateUpgradeLevel";

  public static int Level
  {
    get => PlayerPrefs.GetInt(KEY, 0);   // 0–5
    set
    {
      PlayerPrefs.SetInt(KEY, Mathf.Clamp(value, 0, 5));
      PlayerPrefs.Save();
    }
  }
}

