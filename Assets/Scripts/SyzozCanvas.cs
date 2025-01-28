using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a ScriptableObject that stores references to different UI panels.
/// Provides functionality to retrieve and instantiate specific UI panels.
/// </summary>
[CreateAssetMenu(fileName = "SyzozCanvas", menuName = "Syzoz/SyzozCanvas")]
public class SyzozCanvas : ScriptableObject
{
    public statsCanvas[] Panels; // Array of all available UI panels, each associated with a specific UI type.

    /// <summary>
    /// Retrieves and instantiates a UI panel of the specified type as a child of the given parent transform.
    /// </summary>
    /// <typeparam name="T">The component type to retrieve from the instantiated panel.</typeparam>
    /// <param name="Type">The type of the panel to retrieve.</param>
    /// <param name="parent">The parent transform to attach the instantiated panel to.</param>
    /// <returns>The component of type T from the instantiated panel, or the default value if not found.</returns>
    public T GetPanel<T>(UIType Type, Transform parent)
    {
        // Iterate through the list of panels to find one matching the specified type.
        foreach (var panel in Panels)
        {
            if (panel.Type == Type)
            {
                // Instantiate the panel as a child of the parent transform and return the specified component.
                return Instantiate(panel.Panel, parent).GetComponent<T>();
            }
        }

        // Return the default value (null for reference types) if no matching panel is found.
        return default;
    }
}