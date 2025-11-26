using System.Collections;
using UnityEngine;
using TMPro; // For TextMeshPro

public class CoinPickup : MonoBehaviour
{
  public int coinValue;
  public GameObject coinPickupTextPrefab;

  public float textLifetime = 1f; // How long the text stays

  [Header("Audio")]
  public AudioClip coinPickupSFX;
  public float coinVolume = 1f;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.transform.root.CompareTag("Player"))
    {
      PersistentGameState.Instance.AddCoins(coinValue);

      if (coinPickupTextPrefab != null)
      {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        GameObject instance = Instantiate(coinPickupTextPrefab, canvas.transform);

        // Set text to coin value
        // Set coin text
        TextMeshProUGUI text = instance.transform.Find("coinText")?.GetComponent<TextMeshProUGUI>();
        if (text != null) text.text = "+" + coinValue;

        // Position on canvas
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransform rt = instance.GetComponent<RectTransform>();
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvas.GetComponent<Canvas>().worldCamera,
            out anchoredPos
        );
        rt.anchoredPosition = anchoredPos;

        // Destroy after lifetime
        Destroy(instance, textLifetime);
      }

      // Play coin pickup sound
      if (SettingsManager.IsSfxEnabled && coinPickupSFX != null)
        AudioSource.PlayClipAtPoint(coinPickupSFX, transform.position, coinVolume);

      // Destroy the coin itself immediately
      Destroy(gameObject);
    }
  }

  void Update()
  {
    transform.Translate(Vector3.down * 3f * Time.deltaTime);

    // Destroy when off-screen
    Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

    if (viewportPos.y < -0.1f)  // allow some buffer below screen
    {
      Destroy(gameObject);
    }
  }
}

