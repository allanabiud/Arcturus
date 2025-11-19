using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneUIManager : MonoBehaviour
{
  public static SceneUIManager Instance;

  [Header("Gameplay Flags")]
  public static bool isGameOver = false;
  public static bool canPlayerShoot = false;

  [Header("UI Elements")]
  public GameObject gameOverPanel;
  public GameObject topRightPauseButton;
  public TextMeshProUGUI currentWaveText;
  public TextMeshProUGUI enemiesDestroyedText;

  [Header("Victory UI")]
  public GameObject victoryPanel;
  public TextMeshProUGUI victoryWaveText;
  public TextMeshProUGUI victoryEnemiesText;


  [Header("Stats")]
  public int currentWave = 1;
  public int enemiesDestroyed = 0;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Destroy(gameObject);
    }
  }

  public void RegisterEnemyDestroyed()
  {
    enemiesDestroyed++;
  }

  public void SetWave(int waveNumber)
  {
    currentWave = waveNumber;
  }

  public void GameOver(float delay = 1f)
  {
    if (isGameOver) return;
    StartCoroutine(GameOverRoutine(delay));
  }
  private IEnumerator GameOverRoutine(float delay)
  {

    isGameOver = true;
    canPlayerShoot = false;

    // Stop time after delay
    yield return new WaitForSecondsRealtime(delay);

    // Stop time
    Time.timeScale = 0f;

    // Remove all enemies + bullets
    CleanupObjects();

    // Show game over panel
    gameOverPanel.SetActive(true);

    currentWaveText.text = currentWave.ToString();
    enemiesDestroyedText.text = enemiesDestroyed.ToString();
  }

  public void Victory(float delay = 1f)
  {
    if (isGameOver) return; // Prevent double triggers
    StartCoroutine(VictoryRoutine(delay));
  }

  private IEnumerator VictoryRoutine(float delay)
  {
    isGameOver = true;
    canPlayerShoot = false;

    yield return new WaitForSecondsRealtime(delay);

    // Stop time
    Time.timeScale = 0f;

    // Clean up objects
    CleanupObjects();

    // Show victory panel
    if (victoryPanel != null)
      victoryPanel.SetActive(true);

    // Update stats
    if (victoryWaveText != null)
      victoryWaveText.text = currentWave.ToString();

    if (victoryEnemiesText != null)
      victoryEnemiesText.text = enemiesDestroyed.ToString();
  }


  private void CleanupObjects()
  {
    // Destroy enemies
    foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
      Destroy(player);

    // Destroy enemies
    foreach (GameObject healthUI in GameObject.FindGameObjectsWithTag("HealthUI"))
      Destroy(healthUI);


    // Destroy enemies
    foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
      Destroy(enemy);

    // Destroy bullets
    foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet"))
      Destroy(bullet);

    foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("EnemyBullet"))
      Destroy(bullet);

    // Remove pauseIcon
    Destroy(topRightPauseButton);

    // (Optional) Destroy pickups or obstacles if you add them later
  }

}
