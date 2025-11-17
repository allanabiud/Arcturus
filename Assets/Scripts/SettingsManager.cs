using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
  public void BackToMenu()
  {
    SceneManager.LoadScene("MainMenu");  // OR whatever your menu scene is called
  }
}
