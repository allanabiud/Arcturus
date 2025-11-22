using UnityEngine;
using System.Collections.Generic;

public static class FloatingPickupManager
{
  private static List<float> occupiedYPositions = new List<float>();
  private static float floatSpacing = 0.5f; // vertical spacing between pickups

  // Get a Y position for a new pickup
  public static float GetNextYPosition(float targetY)
  {
    float y = targetY;

    // Shift up until we find a free spot
    while (occupiedYPositions.Contains(Mathf.Round(y * 100f) / 100f))
    {
      y += floatSpacing;
    }

    occupiedYPositions.Add(Mathf.Round(y * 100f) / 100f); // round to avoid float precision issues
    return y;
  }

  // Free a spot when pickup is collected
  public static void ReleaseYPosition(float y)
  {
    occupiedYPositions.Remove(Mathf.Round(y * 100f) / 100f);
  }
}

