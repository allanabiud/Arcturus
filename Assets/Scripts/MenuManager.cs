using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
  public GameObject modePanel;
  public GameObject startButton;

  public void OpenModeMenu()
  {
    modePanel.SetActive(true);
    startButton.SetActive(false);
  }

  public void CloseModeMenu()
  {
    modePanel.SetActive(false);
    startButton.SetActive(true);
  }

  public void StartNormalMode()
  {
    GameMode.infiniteMode = false;
    SceneManager.LoadScene("GameScene");
  }

  public void StartInfiniteMode()
  {
    GameMode.infiniteMode = true;
    SceneManager.LoadScene("GameScene");
  }


  public void OpenShop()
  {
    SceneManager.LoadScene("Shop");
  }

  public void OpenSettings()
  {
    SceneManager.LoadScene("Settings");
  }

  public void ExitGame()
  {
    Application.Quit();
    Debug.Log("Game Quit"); // Works only in build
  }
}
