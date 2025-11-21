using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{
  public GameObject shieldEffect;
  private Coroutine shieldRoutine;

  public float baseDuration = 5f;
  private float shieldEndTime;

  // Add upgrade later: PersistentGameState.Instance.ShieldDurationLevel
  public float GetShieldDuration()
  {
    // int upgradeLevel = PersistentGameState.Instance.ShieldDurationLevel;
    // return baseDuration + (upgradeLevel * 1.5f); // Example: +1.5 sec per level
    return baseDuration;
  }

  public void ActivateShield()
  {
    if (shieldRoutine != null)
      StopCoroutine(shieldRoutine);

    float duration = GetShieldDuration();
    shieldEndTime = Time.time + duration; // Set end time for UI
    shieldRoutine = StartCoroutine(ShieldDurationRoutine());
  }

  private IEnumerator ShieldDurationRoutine()
  {
    shieldEffect.SetActive(true);

    float duration = GetShieldDuration();
    float timer = duration;

    while (timer > 0)
    {
      timer -= Time.deltaTime;
      yield return null;
    }

    shieldEffect.SetActive(false);
  }

  public bool IsShieldActive()
  {
    return shieldEffect.activeSelf;
  }

  public float GetRemainingShieldTime()
  {
    if (!IsShieldActive()) return 0f;
    return Mathf.Max(0f, shieldEndTime - Time.time);
  }
}

