using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the game world and its associated map data.
/// This class is responsible for managing the world state and identifying the current map.
/// </summary>
public class SyzozWorld : MonoBehaviour
{
    private string mapId = ""; // Unique identifier for the current map.

    /// <summary>
    /// Constructor to initialize the world with a specific map ID.
    /// </summary>
    /// <param name="mapId">The unique identifier for the map.</param>
    public SyzozWorld(string mapId)
    {
        this.mapId = mapId; // Set the map ID for the world.
    }
}