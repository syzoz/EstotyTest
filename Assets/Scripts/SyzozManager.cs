using System.Collections;
using UnityEngine;
using Zenject;

/// <summary>
/// Manages the core game systems, including player, waves, enemies, and UI updates.
/// Acts as a singleton for centralized access throughout the game.
/// </summary>
public class SyzozManager : MonoBehaviour
{
    #region Variables
    private SyzozPlayer       Player;             // Reference to the player controller.
    private SyzozWavesManager waveManager;        // Handles wave progression, enemy spawning, and wave logic.
    private SyzozActors       Actors;             // Provides enemy and actor data.
    private SyzozWeapons      Weapons;            // Provides weapon and power-up data.
    private Transform         Container;          // Parent transform for all dynamically spawned objects.
    private SyzozSoundManager Sound;
    private SyzozMain         Main;
    private SyzozUI           syzozUI;

    public bool IsStartGame => waveManager.GameStart;  // Indicates whether the game has started.
    public Transform containerParent => Container;     // Returns the parent container for organizing objects.
    #endregion

    #region Functions
    /// <summary>
    /// Injection of Zenject
    /// </summary>
    [Inject]
    public void Construct(SyzozSoundManager sound, SyzozMain main, SyzozUI SyzozUI)
    {
        Sound = sound;
        Main  = main;
        syzozUI = SyzozUI;
    }

    private void Start()
    {
        LookingNull();
    }

    /// <summary>
    /// Initializes the manager with references to key systems.
    /// </summary>
    public void Init(SyzozActors actors, SyzozWeapons weapons, SyzozPlayer player)
    {
        Actors      = actors;                          // Store the actors reference.
        Weapons     = weapons;                         // Store the weapons reference.
        Player      = player;                          // Store the player reference.
        waveManager = new SyzozWavesManager();         // Initialize the wave manager.

        InisilizeGame();                               // Set up initial game state.
    }

    /// <summary>
    /// Handles wave progression and enemy spawning logic every frame.
    /// </summary>
    private void Update()
    {
        // Exit if the game hasn't started or Plyer Dead.
        if (!waveManager.GameStart || Player != null && !Player.Health.isAlive() || Player == null)
            return;

        // Spawn enemies if the wave isn't complete and it's time to spawn the next enemy.
        if (waveManager.enemiesSpawned < waveManager.enemiesToSpawn && Time.time >= waveManager.nextSpawnTime)
        {
            SpawnEnemy(); // Spawn an enemy.
            waveManager.nextSpawnTime = Time.time + Random.Range(0f, 1f); // Set the next spawn time.
        }

        // Check if all enemies are dead and the wave is complete.
        if (waveManager.listEnemys.Count == 0 && waveManager.enemiesSpawned >= waveManager.enemiesToSpawn)
        {
            //Debug.Log($"Wave {waveManager.currentWave} completed!");
            waveManager.ProgressToNextWave(); // Progress to the next wave.
        }
    }

    /// <summary>
    /// Updates the UI each frame after all other updates.
    /// </summary>
    private void LateUpdate()
    {
        if (!waveManager.GameStart || !Player.Health.isAlive())
            return;

        DoUpdateUI(); // Update the UI elements.
    }

    /// <summary>
    /// Sets up the game's container for dynamically spawned objects.
    /// </summary>
    private void InisilizeGame()
    {
        Container = (new GameObject("Syzoz")).transform; // Create a parent container in the hierarchy.
    }

    /// <summary>
    /// Starts the game by initializing the wave manager and updating the UI.
    /// </summary>
    public void StartGame()
    {
        waveManager.StartWave(); // Begin the first wave.
        DoUpdateUI();            // Update the initial UI state.
        LoadPackages();          // Spawn initial power-ups or resources.
    }

    /// <summary>
    /// Revive The Use In Game Play
    /// </summary>
    public void Revive()
    {

    }

    /// <summary>
    /// Called Wheen the Use Lose or Try Replay Again
    /// </summary>
    public void ReplayGame()
    {
        waveManager.ClearWaves();
        Player.ActivatePlayer();
        Sound.PlaySound(SoundType.Select); // Play Sound Effect Directly
        Main.Reload();
        StartGame();
    }

    /// <summary>
    /// Spawns health recovery packages at random positions within bounds.
    /// </summary>
    private void LoadPackages()
    {
        waveManager.ClearPackages();

        Vector2 minBound = new Vector2(-156f, -122.49f); // Minimum boundary for package placement.
        Vector2 maxBound = new Vector2(178.16f, 118.79f); // Maximum boundary for package placement.

        int count = Random.Range(10, 40);   // Randomize the number of packages.
        for (int i = 0; i < count; i++)
        {
            Vector2 randomizePos = new Vector2(Random.Range(minBound.x, maxBound.x), Random.Range(minBound.y, maxBound.y));
            var recover          = Instantiate(Weapons.HealthRecover, randomizePos, Quaternion.identity); // Spawn a health recovery item.
            recover.transform.SetParent(Container); // Parent it to the container.
            waveManager.AddPackages(recover);
        }
    }

    /// <summary>
    /// Spawns an enemy in a random direction around the player.
    /// </summary>
    private void SpawnEnemy()
    {
        var angle          = Random.Range(0f, 360f); // Random angle for enemy spawn direction.
        var spawnDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        var spawnPosition  = (Vector2)Player.transform.position + spawnDirection * waveManager.spawnRadius;

        // Select a random enemy type.
        var enemySelect = Actors.GetEnemy();
        var enemyPrefab = enemySelect.Enemy;

        if (waveManager.isAllowed(enemySelect))
        {
            // Scale enemy stats based on the current wave.
            waveManager.ScaleEnemyStats(enemySelect);

            // Spawn and initialize the enemy.
            var objSpawn = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            objSpawn.SetUpEnemy(enemySelect, Player); // Set up the enemy with its data.
            objSpawn.transform.SetParent(Container);  // Parent it to the container.

            waveManager.AddEnemy(objSpawn);           // Track the enemy in the wave manager.
            waveManager.enemiesSpawned++;             // Increment the spawned enemy count.
        }
    }

    /// <summary>
    /// Handles the removal of a killed enemy and spawns experience orbs.
    /// </summary>
    public void KillEnemy(SyzozEnemy enemy)
    {
        waveManager.RemoveEnemy(enemy);               // Remove the enemy from the wave manager.
        waveManager.enemiesDead++;                   // Increment the dead enemy count.
        waveManager.enemiesDeadPerWav++;             // Increment the per-wave dead enemy count.

        // Spawn an experience orb at the enemy's position.
        var expFollower = Instantiate(Weapons.expPrefabe, enemy.transform.position, Quaternion.identity);
        expFollower.GetComponent<SpriteRenderer>().sprite = enemy.TypeEnemy().Exp; // Assign the appropriate sprite.
        expFollower.GetComponent<SyzozExp>().StartExp(Player.transform);          // Initialize the orb to follow the player.
        expFollower.transform.SetParent(Container);                               // Parent it to the container.
    }

    /// <summary>
    /// Handles the Player when the health equal zero 
    /// </summary>
    public void KillPlayer()
    {
        StartCoroutine(LoadingFinish());
        IEnumerator LoadingFinish()
        {
            Sound.PlaySound(SoundType.Dead); // Play Sound Effect Directly
            yield return new WaitForSeconds(2f);
            Main.ShowFinish();
            Sound.PlaySound(SoundType.Lose); // Play Sound Effect Directly
            yield return new WaitForSeconds(1f);
            ReplayGame();
        }
        waveManager.GameStart = false;
    }

    /// <summary>
    /// Updates the UI elements with current wave, killed enemies, and experience progress.
    /// </summary>
    public void DoUpdateUI()
    {
        LookingNull();

        syzozUI.UpdateLevel(waveManager.currentWave);       // Update wave level display.
        syzozUI.UpdateKilled(waveManager.enemiesDead);      // Update killed enemy count.
        syzozUI.UpdateExp(waveManager.waveFiller());        // Update experience progress.
    }

    public void LookingNull()
    {
        if (syzozUI == null || Main == null || Sound == null)
        {
            Main = FindFirstObjectByType<SyzozMain>();
            syzozUI = FindFirstObjectByType<SyzozUI>();
            Sound = FindFirstObjectByType<SyzozSoundManager>();
        }
    }
    #endregion
}
