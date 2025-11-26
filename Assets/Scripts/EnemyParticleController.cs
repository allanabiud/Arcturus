// EnemyParticleController.cs
using UnityEngine;

public class EnemyParticleController : MonoBehaviour
{
  // Find all particle systems attached to the enemy ship
  private ParticleSystem[] persistentParticles;

  void Awake()
  {
    // Get all ParticleSystem components on this GameObject and its children
    persistentParticles = GetComponentsInChildren<ParticleSystem>();

    // Apply initial state
    ApplyParticleState(SettingsManager.AreEnemyEngineTrailsEnabled);
  }

  public void ApplyParticleState(bool enabled)
  {
    foreach (ParticleSystem ps in persistentParticles)
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
        // Stop and clear the particles
        if (ps.isPlaying)
        {
          ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
      }
    }
  }
}
