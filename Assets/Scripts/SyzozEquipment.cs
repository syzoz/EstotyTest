using Unity.VisualScripting;
using UnityEngine;
using Zenject;

/// <summary>
/// Handles character equipment, such as weapons and shooting mechanics.
/// Manages targeting, shooting, and visual rotation of equipped items.
/// </summary>
public class SyzozEquipment : MonoBehaviour
{
    [Header("Renders")]
    public SpriteRenderer leftHand;  // Sprite renderer for the left hand or weapon.
    public SpriteRenderer rightHand; // Sprite renderer for the right hand or weapon.

    [Header("References")]
    public Transform shootPoint;    // The point from where bullets are fired.
    public GameObject Bullet;       // The bullet prefab to instantiate when shooting.

    [HideInInspector] public SyzozPlayer Player;           // The Player Connection for services Input
    [HideInInspector] public float fireRate = 1f;          // Time between consecutive shots.
    [HideInInspector] public float detectionRadius = 5f;   // Radius within which enemies can be detected.
    [HideInInspector] public float shootForce = 10f;       // Speed of the bullet after firing.
    [HideInInspector] public float nextFireTime;           // Tracks the next available time for firing.
    [HideInInspector] public bool InBattle;                // Indicates if the character is in combat.
    [HideInInspector] public SyzozManager Manager;
    [HideInInspector] public SyzozSoundManager Sound;

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
    /// Updates equipment behavior, including enemy detection and shooting logic.
    /// </summary>
    private void Update()
    {
        if(Manager == null)
        {
            Debug.Log("Null");
        }

        // Only execute logic if the game has started.
        if (!Manager.IsStartGame || !Player.Health.isAlive())
            return;

        // Check if there are valid enemies within range and set InBattle accordingly.
        InBattle = IsAllowShoot(out var closestEnemy);

        if (InBattle)
        {
            RotateLeftHand(closestEnemy); // Rotate the left hand towards the closest enemy.

            // Fire a shot if the cooldown has elapsed.
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate; // Reset the fire timer.
            }
        }
        else
        {
            DoRotation(false, false); // Reset the rotation if not in battle.
        }
    }

    /// <summary>
    /// Rotates the left hand sprite to aim at the closest enemy.
    /// </summary>
    /// <param name="closestEnemy">The transform of the closest enemy.</param>
    private void RotateLeftHand(Transform closestEnemy)
    {
        if (closestEnemy != null)
        {
            // Calculate the direction and angle to the closest enemy.
            Vector2 direction = (closestEnemy.position - leftHand.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply rotation to the left hand.
            leftHand.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Flip the sprite if aiming to the left.
            if (angle > 90 || angle < -90)
                DoRotation(true);
            else
                DoRotation(false);
        }
    }

    /// <summary>
    /// Handles the shooting mechanics, including bullet instantiation and movement.
    /// </summary>
    private void Shoot()
    {
        // Instantiate a bullet at the shoot point.
        var bulletInstance = Instantiate(Bullet, shootPoint.position, shootPoint.rotation);
        var bulletRb = bulletInstance.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            // Play Sound Effect Directly
            Sound.PlaySound(SoundType.Meele0);

            // Apply velocity to the bullet.
            bulletRb.velocity = shootPoint.right * shootForce;

            // Parent the bullet to the manager's container.
            bulletRb.transform.SetParent(Manager.containerParent);
        }

        // Destroy the bullet after 3 seconds to prevent clutter.
        Destroy(bulletInstance, 3f);
    }

    /// <summary>
    /// Adjusts the rotation and scale of the character's hands.
    /// </summary>
    /// <param name="isLeft">Indicates whether the character is aiming to the left.</param>
    /// <param name="onceOverrid">Determines whether to reset rotations.</param>
    private void DoRotation(bool isLeft, bool onceOverrid = true)
    {
        if (onceOverrid)
        {
            // Flip the character and hands based on the aiming direction.
            transform.localScale = new Vector3(1 * (isLeft ? -1 : 1), 1, 1);
        }

        leftHand.transform.localScale = new Vector3(1 * (isLeft ? -1 : 1), 1 * (isLeft ? -1 : 1), 1);

        // Reset rotation to default if not overridden.
        if (!onceOverrid)
        {
            leftHand.transform.localEulerAngles = Vector3.zero;
        }
    }

    /// <summary>
    /// Detects if there are valid enemies within the detection radius and identifies the closest one.
    /// </summary>
    /// <param name="closestEnemy">Outputs the transform of the closest enemy if found.</param>
    /// <returns>True if a valid enemy is detected; otherwise, false.</returns>
    public bool IsAllowShoot(out Transform closestEnemy)
    {
        // Detect all colliders within the detection radius.
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        float closestDistance = Mathf.Infinity;

        closestEnemy = null;

        // Iterate through detected objects to find the closest valid enemy.
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                var enemy = hit.GetComponent<SyzozEnemy>();
                if (enemy != null && enemy.Health.isAlive())
                {
                    float distance = Vector2.Distance(transform.position, hit.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = hit.transform; // Update the closest enemy.
                    }
                }
            }
        }

        // Return true if a valid enemy was found, otherwise false.
        return closestEnemy != null;
    }

    /// <summary>
    /// Call When Player is Dead to Disable Hands Weapons
    /// </summary>
    public void Kill()
    {
        leftHand.gameObject.SetActive(false);
        rightHand.gameObject.SetActive(false);
    }

    public void Reload()
    {
        leftHand.gameObject.SetActive(true);
        rightHand.gameObject.SetActive(true);
    }

    public void LookingNull()
    {
        Manager = FindFirstObjectByType<SyzozManager>();
        Sound = FindFirstObjectByType<SyzozSoundManager>();
    }
}
