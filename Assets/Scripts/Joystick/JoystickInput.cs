using UnityEngine;

/// <summary>
/// Static class for managing joystick input and animation parameters.
/// Stores the state of joystick input, scaling, animations, and appearance.
/// </summary>
public class JoystickInput
{
    public static Vector3 Input;            // Current input direction from the joystick (normalized vector).
    public static Vector3 HandleScale;      // Original scale of the joystick handle for animations.
    public static Vector3 JoystickScale;    // Original scale of the joystick background for animations.
    public static bool IsDown;              // Indicates whether the joystick is currently being pressed.
    public static float animSpeed = 0.2f;   // Speed of joystick animations (e.g., fade or scale transitions).
    public static float anim = 0.00001f;    // Current animation progress (used for interpolation).
    public static float radius = 0f;        // Normalized radius of the joystick input (distance from center).
    public static Color Joystick;           // Original color of the joystick background for fade animations.
    public static Color Handle;             // Original color of the joystick handle for fade animations.
}