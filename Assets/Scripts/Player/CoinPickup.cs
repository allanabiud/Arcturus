using System.Collections;
using UnityEngine;
using TMPro; // For TextMeshPro

public class CoinPickup : MonoBehaviour
{
  public int coinValue = 1;
  public GameObject coinPrefab; // Prefab with coin icon and text

  public float textLifetime = 1f; // How long the text stays

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Player"))
    {
      PersistentGameState.Instance.AddCoins(coinValue);

      if (coinPrefab != null)
      {
        GameObject canvas = GameObject.Find("Canvas");

        // Spawn slightly above and to the right
        Vector3 worldPos = transform.position + new Vector3(0.3f, 0.5f, 0f);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        // Instantiate prefab under Canvas
        GameObject instance = Instantiate(coinPrefab, canvas.transform);
        instance.transform.position = screenPos;
        instance.transform.localScale = Vector3.one;

        // Destroy the prefab after 'textLifetime' seconds
        Destroy(instance, textLifetime);
      }

      // Destroy the coin itself immediately
      Destroy(gameObject);
    }
  }

  void Update()
  {
    transform.Translate(Vector3.down * 3f * Time.deltaTime);
  }
}

