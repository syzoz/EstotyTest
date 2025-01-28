using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a ScriptableObject that stores references to weapon-related prefabs.
/// This includes prefabs for health recovery items and experience orbs.
/// </summary>
[CreateAssetMenu(fileName = "SyzozWeapons", menuName = "Syzoz/SyzozWeapons")]
public class SyzozWeapons : ScriptableObject
{
    public GameObject HealthRecover; // Prefab for health recovery items.
    public GameObject expPrefabe;    // Prefab for experience orbs.
}