using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

/// <summary>
/// Manages the health system for a character, including damage, recovery, and health-related status updates.
/// </summary>
public class SyzozHealth : MonoBehaviour
{
    [HideInInspector] public float health = 100f;                 // The current health value of the character.
    [HideInInspector] public CharacterStatus status = CharacterStatus.Alive; // The current status of the character (e.g., Alive, Dead).
    [HideInInspector] public SyzozUI syzozUI;

    /// <summary>
    /// Injection of Zenject
    /// </summary>
    /// <param name="soundManager"></param>
    [Inject]
    public void Construct(SyzozUI soundManager)
    {
        syzozUI = soundManager;
    }

    private void Start()
    {
        LookingNull();
    }

    /// <summary>
    /// Reduces the character's health by the specified amount of damage.
    /// Sets health to 0 if it falls below or equals zero.
    /// </summary>
    /// <param name="power">The amount of damage to apply.</param>
    public void TakeDamage(float power)
    {
        if (health > 0)
            health -= power;

        if (health <= 0)
        {
            health = 0f; // Clamp health to 0 to prevent negative values.
            Kill(); // Do Killing For the Player
        }
    }

    /// <summary>
    /// Marks the character as dead by updating their status.
    /// </summary>
    public void Kill()
    {
        health = 0f;
        status = CharacterStatus.Dead; // Set status to Dead.
    }

    /// <summary>
    /// Fully restores the character's health to its maximum value.
    /// </summary>
    public void RecoverHealth()
    {
        health = 100f; // Reset health to its maximum value.
        status = CharacterStatus.Alive;
    }

    /// <summary>
    /// Updates the health UI to reflect the current health value.
    /// </summary>
    public void HealthHandler()
    {
        LookingNull();
        syzozUI.UpdateHealth(health); // Update the health UI using the SyzozUI instance.
    }

    public void LookingNull()
    {
        if (syzozUI == null)
            syzozUI = FindFirstObjectByType<SyzozUI>();
    }

    /// <summary>
    /// Checks if the character is alive.
    /// </summary>
    /// <returns>True if the character is alive, otherwise false.</returns>
    public bool isAlive()
    {
        return status == CharacterStatus.Alive; // Return true if the character is marked as Alive.
    }
}
