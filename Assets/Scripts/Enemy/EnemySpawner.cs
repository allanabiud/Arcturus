using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
  public List<EnemyType> enemiesToSpawn;
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
  public UnityEngine.UI.Image countdownImage;
  public List<Sprite> countdownSprites;

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

    countdownImage.gameObject.SetActive(true);

    while (countdown > 0)
    {
      int spriteIndex = Mathf.CeilToInt(countdown) - 1;

      // Safety check
      if (spriteIndex >= 0 && spriteIndex < countdownSprites.Count)
        countdownImage.sprite = countdownSprites[spriteIndex];

      yield return new WaitForSeconds(1f);
      countdown -= 1f;
    }

    // Optional GO! image (index 3)
    if (countdownSprites.Count > 3)
    {
      countdownImage.sprite = countdownSprites[3];
      yield return new WaitForSeconds(1f);
    }

    countdownImage.gameObject.SetActive(false);

    // Enable pause after countdown
    if (pauseButton != null)
      pauseButton.SetActive(true);

    // Enable player shooting AFTER countdown finishes
    GameState.canPlayerShoot = true;

    StartCoroutine(SpawnWave());
  }

  IEnumerator SpawnWave()
  {
    if (currentWaveIndex >= waves.Count) yield break;

    // Small delay after countdown
    yield return new WaitForSeconds(0.5f);

    // Show wave text AND wait until fade in/out completes
    yield return StartCoroutine(ShowWaveText());

    Wave wave = waves[currentWaveIndex];
    List<GameObject> spawnedEnemies = new List<GameObject>();
    List<Vector2> usedPositions = new List<Vector2>(); // To avoid overlaps

    // Calculate camera bounds
    float camHeight = 2f * mainCamera.orthographicSize;
    float camWidth = camHeight * mainCamera.aspect;

    float xMin = mainCamera.transform.position.x - camWidth / 2f + padding;
    float xMax = mainCamera.transform.position.x + camWidth / 2f - padding;
    float yMin = mainCamera.transform.position.y + camHeight / 3f; // above middle
    float yMax = mainCamera.transform.position.y + camHeight / 2f - 1.5f; // top half

    for (int i = 0; i < wave.totalEnemies; i++)
    {
      EnemyType type = wave.enemiesToSpawn[Random.Range(0, wave.enemiesToSpawn.Count)];

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
    yield return new WaitForSeconds(5f);

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

    // --- START FULLY TRANSPARENT ---
    Color c = waveText.color;
    c.a = 0f;
    waveText.color = c;

    // ---------- FADE IN ----------
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

    // ---------- VISIBLE TIME ----------
    yield return new WaitForSeconds(visibleDuration);

    // ---------- FADE OUT ----------
    t = 0f;
    while (t < fadeDuration)
    {
      t += Time.deltaTime;
      float a = Mathf.Lerp(1f, 0f, t / fadeDuration);
      c.a = a;
      waveText.color = c;
      yield return null;
    }

    // Hide
    waveText.gameObject.SetActive(false);
  }
}

