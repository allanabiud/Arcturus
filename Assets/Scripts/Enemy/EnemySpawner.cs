using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class EnemyType
{
  public string name;
  public GameObject prefab;
}

[System.Serializable]
public class Wave
{
  public string waveName;
  public List<EnemyType> enemyTypesToSpawn;
  public int totalEnemies;
  public float spawnInterval;
}

public class EnemySpawner : MonoBehaviour
{
  [Header("Wave Settings")]
  public List<Wave> waves;

  [Header("Wave UI")]
  public TMPro.TextMeshProUGUI waveText;
  public float fadeDuration = 0.5f;     // fade in/out time
  public float visibleDuration = 1f;  // time fully visible


  [Header("Spawn Area Settings")]
  public float padding = 0.5f;       // Space from screen edges
  public float spawnYOffset = 0.5f;  // Optional: spawn slightly above the top half
  public float flyInSpeed = 5f;      // Speed for enemies flying in

  [Header("Start Settings")]
  public float startDelay = 3f;       // Countdown duration
  public TMPro.TextMeshProUGUI countdownText;
  // public UnityEngine.UI.Image countdownImage;
  // public List<Sprite> countdownSprites;

  [Header("UI Controls")]
  public GameObject pauseButton;


  private Camera mainCamera;
  private int currentWaveIndex = 0;

  private void Start()
  {
    mainCamera = Camera.main;
    StartCoroutine(StartCountdownAndSpawn());
  }

  private IEnumerator StartCountdownAndSpawn()
  {
    // Disable pause while countdown is active
    if (pauseButton != null)
      pauseButton.SetActive(false);

    // Stop player from shooting during countdown
    GameState.canPlayerShoot = false;

    float countdown = startDelay;

    countdownText.gameObject.SetActive(true);

    while (countdown > 0)
    {
      int displayValue = Mathf.CeilToInt(countdown);

      // Set the text to the number
      countdownText.text = displayValue.ToString();

      yield return new WaitForSeconds(1f);
      countdown -= 1f;
    }

    // Display GO!
    countdownText.text = "GO!";
    yield return new WaitForSeconds(1f);

    // Hide countdown text
    countdownText.gameObject.SetActive(false);

    // Enable pause after countdown
    if (pauseButton != null)
      pauseButton.SetActive(true);

    // Start wave
    StartCoroutine(SpawnWave());
  }

  IEnumerator SpawnWave()
  {
    if (currentWaveIndex >= waves.Count)
    {
      // Trigger victory!
      GameState.Instance.Victory();

      yield break;
    }

    // Delay after countdown
    yield return new WaitForSeconds(0.5f);

    // Show wave text and wait until fade in/out completes
    yield return StartCoroutine(ShowWaveText());

    // Update gamestate wave stats
    GameState.Instance.SetWave(currentWaveIndex + 1);

    // Enable player shooting after WaveText
    GameState.canPlayerShoot = true;

    Wave wave = waves[currentWaveIndex];
    List<GameObject> spawnedEnemies = new List<GameObject>();
    List<Vector2> usedPositions = new List<Vector2>(); // To avoid overlaps

    // Calculate camera bounds
    float camHeight = 2f * mainCamera.orthographicSize;
    float camWidth = camHeight * mainCamera.aspect;

    float camTop = mainCamera.transform.position.y + camHeight / 2f;
    float camMiddle = mainCamera.transform.position.y;

    float textReserved = 1.0f; // amount of space at the top you want to block

    // Spawn zone starts below the text
    float yTopSpawnLimit = camTop - textReserved;

    // Spawn zone bottom moves toward the middle
    float yBottomSpawnLimit = Mathf.Lerp(yTopSpawnLimit, camMiddle, 0.5f);

    float xMin = mainCamera.transform.position.x - camWidth / 2f + padding;
    float xMax = mainCamera.transform.position.x + camWidth / 2f - padding;
    float yMin = yBottomSpawnLimit;
    float yMax = yTopSpawnLimit;

    for (int i = 0; i < wave.totalEnemies; i++)
    {
      EnemyType type = wave.enemyTypesToSpawn[Random.Range(0, wave.enemyTypesToSpawn.Count)];

      // Try to find a position that doesn’t overlap
      Vector2 spawnPos2D;
      int tries = 0;
      do
      {
        float xPos = Random.Range(xMin, xMax);
        float yPos = Random.Range(yMin, yMax);
        spawnPos2D = new Vector2(xPos, yPos);
        tries++;
      } while (IsOverlapping(spawnPos2D, usedPositions, 1f) && tries < 20); // 1f = minimum spacing

      usedPositions.Add(spawnPos2D);

      // Spawn off-screen above the top
      Vector3 spawnPos = new Vector3(spawnPos2D.x, yMax + 2f, 0f);
      GameObject enemy = Instantiate(type.prefab, spawnPos, type.prefab.transform.rotation);

      // Add fly-in component if not already present
      EnemyFlyIn flyIn = enemy.GetComponent<EnemyFlyIn>();
      if (flyIn == null)
        flyIn = enemy.AddComponent<EnemyFlyIn>();

      flyIn.targetPosition = new Vector3(spawnPos2D.x, spawnPos2D.y, 0f);
      flyIn.speed = flyInSpeed;

      spawnedEnemies.Add(enemy);

      yield return new WaitForSeconds(wave.spawnInterval);
    }

    // Wait until all enemies are destroyed
    while (spawnedEnemies.Count > 0)
    {
      spawnedEnemies.RemoveAll(e => e == null);
      yield return null;
    }

    // Delay before next wave
    yield return new WaitForSeconds(1f);

    currentWaveIndex++;
    StartCoroutine(SpawnWave());
  }

  // Check if a new position is too close to existing ones
  private bool IsOverlapping(Vector2 pos, List<Vector2> positions, float minDistance)
  {
    foreach (Vector2 used in positions)
    {
      if (Vector2.Distance(pos, used) < minDistance)
        return true;
    }
    return false;
  }

  private IEnumerator ShowWaveText()
  {
    if (waveText == null) yield break;

    waveText.gameObject.SetActive(true);

    string textToShow = "WAVE " + (currentWaveIndex + 1);
    waveText.text = textToShow;

    // Start fully transparent
    Color c = waveText.color;
    c.a = 0f;
    waveText.color = c;

    // Fade-In
    float t = 0f;
    while (t < fadeDuration)
    {
      t += Time.deltaTime;
      float a = Mathf.Lerp(0f, 1f, t / fadeDuration);
      c.a = a;
      waveText.color = c;
      yield return null;
    }

    // Hold fully visible
    c.a = 1f;
    waveText.color = c;

    // Time text is visible
    yield return new WaitForSeconds(visibleDuration);

    // Fade-Out
    t = 0f;
    while (t < fadeDuration)
    {
      t += Time.deltaTime;
      float a = Mathf.Lerp(1f, 0f, t / fadeDuration);
      c.a = a;
      waveText.color = c;
      yield return null;
    }

    // Hide WaveText
    waveText.gameObject.SetActive(false);
  }
}

