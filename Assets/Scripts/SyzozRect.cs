using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides basic utility functions for managing the visibility and lifecycle of a GameObject.
/// Acts as a base class for UI elements or other objects that need show, hide, and destroy functionality.
/// </summary>
public class SyzozRect : MonoBehaviour
{
    /// <summary>
    /// Activates the GameObject, making it visible and interactable.
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true); // Set the GameObject's active state to true.
    }

    /// <summary>
    /// Deactivates the GameObject, hiding it from view and disabling interaction.
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false); // Set the GameObject's active state to false.
    }

    /// <summary>
    /// Destroys the GameObject, removing it permanently from the scene.
    /// </summary>
    public void Destroy()
    {
        Destroy(gameObject); // Destroy the GameObject and release its resources.
    }
}