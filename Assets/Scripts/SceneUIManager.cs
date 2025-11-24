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

  private void Start()
  {
    // Reset everything every time GameScene loads
    Time.timeScale = 1f;
    isGameOver = false;
    canPlayerShoot = false;

    enemiesDestroyed = 0;
    currentWave = 1;

    // Reset static shield flag
    BasicEnemy.ShieldActiveInScene = false;

    // Reset ship upgrade flags & cooldown
    BasicEnemy.ShipUpgradeLv2ActiveInScene = false;
    BasicEnemy.ShipUpgradeLv3ActiveInScene = false;
    BasicEnemy.lastShipUpgradeSpawnTime = -999f;
  }

  public void RegisterEnemyDestroyed()
  {
    enemiesDestroyed++;
  }

  public void SetWave(int waveNumber)
  {
    currentWave = waveNumber;
  }

  private IEnumerator WaitForCoins(float timeout = 1f)
  {
    float timer = timeout;
    while (timer > 0f && GameObject.FindGameObjectsWithTag("CoinPickup").Length > 0)
    {
      timer -= Time.deltaTime;
      yield return null;
    }
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

    // Wait for coins to be collected BEFORE freezing time
    yield return StartCoroutine(WaitForCoins(1f));

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

    // Wait for coins
    yield return StartCoroutine(WaitForCoins(1f));

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
    // Destroy Player Object
    foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
      Destroy(player);

    // Destroy HealthUI
    foreach (GameObject healthUI in GameObject.FindGameObjectsWithTag("HealthUI"))
      Destroy(healthUI);

    // Destroy ShieldUI
    foreach (GameObject shieldUI in GameObject.FindGameObjectsWithTag("ShieldUI"))
      Destroy(shieldUI);

    // Destroy ShipUpgradeUI
    foreach (GameObject shipUpgradeUI in GameObject.FindGameObjectsWithTag("ShipUpgradeUI"))
      Destroy(shipUpgradeUI);

    // Destroy CoinPickups
    foreach (GameObject coinPickup in GameObject.FindGameObjectsWithTag("CoinPickup"))
      Destroy(coinPickup);

    // Destroy ShieldPickups
    foreach (GameObject shieldPickup in GameObject.FindGameObjectsWithTag("ShieldPickup"))
      Destroy(shieldPickup);

    // Destroy ShipUpgradePickups
    foreach (GameObject shipUpgradePickup in GameObject.FindGameObjectsWithTag("ShipUpgradePickup"))
      Destroy(shipUpgradePickup);

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
