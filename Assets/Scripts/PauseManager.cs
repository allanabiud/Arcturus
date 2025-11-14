using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
  private bool isPaused = false;

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
    SceneManager.LoadScene("MainMenu");
  }

  public void RestartGame()
  {
    Time.timeScale = 1f; // make sure time is running
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}
