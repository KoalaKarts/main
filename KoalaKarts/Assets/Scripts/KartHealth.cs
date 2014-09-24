using UnityEngine;
using System.Collections;

public class KartHealth : MonoBehaviour
{
    private int currentLeaves = 3;
    private int bankedLeaves = 0;

    /// <summary>
    /// Getter for kart's current leaves.
    /// </summary>
    /// <returns> Current leaves </returns>
    public int GetCurrentLeaves()
    {
        return currentLeaves;
    }

    /// <summary>
    /// Getter for kart's banked leaves.
    /// </summary>
    /// <returns> Current leaves </returns>
    public int GetBankedLeaves()
    {
        return bankedLeaves;
    }

    /// <summary>
    /// Add 1 leaf to the kart's current leaves.
    /// </summary>
    public void AddLeaf()
    {
        currentLeaves++;
    }

    /// <summary>
    /// Add a specified amount of leaves to
    /// the kart's current leaves.
    /// </summary>
    /// <param name="leaves"> Leaves to add </param>
    public void AddLeaves(int leaves)
    {
        currentLeaves += leaves;
    }

    /// <summary>
    /// Subtract 1 leaf to the kart's current leaves.
    /// </summary>
    public void SubtractLeaf()
    {
        currentLeaves--;
        if (currentLeaves < 0)
            OnDeath();
    }

    /// <summary>
    /// Subtract a specified amount of leaves
    /// from the kart's current leaves.
    /// </summary>
    /// <param name="leaves"> Leaves to subtract </param>
    public void SubtractLeaves(int leaves)
    {
        currentLeaves -= leaves;
        if (currentLeaves < 0)
            OnDeath();
    }

    /// <summary>
    /// Bank all current leaves.
    /// </summary>
    public void BankLeaves()
    {
        bankedLeaves += currentLeaves;
        currentLeaves = 0;
    }

    /// <summary>
    /// Handle death of player.
    /// </summary>
    void OnDeath()
    {
        // TODO: Handle death
    }
}
