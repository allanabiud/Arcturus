using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{
  public GameObject shieldEffect;
  private Coroutine shieldRoutine;

  public float baseDuration = 5f;
  private float shieldEndTime;

  [Header("Audio")]
  public AudioClip shieldUpSFX;
  public AudioClip shieldDownSFX;
  public float shieldVolume = 1f;

  // Add upgrade later: PersistentGameState.Instance.ShieldDurationLevel
  public float GetShieldDuration()
  {
    int upgradeLevel = ShieldDurationUpgradeState.Level;
    return baseDuration + (upgradeLevel * 2.5f); // Each level adds 2.5 sec
  }

  public void ActivateShield()
  {
    if (shieldRoutine != null)
      StopCoroutine(shieldRoutine);

    float duration = GetShieldDuration();
    shieldEndTime = Time.time + duration; // Set end time for UI
    shieldRoutine = StartCoroutine(ShieldDurationRoutine());

    // Play shield activate sound
    if (shieldUpSFX != null)
      AudioSource.PlayClipAtPoint(shieldUpSFX, transform.position, shieldVolume);
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

    // Play shield deactivate sound
    if (shieldDownSFX != null)
      AudioSource.PlayClipAtPoint(shieldDownSFX, transform.position, shieldVolume);

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

