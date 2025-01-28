using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages wave progression, enemy spawning, and difficulty scaling in the game.
/// Handles tracking of spawned enemies, dead enemies, and wave transitions.
/// </summary>
[Serializable]
public class SyzozWavesManager
{
    #region Wave Properties
    public int currentWave = 1;                   // The current wave number.
    public int baseEnemiesPerWave = 10;          // Base number of enemies to spawn per wave.
    public float difficultyMultiplier = 1.1f;    // Multiplier for increasing difficulty per wave.
    public int enemiesToSpawn;                   // Total number of enemies to spawn in the current wave.
    public int enemiesSpawned;                   // Number of enemies spawned so far in the current wave.
    public int enemiesDeadPerWav;                // Number of enemies killed in the current wave.
    public int enemiesDead;                      // Total number of enemies killed across all waves.
    public bool GameStart;                       // Indicates whether the game has started.
    #endregion

    #region Spawning Properties
    public float spawnRadius = 10f;              // Radius within which enemies spawn around the player.
    public int allowedEnemys = 15;               // Maximum number of active enemies allowed at a time.
    public float nextSpawnTime;                  // Time for the next enemy spawn.
    public List<SyzozEnemy> listEnemys = new List<SyzozEnemy>(); // List of active enemies in the current wave.
    public List<GameObject> Compoenent = new List<GameObject>(); // List player Packages
    #endregion

    #region Wave Logic
    /// <summary>
    /// Calculates the number of enemies to spawn for the current wave.
    /// </summary>
    /// <returns>The total number of enemies to spawn for the wave.</returns>
    public int GetEnemiesForWave()
    {
        return Mathf.RoundToInt(baseEnemiesPerWave * currentWave);
    }

    /// <summary>
    /// Scales enemy stats based on the current wave's difficulty multiplier.
    /// </summary>
    /// <param name="enemy">The enemy whose stats will be scaled.</param>
    public void ScaleEnemyStats(statsEnemy enemy)
    {
        // Implement scaling logic here (e.g., increase HP, speed, or damage based on wave).
    }

    /// <summary>
    /// Advances to the next wave by incrementing the wave counter.
    /// </summary>
    public void NextWave()
    {
        currentWave++;
    }

    /// <summary>
    /// Clear All Enemys And Reset Settings For Waves
    /// </summary>
    public void ClearWaves()
    {
        GameStart = true;
        difficultyMultiplier = 0;
        baseEnemiesPerWave = 10;
        currentWave = 1;
        enemiesToSpawn = 0;
        enemiesSpawned = 0;
        enemiesDeadPerWav = 0;
        enemiesDead = 0;
        spawnRadius = 10f;
        nextSpawnTime = 0f;
        foreach (var enemy in listEnemys)
            enemy.Destroy();
        listEnemys.Clear();
    }

    /// <summary>
    /// Starts a new wave by initializing wave properties and spawning logic.
    /// </summary>
    public void StartWave()
    {
        GameStart = true;
        enemiesToSpawn = GetEnemiesForWave();
        enemiesSpawned = 0;
        enemiesDeadPerWav = 0;
        //Debug.Log($"Starting Wave {currentWave}");
    }

    /// <summary>
    /// Progresses to the next wave after completing the current one.
    /// </summary>
    public void ProgressToNextWave()
    {
        enemiesDeadPerWav = 0;
        NextWave();
        StartWave();
    }
    #endregion

    #region Enemy Management
    /// <summary>
    /// Adds an enemy to the active enemy list.
    /// </summary>
    /// <param name="enemy">The enemy to add.</param>
    public void AddEnemy(SyzozEnemy enemy)
    {
        listEnemys.Add(enemy);
    }

    /// <summary>
    /// Removes an enemy from the active enemy list and handles its death.
    /// </summary>
    /// <param name="enemy">The enemy to remove.</param>
    public void RemoveEnemy(SyzozEnemy enemy)
    {
        listEnemys.Remove(enemy);
        enemy.Kill();
    }

    /// <summary>
    /// Checks if the given enemy is allowed to spawn based on active counts and restrictions.
    /// </summary>
    /// <param name="enemy">The enemy to check.</param>
    /// <returns>True if the enemy is allowed to spawn; otherwise, false.</returns>
    public bool isAllowed(statsEnemy enemy)
    {
        // Check if the maximum number of allowed enemies has been reached.
        if (listEnemys.Count >= allowedEnemys)
            return false;

        // Check if the enemy's spawn count exceeds its max count.
        if (enemy.maxCount != 0)
        {
            int counted = 0;
            foreach (var chEnemy in listEnemys)
            {
                if (chEnemy.TypeEnemy() == enemy)
                {
                    counted++;
                }
            }

            return counted < enemy.maxCount;
        }

        return true; // Default to true if no restrictions are applied.
    }

    /// <summary>
    /// Calculates the progress of the current wave based on enemies killed.
    /// </summary>
    /// <returns>A normalized value (0 to 1) representing the wave progress.</returns>
    public float waveFiller()
    {
        float progress = (float)enemiesDeadPerWav / (float)enemiesToSpawn;
        return progress;
    }

    /// <summary>
    /// Clear The Packages Of The User
    /// </summary>
    public void ClearPackages()
    {
        foreach(var item in Compoenent)
            if(item != null)
                UnityEngine.Object.Destroy(item);
        Compoenent.Clear();
    }

    /// <summary>
    /// Add Item Pack For the Player
    /// </summary>
    /// <param name="bagage"></param>
    public void AddPackages(GameObject bagage)
    {
        Compoenent.Add(bagage);
    }
    #endregion
}
