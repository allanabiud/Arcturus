using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
  public void StartGame()
  {
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
