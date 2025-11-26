// PlayerParticleController.cs
using UnityEngine;

public class PlayerParticleController : MonoBehaviour
{
  // Find all particle systems attached to the player ship
  private ParticleSystem[] engineTrails;

  void Awake()
  {
    // Get all ParticleSystem components on this GameObject and its children
    engineTrails = GetComponentsInChildren<ParticleSystem>();

    // Apply initial state
    ApplyParticleState(SettingsManager.ArePlayerEngineTrailsEnabled);
  }

  public void ApplyParticleState(bool enabled)
  {
    foreach (ParticleSystem ps in engineTrails)
    {
      if (ps == null) continue;

      if (enabled)
      {
        if (!ps.isPlaying)
        {
          ps.Play();
        }
      }
      else
      {
        // Stop and clear the trails
        if (ps.isPlaying)
        {
          ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
      }
    }
  }
}
