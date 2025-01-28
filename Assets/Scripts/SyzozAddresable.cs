using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a ScriptableObject that stores addressable references for key game components.
/// This includes the camera, manager, and world maps.
/// </summary>
[CreateAssetMenu(fileName = "SyzozAddresable", menuName = "Syzoz/SyzozAddresable")]
public class SyzozAddresable : ScriptableObject
{
    public SyzozCamera  Camera;     // Reference to the camera controller.
    public SyzozManager Manager;    // Reference to the main game manager.
    public SyzozSoundManager Sound; // Reference to the sound manager
    public SyzozWorld[] Maps;       // Array of available world maps.

    /// <summary>
    /// Retrieves the world map corresponding to the player's saved level.
    /// </summary>
    /// <returns>The world map associated with the player's current level.</returns>
    public SyzozWorld GetMap()
    {
        // Get the player's current level from PlayerPrefs and return the corresponding map.
        // Assumes "Lvmap" stores the index of the map.
        return Maps[PlayerPrefs.GetInt("Lvmap")];
    }
}
