using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
  public void BackToMenu()
  {
    SceneManager.LoadScene("MainMenu");  // OR whatever your menu scene is called
  }
}
