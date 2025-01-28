using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a ScriptableObject that stores references to actor-related prefabs,
/// including enemies and the player.
/// </summary>
[CreateAssetMenu(fileName = "SyzozActors", menuName = "Syzoz/SyzozActors")]
public class SyzozActors : ScriptableObject
{
    [SerializeField] private statsEnemy[] enemyPrefabs; // Array of enemy stats and prefabs.
    [SerializeField] private SyzozPlayer  Player;       // Reference to the player prefab.

    /// <summary>
    /// Retrieves a random enemy prefab from the list of available enemies.
    /// </summary>
    /// <returns>A randomly selected statsEnemy object.</returns>
    public statsEnemy GetEnemy()
    {
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
    }

    /// <summary>
    /// Retrieves the player prefab.
    /// </summary>
    /// <returns>The SyzozPlayer object representing the player.</returns>
    public SyzozPlayer GetPlayer()
    {
        return Player;
    }
}