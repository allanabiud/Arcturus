using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    // Load MainMenu or GameScene automatically
    SceneManager.LoadScene("MainMenu");

  }

  // Update is called once per frame
  void Update()
  {

  }
}
