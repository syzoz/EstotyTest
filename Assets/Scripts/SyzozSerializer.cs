using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the stats and attributes for an enemy, including health, speed, and damage.
/// Includes references to the enemy prefab and its associated experience sprite.
/// </summary>
[Serializable]
public class statsEnemy
{
    public float HP;       // The health points of the enemy.
    public float Speed;    // The movement speed of the enemy.
    public float Damage;   // The damage dealt by the enemy.
    public int maxCount;   // The maximum count of this enemy type allowed in the game.

    [Header("Object")]
    public SyzozEnemy Enemy; // Reference to the enemy prefab.
    public Sprite Exp;       // Sprite representing the experience drop associated with this enemy.
}

/// <summary>
/// Stores the configuration for UI panels, including their type and associated panel objects.
/// </summary>
[Serializable]
public class statsCanvas
{
    public UIType Type;       // The type of the UI panel (e.g., Main, Input, InGame).
    public SyzozRect Panel;   // Reference to the UI panel object.
}

[Serializable]
public class statsSound
{
    public SoundType Type;
    public AudioClip clip;
}