using UnityEngine;

public class HealthUpgradeState
{
  private const string KEY = "HealthUpgradeLevel";

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

