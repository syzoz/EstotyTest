using Unity.VisualScripting;
using UnityEngine;
using Zenject;

/// <summary>
/// Represents an enemy character in the game, handling movement, direction, health, and interactions with the player.
/// Inherits animation handling from SyzozBase.
/// </summary>
public class SyzozEnemy : SyzozBase
{
    [Header("Base Character Components")]
    private SyzozPlayer player;      // Reference to the player for movement and attack targeting.
    private Rigidbody2D rb;         // Rigidbody2D for controlling enemy physics and movement.
    private SpriteRenderer render;  // SpriteRenderer for handling visual direction changes.

    [HideInInspector] public float reloadHit = 0f;       // Timer for hit cooldown.
    [HideInInspector] public bool isFacingRight = true;  // Tracks whether the enemy is facing right.
    [HideInInspector] public bool isTouchPlayer = false; // Tracks whether the enemy is in contact with the player.
    [HideInInspector] public statsEnemy informer;        // Contains stats for the enemy (e.g., speed, health, damage).
    [HideInInspector] public SyzozHealth Health;         // Reference to the enemy's health system.
    [HideInInspector] public SyzozManager Manager;
    [HideInInspector] public SyzozSoundManager Sound;

    #region Default Functions

    /// <summary>
    /// Injection of Zenject
    /// </summary>
    [Inject]
    public void Construct(SyzozManager manager, SyzozSoundManager sound)
    {
        Manager = manager;
        Sound = sound;
    }

    private void Start()
    {
        LookingNull();
    }

    /// <summary>
    /// Handles enemy movement towards the player using Rigidbody2D, if the enemy is alive.
    /// </summary>
    private void FixedUpdate()
    {
        if (!Health.isAlive()) return;

        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * informer.Speed; // Move towards the player based on speed.
        }
    }

    /// <summary>
    /// Handles direction changes, interactions with the player, and hit cooldown.
    /// </summary>
    private void Update()
    {
        if (!Health.isAlive()) return;

        if (informer != null)
        {
            HandleDirection(); // Update the enemy's facing direction.

            // Handle interactions when the enemy is touching the player.
            if (isTouchPlayer)
            {
                if (reloadHit > 0f)
                {
                    reloadHit -= Time.deltaTime; // Reduce hit cooldown timer.
                }
                else
                {
                    ApplyHit();   // Apply damage to the player.
                    reloadHit = 0.2f; // Reset hit cooldown.
                }
            }
        }
    }

    /// <summary>
    /// Handles collision with the player, marking the enemy as touching the player.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!Health.isAlive()) return;

        string tag = collision.gameObject.tag;
        if (tag == "Player")
        {
            isTouchPlayer = true; // Start tracking player contact.
        }
    }

    /// <summary>
    /// Handles exiting collision with the player, clearing the touch flag.
    /// </summary>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!Health.isAlive()) return;

        string tag = collision.gameObject.tag;
        if (tag == "Player")
        {
            isTouchPlayer = false; // Stop tracking player contact.
        }
    }

    /// <summary>
    /// Handles collision with bullets, triggering hit animations and damage.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Health.isAlive()) return;

        if (collision.tag == "Bullet")
        {
            Destroy(collision.gameObject);                // Destroy the bullet.
            HandleAnimation(CharacterAnimation.Hit);      // Play hit animation.
            TakeDamage();                                 // Apply damage to the enemy.
        }
    }

    #endregion

    /// <summary>
    /// Sets up the enemy's stats, references, and initial state.
    /// </summary>
    /// <param name="informer">The stats and data for this enemy.</param>
    /// <param name="player">Reference to the player.</param>
    public void SetUpEnemy(statsEnemy informer, SyzozPlayer player)
    {
        rb = GetComponent<Rigidbody2D>();               // Cache Rigidbody2D.
        render = GetComponent<SpriteRenderer>();        // Cache SpriteRenderer.

        this.informer = informer;                       // Set enemy stats.
        this.player = player;                           // Set player reference.
        this.Health.health = informer.HP;               // Initialize enemy health.
        this.rb.bodyType = RigidbodyType2D.Dynamic;     // Enable dynamic physics.
    }

    /// <summary>
    /// Handles flipping the enemy's direction based on player position.
    /// </summary>
    private void HandleDirection()
    {
        if (informer != null)
        {
            if (player.transform.position.x > transform.position.x && !isFacingRight ||
                player.transform.position.x < transform.position.x && isFacingRight)
            {
                isFacingRight = !isFacingRight;           // Flip facing direction.
                render.flipX = !isFacingRight;            // Flip the sprite.
            }
        }
    }

    /// <summary>
    /// Reduces the enemy's health, triggers death logic if health reaches zero.
    /// </summary>
    private void TakeDamage()
    {
        Health.TakeDamage(10); // Apply damage to health.

        Sound.PlaySound(SoundType.Hit1); // Play Sound Effect Directly
        if (Health.health <= 0)
        {
            Manager.KillEnemy(this); // Notify the manager of the enemy's death.
            Sound.PlaySound(SoundType.Dead); // Play Sound Effect Directly
        }
    }

    /// <summary>
    /// Applies damage to the player when the enemy is in contact.
    /// </summary>
    public void ApplyHit()
    {
        Sound.PlaySound(SoundType.Hit0); // Play Sound Effect Directly
        player.Health.TakeDamage(informer.Damage); // Reduce the player's health.
        player.LoadAnimationHited();
    }

    /// <summary>
    /// Handles the enemy's death animation and cleanup.
    /// </summary>
    public void Kill()
    {
        HandleAnimation(CharacterAnimation.Dead);  // Play death animation.
        Health.Kill();                             // Mark health as depleted.
        rb.bodyType = RigidbodyType2D.Static;      // Disable movement.
        Destroy(gameObject, 2f);                   // Destroy the enemy after 2 seconds.

        // Disable Colliders when its Dead to not Conflit With others also for Collision purpose
        Collider2D[] colds = GetComponentsInChildren<Collider2D>();
        foreach (var cold in colds)
            cold.enabled = false;
    }

    /// <summary>
    /// Force Destroy The Enemy When Its Over
    /// </summary>
    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void LookingNull()
    {
        Manager = FindFirstObjectByType<SyzozManager>();
        Sound = FindFirstObjectByType<SyzozSoundManager>();
    }

    /// <summary>
    /// Returns the stats of the enemy.
    /// </summary>
    /// <returns>The statsEnemy object associated with this enemy.</returns>
    public statsEnemy TypeEnemy()
    {
        return informer;
    }
}
