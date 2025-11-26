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
  public Toggle playerParticlesToggle;
  public Toggle enemyParticlesToggle;

  private bool currentMusicState;
  private bool currentSfxState;
  private bool currentBackgroundParticlesState;
  private bool currentPlayerParticlesState;
  private bool currentEnemyParticlesState;

  public static bool IsSfxEnabled => PlayerPrefs.GetInt("SFX", 1) == 1;
  public static bool AreBackgroundParticlesEnabled => PlayerPrefs.GetInt("Particles", 1) == 1;
  public static bool ArePlayerParticlesEnabled => PlayerPrefs.GetInt("PlayerParticles", 1) == 1;
  public static bool AreEnemyParticlesEnabled => PlayerPrefs.GetInt("EnemyParticles", 1) == 1;

  private void Start()
  {
    currentMusicState = PlayerPrefs.GetInt("Music", 1) == 1;
    currentSfxState = PlayerPrefs.GetInt("SFX", 1) == 1;
    currentBackgroundParticlesState = PlayerPrefs.GetInt("Particles", 1) == 1;
    currentPlayerParticlesState = PlayerPrefs.GetInt("PlayerParticles", 1) == 1;
    currentEnemyParticlesState = PlayerPrefs.GetInt("EnemyParticles", 1) == 1;

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

    if (playerParticlesToggle != null)
    {
      playerParticlesToggle.onValueChanged.RemoveAllListeners();
      playerParticlesToggle.isOn = currentPlayerParticlesState;
      playerParticlesToggle.onValueChanged.AddListener(OnPlayerParticlesToggleChanged);
    }

    if (enemyParticlesToggle != null)
    {
      enemyParticlesToggle.onValueChanged.RemoveAllListeners();
      enemyParticlesToggle.isOn = currentEnemyParticlesState;
      enemyParticlesToggle.onValueChanged.AddListener(OnEnemyParticlesToggleChanged);
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

  public void OnPlayerParticlesToggleChanged(bool enabled)
  {
    TogglePlayerParticles(enabled);
  }

  public void OnEnemyParticlesToggleChanged(bool enabled)
  {
    ToggleEnemyParticles(enabled);
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

  // Toggle Player particles
  public static void TogglePlayerParticles(bool enabled)
  {
    PlayerPrefs.SetInt("PlayerParticles", enabled ? 1 : 0);
    PlayerPrefs.Save();

    UpdatePlayerParticles(enabled);
  }

  public static void ToggleEnemyParticles(bool enabled)
  {
    PlayerPrefs.SetInt("EnemyParticles", enabled ? 1 : 0);
    PlayerPrefs.Save();

    UpdateEnemyParticles(enabled);
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

  public static void UpdatePlayerParticles(bool enabled)
  {
    // Find all active instances of the new controller script
    PlayerParticleController[] allPlayers = FindObjectsOfType<PlayerParticleController>();

    foreach (PlayerParticleController playerController in allPlayers)
    {
      playerController.ApplyParticleState(enabled);
    }
  }

  public static void UpdateEnemyParticles(bool enabled)
  {
    // Find all active instances of the new controller script
    EnemyParticleController[] allEnemies = FindObjectsOfType<EnemyParticleController>();

    foreach (EnemyParticleController enemyController in allEnemies)
    {
      enemyController.ApplyParticleState(enabled);
    }
  }

  public void BackToMenu()
  {
    SceneManager.LoadScene("MainMenu");
  }
}
