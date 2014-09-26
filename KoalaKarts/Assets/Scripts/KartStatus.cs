using UnityEngine;
using System.Collections;

/// <summary>
/// Script for keeping track of lives,
/// hits, points, and leaves.
/// </summary>
public class KartStatus : MonoBehaviour
{
    private int lives = 3;
    private int hits = 3;
    private int currentPoints = 0;
    private int currentLeaves = 0;
    private int bankedLeaves = 0;

    private int leafPointValue = 100;

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
    /// Getter for remaining hits.
    /// </summary>
    /// <returns> Remaining Hits </returns>
    public int GetHits()
    {
        return hits;
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
        PrintDebug();
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
        else
            Respawn();

        PrintDebug();
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
        PrintDebug();
    }

    /// <summary>
    /// Respawn player.
    /// </summary>
    void Respawn()
    {

    }

    /// <summary>
    /// Handle death of player.
    /// </summary>
    void OnDeath()
    {
        Application.LoadLevel("Menus");
    }

    #endregion

    #region Leaves

    /// <summary>
    /// Add 1 leaf to the kart's current leaves.
    /// </summary>
    public void AddLeaf()
    {
        currentLeaves++;
        PrintDebug();
    }

    /// <summary>
    /// Add a specified amount of leaves to
    /// the kart's current leaves.
    /// </summary>
    /// <param name="leaves"> Leaves to add </param>
    public void AddLeaves(int leaves)
    {
        currentLeaves += leaves;
        PrintDebug();
    }

    /// <summary>
    /// Subtract 1 leaf to the kart's current leaves.
    /// </summary>
    public void SubtractLeaf()
    {
        currentLeaves--;
        PrintDebug();
    }

    /// <summary>
    /// Subtract a specified amount of leaves
    /// from the kart's current leaves.
    /// </summary>
    /// <param name="leaves"> Leaves to subtract </param>
    public void SubtractLeaves(int leaves)
    {
        currentLeaves -= leaves;
        PrintDebug();
    }

    /// <summary>
    /// Bank all current leaves.
    /// </summary>
    public void BankLeaves()
    {
        bankedLeaves += currentLeaves;
        AddPoints(currentLeaves * leafPointValue);
        currentLeaves = 0;
        PrintDebug();
    }

    #endregion

    #region Points

    /// <summary>
    /// Add 1 point to current points.
    /// </summary>
    public void AddPoint()
    {
        currentPoints++;
        PrintDebug();
    }

    /// <summary>
    /// Add specified number of points to
    /// current points.
    /// </summary>
    /// <param name="points"> Points to be added </param>
    public void AddPoints(int points)
    {
        currentPoints += points;
        PrintDebug();
    }

    /// <summary>
    /// Subtract 1 point from current points
    /// </summary>
    public void SubtractPoint()
    {
        currentPoints--;
        PrintDebug();
    }

    /// <summary>
    /// Subtract specified number of points
    /// from current points.
    /// </summary>
    /// <param name="points"> Points to subtract </param>
    public void SubtractPoints(int points)
    {
        currentPoints -= points;
        PrintDebug();
    }

    #endregion

    #region Debug

    /// <summary>
    /// Method for printing current values.
    /// </summary>
    private void PrintDebug()
    {
        Debug.Log("Remaining Hits: " + GetHits());
        Debug.Log("Current Lives: " + GetLives());
        Debug.Log("Current Points: " + GetCurrentPoints());
        Debug.Log("Current Leaves: " + GetCurrentLeaves());
        Debug.Log("Banked Leaves: " + GetBankedLeaves());
    }

    #endregion
}
