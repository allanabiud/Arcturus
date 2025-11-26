using UnityEngine;

public class ShieldDurationUpgradeState
{
  private const string KEY = "ShieldDurationLevel";

  public static int Level
  {
    get => PlayerPrefs.GetInt(KEY, 0);   // 0–5 levels
    set
    {
      PlayerPrefs.SetInt(KEY, Mathf.Clamp(value, 0, 5));
      PlayerPrefs.Save();
    }
  }
}

