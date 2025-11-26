using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
  private bool isPaused = false;

  [Header("Top Right Pause Button")]
  public GameObject topRightPauseButton;

  [Header("UI Icon Switching")]
  public Image iconImage;
  public Sprite pauseSprite;
  public Sprite playSprite;

  [Header("Pause Menu")]
  public GameObject pauseMenuPanel;

  public void TogglePause()
  {
    if (isPaused)
      Resume();
    else
      Pause();
  }

  public void Pause()
  {
    Time.timeScale = 0f;
    isPaused = true;

    if (MusicManager.Instance != null)
      MusicManager.Instance.SetMusicEnabled(false, false);

    // Hide top-right pause button
    if (topRightPauseButton != null)
      topRightPauseButton.SetActive(false);

    // Switch to "play" icon
    if (iconImage != null)
      iconImage.sprite = playSprite;

    // Show the pause menu
    if (pauseMenuPanel != null)
      pauseMenuPanel.SetActive(true);
  }

  public void Resume()
  {
    Time.timeScale = 1f;
    isPaused = false;

    if (MusicManager.Instance != null)
      MusicManager.Instance.SetMusicEnabled(MusicManager.IsMusicEnabled, false);

    // Show top-right pause button again
    if (topRightPauseButton != null)
      topRightPauseButton.SetActive(true);

    // Switch to "pause" icon
    if (iconImage != null)
      iconImage.sprite = pauseSprite;

    // Hide the pause menu
    if (pauseMenuPanel != null)
      pauseMenuPanel.SetActive(false);

  }

  public void GoToMainMenu()
  {
    Time.timeScale = 1f; // make sure game resumes

    if (MusicManager.Instance != null)
      MusicManager.Instance.RestartMusic();

    SceneManager.LoadScene("MainMenu");
  }

  public void RestartGame()
  {
    Time.timeScale = 1f; // make sure time is running

    if (MusicManager.Instance != null)
      MusicManager.Instance.RestartMusic();

    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}
