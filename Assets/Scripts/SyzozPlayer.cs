using Unity.VisualScripting;
using UnityEngine;
using Zenject;

/// <summary>
/// Handles player movement, health, animations, and interactions in the game.
/// </summary>
public class SyzozPlayer : SyzozBase
{
    [Header("Speed Movement")]
    public float speed = 5f; // Speed of the player's movement.

    [HideInInspector] public Rigidbody2D    thisRb;       // Rigidbody2D component for handling physics-based movement.
    [HideInInspector] public SyzozHealth    Health;       // Reference to the player's health system.
    [HideInInspector] public SyzozEquipment Equiper;      // Reference to the player's equipped items and battle state.
    [HideInInspector] public SyzozManager   Manager;
    [HideInInspector] public SyzozUI        syzozUI;

    #region Default Functions
    /// <summary>
    /// Injection of Zenject
    /// </summary>
    [Inject]
    public void Construct(SyzozManager manager, SyzozUI uiManager)
    {
        Manager = manager;
        syzozUI = uiManager;
    }

    private void Start()
    {
        LookingNull();
    }

    /// <summary>
    /// Handles player movement, direction, and animations during the fixed update cycle.
    /// </summary>
    private void FixedUpdate()
    {
        // Proceed only if the game has started.
        if (Manager.IsStartGame)
        {
            Health.HealthHandler(); // Manage the player's health (e.g., regeneration, damage checks).

            // Check if the player is alive.
            if (Health.isAlive())
            {
                HandleMove();       // Handle player movement based on joystick input.
                HandleDirection();  // Update the player's facing direction.
            }
            else
            {
                HandleAnimation(CharacterAnimation.Dead); // Play the death animation if the player is not alive.
                Kill();
            }
        }
    }

    /// <summary>
    /// Handles collisions with recoverable items to restore health.
    /// </summary>
    /// <param name="collision">Collision data from the physics system.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with an object on the "RecoverItem" layer.
        if (collision.gameObject.layer == LayerMask.NameToLayer("RecoverItem"))
        {
            Health.RecoverHealth();  // Restore the player's health.
            Destroy(collision.gameObject); // Remove the recoverable item from the game.
        }
    }

    #endregion

    /// <summary>
    /// Handles player movement based on joystick input.
    /// Adjusts the player's velocity and triggers appropriate animations.
    /// </summary>
    private void HandleMove()
    {
        thisRb.velocity = JoystickInput.Input * speed; // Apply movement input to the player's Rigidbody2D.

        // Check if there is movement input.
        if (JoystickInput.Input != Vector3.zero)
        {
            HandleAnimation(CharacterAnimation.Run); // Play the running animation.
        }
        else
        {
            HandleAnimation(CharacterAnimation.Idle); // Play the idle animation if there's no movement.
        }
    }

    /// <summary>
    /// Updates the player's facing direction based on movement input.
    /// Skips direction updates if the player is in battle mode.
    /// </summary>
    private void HandleDirection()
    {
        // Skip direction handling if the player is in battle mode.
        if (Equiper.InBattle)
            return;

        float horizontalInput = JoystickInput.Input.x;

        // Face right if moving right.
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        // Face left if moving left.
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    /// <summary>
    /// Reload The Player
    /// </summary>
    public void ActivatePlayer()
    {
        transform.position = Vector3.zero;
        transform.localScale = Vector3.one;

        Health.RecoverHealth();
        Equiper.Reload();
        thisRb.bodyType = RigidbodyType2D.Dynamic;
    }

    /// <summary>
    /// Showing The Wave Animaiton Red When Player Get Touched By The Enemys
    /// </summary>
    public void LoadAnimationHited()
    {
        LookingNull();

        syzozUI.ApplyHited();
    }

    /// <summary>
    /// Placeholder for player kill logic, to be implemented as needed.
    /// </summary>
    public void Kill()
    {
        Manager.KillPlayer();
        Equiper.Kill();
        thisRb.bodyType = RigidbodyType2D.Static;
    }

    public void LookingNull()
    {
        if(Manager == null || syzozUI == null)
        {
            Manager = FindFirstObjectByType<SyzozManager>();
            syzozUI = FindFirstObjectByType<SyzozUI>();
        }
    }
}
