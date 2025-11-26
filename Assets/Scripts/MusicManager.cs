using UnityEngine;

public class MusicManager : MonoBehaviour
{
  public static MusicManager Instance;

  public AudioSource audioSource;
  // Public property to check current state from PlayerPrefs
  public static bool IsMusicEnabled => PlayerPrefs.GetInt("Music", 1) == 1;

  void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
      audioSource = GetComponent<AudioSource>();

      SetMusicEnabled(IsMusicEnabled, false);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  public void SetMusicEnabled(bool enabled, bool saveState = true)
  {
    if (enabled)
    {
      // Only Play if it's not already playing and should be enabled
      if (!audioSource.isPlaying)
      {
        audioSource.Play();
      }
    }
    else
    {
      // Only Pause if it's currently playing
      if (audioSource.isPlaying)
      {
        audioSource.Pause();
      }
    }
    if (saveState)
    {
      PlayerPrefs.SetInt("Music", enabled ? 1 : 0);
      PlayerPrefs.Save();
    }
  }

  public void RestartMusic()
  {
    // Use the persistent state for decision
    if (audioSource != null && IsMusicEnabled)
    {
      audioSource.time = 0f;
      audioSource.Play();
    }
    else if (audioSource != null && !IsMusicEnabled)
    {
      // Just ensure it's paused if it should be off
      audioSource.Pause();
      audioSource.time = 0f; // Reset time anyway
    }
  }
}

