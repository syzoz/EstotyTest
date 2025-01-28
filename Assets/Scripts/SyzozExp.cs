using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

/// <summary>
/// Handles the behavior of experience orbs that are collected by the player.
/// The orb initially pushes away from the player before being pulled toward them for collection.
/// </summary>
public class SyzozExp : MonoBehaviour
{
    private Transform Player; // Reference to the player's transform.

    [HideInInspector] public float pushSpeed = 10f;       // Speed of the orb during the push-away phase.
    [HideInInspector] public float pullSpeed = 50f;       // Speed of the orb during the pull-toward phase.
    [HideInInspector] public float pushDuration = 0.2f;   // Duration of the push-away phase.
    [HideInInspector] public float destroyDistance = 0.5f; // Distance at which the orb is collected and destroyed.
    [HideInInspector] public SyzozSoundManager Sound;

    private bool isBeingPulled = false; // Tracks whether the orb is in the pull-toward phase.

    /// <summary>
    /// Injection of Zenject
    /// </summary>
    [Inject]
    public void Construct(SyzozSoundManager sound)
    {
        Sound = sound;
    }

    private void Start()
    {
        LookingNull();
    }

    /// <summary>
    /// Initializes the orb behavior and begins the push-away phase.
    /// </summary>
    /// <param name="player">The player's transform to which the orb will be attracted.</param>
    public void StartExp(Transform player)
    {
        Player = player;              // Set the player's transform.
        StartCoroutine(PushAway());   // Start the push-away phase.
    }

    /// <summary>
    /// Updates the orb's position during the pull-toward phase.
    /// </summary>
    private void FixedUpdate()
    {
        // Exit if the player is null or the orb is not in the pull phase.
        if (Player == null || !isBeingPulled) return;

        // Calculate direction to the player and move toward them.
        Vector3 directionToPlayer = (Player.position - transform.position).normalized;
        transform.position += directionToPlayer * pullSpeed * Time.fixedDeltaTime;

        // Destroy the orb if it reaches the player.
        if (Vector3.Distance(transform.position, Player.position) <= destroyDistance)
        {
            // Play Sound Effect Directly
            Sound.PlaySound(SoundType.Range);

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Pushes the orb away from the player for a brief duration, then transitions to the pull phase.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    private IEnumerator PushAway()
    {
        // Calculate a direction away from the player with slight randomization.
        Vector3 directionAwayFromPlayer = (transform.position - Player.position).normalized;
        directionAwayFromPlayer.x += Random.Range(-1, 1);
        directionAwayFromPlayer.y += Random.Range(-1, 1);

        float timer = 0f;

        // Move the orb away from the player for the push duration.
        while (timer < pushDuration)
        {
            transform.position += directionAwayFromPlayer * pushSpeed * Time.fixedDeltaTime;
            timer += Time.fixedDeltaTime;
            yield return null;
        }

        // Switch to the pull-toward phase after the push duration ends.
        isBeingPulled = true;
    }

    public void LookingNull()
    {
        Sound = FindFirstObjectByType<SyzozSoundManager>();
    }
}
