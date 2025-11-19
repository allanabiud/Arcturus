using UnityEngine;

public class PersistentGameState : MonoBehaviour
{
  public static PersistentGameState Instance;

  public int Coins { get; private set; } = 0;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
      Coins = PlayerPrefs.GetInt("Coins", 0);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  public void AddCoins(int amount)
  {
    Coins += amount;
    PlayerPrefs.SetInt("Coins", Coins);
    PlayerPrefs.Save();
  }

  public bool SpendCoins(int amount)
  {
    if (Coins >= amount)
    {
      Coins -= amount;
      PlayerPrefs.SetInt("Coins", Coins);
      PlayerPrefs.Save();
      return true;
    }
    return false;
  }
}
