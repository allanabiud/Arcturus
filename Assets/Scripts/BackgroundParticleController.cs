// BackgroundParticleController.cs
using UnityEngine;

public class BackgroundParticleController : MonoBehaviour
{
  private ParticleSystem particleSystemComponent;

  void Awake()
  {
    // Assuming the ParticleSystem is a direct child or on the same GameObject
    particleSystemComponent = GetComponentInChildren<ParticleSystem>();

    // Apply the initial state loaded from PlayerPrefs/SettingsManager
    ApplyParticleState(SettingsManager.AreBackgroundParticlesEnabled);
  }

  // Listen for scene loaded events if needed, but since the background
  // prefab is in every scene, Awake is generally fine.

  public void ApplyParticleState(bool enabled)
  {
    if (particleSystemComponent == null) return;

    if (enabled)
    {
      // Set the system to run normally
      if (!particleSystemComponent.isPlaying)
      {
        particleSystemComponent.Play();
      }
    }
    else
    {
      // Stop and clear the particles
      if (particleSystemComponent.isPlaying)
      {
        particleSystemComponent.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
      }
    }
  }
}
