using UnityEngine;
using System.Collections;

public class KartHealth : MonoBehaviour
{
    private int lives = 3;
    private int hits = 0;
    private int currentPoints = 0;
    private int currentLeaves = 0;
    private int bankedLeaves = 0;

    #region Getters

    /// <summary>
    /// Getter for current lives.
    /// </summary>
    /// <returns> Lives </returns>
    public int GetLives()
    {
        return lives;
    }

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
    /// Getter for kart's current points.
    /// </summary>
    /// <returns> Current points </returns>
    public int GetCurrentPoints()
    {
        return currentPoints;
    }

    #endregion

    #region Lives & Hits

    /// <summary>
    /// Add life to kart.
    /// </summary>
    public void AddLife()
    {
        lives++;
    }

    /// <summary>
    /// Subtract life from kart. If out of
    /// lives, call OnDeath().
    /// </summary>
    public void SubtractLife()
    {
        lives--;
        if (lives == 0)
            OnDeath();
    }

    /// <summary>
    /// Hit kart.  Subtract a hit and
    /// if hits == 0, subtract a life.
    /// </summary>
    public void Hit()
    {
        hits--;
        if (hits == 0)
        {
            SubtractLife();
        }
    }

    /// <summary>
    /// Handle death of player.
    /// </summary>
    void OnDeath()
    {
        // TODO: Handle death
    }

    #endregion

    #region Leaves

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
    }

    /// <summary>
    /// Subtract a specified amount of leaves
    /// from the kart's current leaves.
    /// </summary>
    /// <param name="leaves"> Leaves to subtract </param>
    public void SubtractLeaves(int leaves)
    {
        currentLeaves -= leaves;
    }

    #endregion

    #region Points

    /// <summary>
    /// Add 1 point to current points.
    /// </summary>
    public void AddPoint()
    {
        currentPoints++;
    }

    /// <summary>
    /// Add specified number of points to
    /// current points.
    /// </summary>
    /// <param name="points"> Points to be added </param>
    public void AddPoints(int points)
    {
        currentPoints += points;
    }

    /// <summary>
    /// Subtract 1 point from current points
    /// </summary>
    public void SubtractPoint()
    {
        currentPoints--;
    }

    /// <summary>
    /// Subtract specified number of points
    /// from current points.
    /// </summary>
    /// <param name="points"> Points to subtract </param>
    public void SubtractPoints(int points)
    {
        currentPoints -= points;
    }

    #endregion

    /// <summary>
    /// Bank all current leaves.
    /// </summary>
    public void BankLeaves()
    {
        bankedLeaves += currentLeaves;
        currentLeaves = 0;
    }
}
