using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
  public Toggle musicToggle;
  public Toggle sfxToggle;
  public Toggle backgroundParticlesToggle;
  public Toggle playerEngineTrailToggle;
  public Toggle playerHitEffectToggle;
  public Toggle enemyEngineTrailToggle;
  public Toggle enemyHitEffectToggle;

  private bool currentMusicState;
  private bool currentSfxState;
  private bool currentBackgroundParticlesState;
  private bool currentPlayerEngineTrailState;
  private bool currentPlayerHitEffectState;
  private bool currentEnemyEngineTrailState;
  private bool currentEnemyHitEffectState;

  public static bool IsSfxEnabled => PlayerPrefs.GetInt("SFX", 1) == 1;
  public static bool AreBackgroundParticlesEnabled => PlayerPrefs.GetInt("Particles", 1) == 1;
  public static bool ArePlayerEngineTrailsEnabled => PlayerPrefs.GetInt("EngineTrails", 1) == 1;
  public static bool ArePlayerHitEffectsEnabled => PlayerPrefs.GetInt("PlayerHitEffects", 1) == 1;
  public static bool AreEnemyEngineTrailsEnabled => PlayerPrefs.GetInt("EnemyEngineTrails", 1) == 1;
  public static bool AreEnemyHitEffectsEnabled => PlayerPrefs.GetInt("EnemyHitEffects", 1) == 1;

  private void Start()
  {
    currentMusicState = PlayerPrefs.GetInt("Music", 1) == 1;
    currentSfxState = PlayerPrefs.GetInt("SFX", 1) == 1;
    currentBackgroundParticlesState = PlayerPrefs.GetInt("Particles", 1) == 1;
    currentPlayerEngineTrailState = PlayerPrefs.GetInt("EngineTrails", 1) == 1;
    currentPlayerHitEffectState = PlayerPrefs.GetInt("PlayerHitEffects", 1) == 1;
    currentEnemyEngineTrailState = PlayerPrefs.GetInt("EnemyEngineTrails", 1) == 1;
    currentEnemyHitEffectState = PlayerPrefs.GetInt("EnemyHitEffects", 1) == 1;

    // Update toggle UI
    if (musicToggle != null)
    {
      // Remove all existing listeners to prevent double calls or errors
      musicToggle.onValueChanged.RemoveAllListeners();

      musicToggle.isOn = currentMusicState;

      // Add the listener *after* setting the initial state
      musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
    }

    if (sfxToggle != null)
    {
      sfxToggle.onValueChanged.RemoveAllListeners();
      sfxToggle.isOn = currentSfxState;
      sfxToggle.onValueChanged.AddListener(OnSfxToggleChanged); // <-- Add listener
    }

    if (backgroundParticlesToggle != null)
    {
      backgroundParticlesToggle.onValueChanged.RemoveAllListeners();
      backgroundParticlesToggle.isOn = currentBackgroundParticlesState;
      backgroundParticlesToggle.onValueChanged.AddListener(OnBackgroundParticlesToggleChanged);
    }

    if (playerEngineTrailToggle != null)
    {
      playerEngineTrailToggle.onValueChanged.RemoveAllListeners();
      playerEngineTrailToggle.isOn = currentPlayerEngineTrailState;
      playerEngineTrailToggle.onValueChanged.AddListener(OnPlayerEngineTrailToggleChanged);
    }

    if (playerHitEffectToggle != null)
    {
      playerHitEffectToggle.onValueChanged.RemoveAllListeners();
      playerHitEffectToggle.isOn = currentPlayerHitEffectState;
      playerHitEffectToggle.onValueChanged.AddListener(OnPlayerHitEffectToggleChanged);
    }

    if (enemyEngineTrailToggle != null)
    {
      enemyEngineTrailToggle.onValueChanged.RemoveAllListeners();
      enemyEngineTrailToggle.isOn = currentEnemyEngineTrailState;
      enemyEngineTrailToggle.onValueChanged.AddListener(OnEnemyEngineTrailToggleChanged);
    }

    if (enemyHitEffectToggle != null)
    {
      enemyHitEffectToggle.onValueChanged.RemoveAllListeners();
      enemyHitEffectToggle.isOn = currentEnemyHitEffectState;
      enemyHitEffectToggle.onValueChanged.AddListener(OnEnemyHitEffectToggleChanged);
    }
  }

  public void OnMusicToggleChanged(bool enabled)
  {
    ToggleMusic(enabled);
  }

  public void OnSfxToggleChanged(bool enabled)
  {
    ToggleSFX(enabled);
  }

  public void OnBackgroundParticlesToggleChanged(bool enabled)
  {
    ToggleBackgroundParticles(enabled);
  }

  public void OnPlayerEngineTrailToggleChanged(bool enabled)
  {
    TogglePlayerEngineTrails(enabled);
  }

  public void OnPlayerHitEffectToggleChanged(bool enabled)
  {
    TogglePlayerHitEffects(enabled);
  }

  public void OnEnemyEngineTrailToggleChanged(bool enabled)
  {
    ToggleEnemyEngineTrails(enabled);
  }

  public void OnEnemyHitEffectToggleChanged(bool enabled)
  {
    ToggleEnemyHitEffects(enabled);
  }

  // Toggle Music
  public static void ToggleMusic(bool enabled)
  {
    if (MusicManager.Instance != null)
    {
      MusicManager.Instance.SetMusicEnabled(enabled);
    }
    else
    {
      PlayerPrefs.SetInt("Music", enabled ? 1 : 0);
      PlayerPrefs.Save();
    }
  }

  // Toggle SFX
  public static void ToggleSFX(bool enabled)
  {
    // Save to PlayerPrefs
    PlayerPrefs.SetInt("SFX", enabled ? 1 : 0);
    PlayerPrefs.Save();
  }

  // Toggle Background particles
  public static void ToggleBackgroundParticles(bool enabled)
  {
    PlayerPrefs.SetInt("Particles", enabled ? 1 : 0);
    PlayerPrefs.Save();

    UpdateBackgroundParticles(enabled);
  }

  // Toggle Player Engine Trail
  public static void TogglePlayerEngineTrails(bool enabled)
  {
    PlayerPrefs.SetInt("EngineTrails", enabled ? 1 : 0);
    PlayerPrefs.Save();
    UpdatePlayerEngineTrails(enabled);
  }

  // Toggle Player Hit Effect
  public static void TogglePlayerHitEffects(bool enabled)
  {
    PlayerPrefs.SetInt("PlayerHitEffects", enabled ? 1 : 0);
    PlayerPrefs.Save();
  }

  // Toggle Enemy Engine Trail
  public static void ToggleEnemyEngineTrails(bool enabled)
  {
    PlayerPrefs.SetInt("EnemyEngineTrails", enabled ? 1 : 0);
    PlayerPrefs.Save();
    UpdateEnemyEngineTrails(enabled);
  }

  // Toggle Enemy Hit Effect
  public static void ToggleEnemyHitEffects(bool enabled)
  {
    PlayerPrefs.SetInt("EnemyHitEffects", enabled ? 1 : 0);
    PlayerPrefs.Save();
  }

  public static void UpdateBackgroundParticles(bool enabled)
  {
    // Find all active instances of the new controller script in the current scene(s)
    BackgroundParticleController[] allBackgrounds = FindObjectsOfType<BackgroundParticleController>();

    foreach (BackgroundParticleController bgController in allBackgrounds)
    {
      // Call the specific method on the controller to manage the particle system
      bgController.ApplyParticleState(enabled);
    }
  }

  public static void UpdatePlayerEngineTrails(bool enabled)
  {
    // Find all active instances of the new controller script
    PlayerParticleController[] allPlayers = FindObjectsOfType<PlayerParticleController>();

    foreach (PlayerParticleController playerController in allPlayers)
    {
      // PlayerParticleController will now only manage engine trails
      playerController.ApplyParticleState(enabled);
    }
  }

  public static void UpdateEnemyEngineTrails(bool enabled)
  {
    // Find all active instances of the EnemyParticleController
    EnemyParticleController[] allEnemies = FindObjectsOfType<EnemyParticleController>();

    foreach (EnemyParticleController enemyController in allEnemies)
    {
      // Controller will now only manage engine trails
      enemyController.ApplyParticleState(enabled);
    }
  }

  public void BackToMenu()
  {
    SceneManager.LoadScene("MainMenu");
  }
}
