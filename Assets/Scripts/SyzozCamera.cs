using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the camera behavior in the game, including movement, zoom animations, and screen shaking.
/// The camera follows the player while staying within defined boundaries.
/// </summary>
public class SyzozCamera : MonoBehaviour
{
    #region Variables

    [HideInInspector] public SyzozPlayer Player; // Reference to the player that the camera will follow.
    [HideInInspector] public Camera cam;         // Reference to the camera component.
    [HideInInspector] public float smoothSpeed = 5f;  // Speed of the camera's smooth movement.
    [HideInInspector] public float ortographSize = 5f; // Default orthographic size of the camera.
    [HideInInspector] public Vector3 offset;     // Offset between the player and the camera.
    [HideInInspector] public Vector2 minSize;    // Minimum boundary for camera movement.
    [HideInInspector] public Vector2 maxSize;    // Maximum boundary for camera movement.

    #endregion

    #region Default Functions

    /// <summary>
    /// Initializes the camera with a reference to the player and sets the default orthographic size.
    /// </summary>
    /// <param name="player">The player that the camera will follow.</param>
    public void Init(SyzozPlayer player)
    {
        Player = player;                          // Assign the player reference.
        cam.orthographicSize = ortographSize - 1; // Set the initial orthographic size for a smooth transition.
    }

    /// <summary>
    /// Called at a fixed time interval, updates camera movement, animations, and shaking effects.
    /// </summary>
    private void FixedUpdate()
    {
        if (Player == null) return; // Exit if the player reference is missing.

        HandleMove();      // Adjust camera position to follow the player.
        HandleAnimation(); // Manage zoom animations based on input.
        HandleShake();     // Apply screen shake effects (if implemented).
    }

    #endregion

    /// <summary>
    /// Moves the camera smoothly towards the player's position while respecting the defined boundaries.
    /// </summary>
    private void HandleMove()
    {
        // Calculate the target position based on player position and offset.
        Vector3 targetPosition = Player.transform.position + offset;

        // Clamp the camera's position within the defined minimum and maximum bounds.
        targetPosition.x = Mathf.Clamp(targetPosition.x, minSize.x, maxSize.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minSize.y, maxSize.y);

        // Smoothly move the camera towards the target position.
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Adjusts the camera's orthographic size for zoom-in and zoom-out animations based on user input.
    /// </summary>
    private void HandleAnimation()
    {
        // Check if the input is pressed or released.
        bool isDown = JoystickInput.IsDown;

        // Zoom out when the input is released.
        if (!isDown && cam.orthographicSize > ortographSize - 1)
        {
            cam.orthographicSize -= Time.deltaTime * smoothSpeed;
        }

        // Zoom in when the input is pressed.
        if (isDown && cam.orthographicSize < ortographSize)
        {
            cam.orthographicSize += Time.deltaTime * smoothSpeed;
        }
    }

    /// <summary>
    /// Placeholder for implementing screen shake effects.
    /// </summary>
    private void HandleShake()
    {
        // Logic for screen shake can be implemented here.
    }
}
