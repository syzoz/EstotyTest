using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Represents a joystick control that handles player input via touch or pointer events.
/// Supports animations and different joystick types.
/// </summary>
public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform Handle;      // The movable part of the joystick (the "stick").
    public RectTransform JoystickBg;  // The background of the joystick.
    public RectTransform Background;  // Optional additional background (not currently used).

    private Image handleImage;        // Image component of the handle.
    private Image joystickImage;      // Image component of the joystick background.

    public JoystickType Type;         // Type of joystick (e.g., static, follow, etc.).
    public JoystickAnim Anim;         // Animation type for the joystick.

    private bool NeedUpdate;          // Determines whether the joystick needs to update its animation.

    /// <summary>
    /// Initializes joystick components and stores initial values for animations.
    /// </summary>
    private void Start()
    {
        // Cache image components for the handle and joystick background.
        joystickImage               = JoystickBg.GetComponent<Image>();
        handleImage                 = Handle.GetComponent<Image>();

        JoystickInput.Joystick      = joystickImage.color;
        JoystickInput.JoystickScale = joystickImage.rectTransform.sizeDelta;
        JoystickInput.Handle        = handleImage.color;
        JoystickInput.HandleScale   = handleImage.rectTransform.sizeDelta;

        NeedUpdate = true; // Start with the animation update enabled.
    }

    /// <summary>
    /// Updates the joystick animations each frame.
    /// </summary>
    private void Update()
    {
        Animation();
    }

    /// <summary>
    /// Handles pointer down events to activate the joystick and set its position (for follow type).
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Type == JoystickType.Follow)
        {
            // Move the joystick background to the pointer's position for "Follow" mode.
            JoystickBg.position = eventData.position;
        }

        // Reset joystick input data.
        JoystickInput.radius = 0f;
        JoystickInput.IsDown = true;
        JoystickInput.Input  = Vector2.zero;

        // Handle the initial drag behavior.
        OnDrag(eventData);

        NeedUpdate = true; // Enable animation updates.
    }

    /// <summary>
    /// Handles drag events to update joystick input and handle position.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnDrag(PointerEventData eventData)
    {
        // Calculate the joystick's radius and input vector.
        var radius = JoystickBg.sizeDelta.x / 2f;
        var input  = Vector2.ClampMagnitude(eventData.position - (Vector2)JoystickBg.position, radius);

        // Update handle position and input values.
        Handle.anchoredPosition = input;
        JoystickInput.Input     = input / radius;
        JoystickInput.radius    = input.magnitude / radius;
    }

    /// <summary>
    /// Handles pointer up events to reset joystick input and position.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        // Reset joystick input data.
        JoystickInput.Input  = Vector2.zero;
        JoystickInput.IsDown = false;
        JoystickInput.radius = 0f;

        NeedUpdate = true; // Enable animation updates.
    }

    /// <summary>
    /// Handles joystick animations for fade, scale, or fade-scale effects.
    /// </summary>
    public void Animation()
    {
        if (NeedUpdate)
        {
            switch (Anim)
            {
                case JoystickAnim.Static:
                    // No animation for the static mode.
                    break;

                case JoystickAnim.Scale:
                case JoystickAnim.Fade:
                case JoystickAnim.FadeScale:
                    var isModified = false; // Tracks whether an animation change occurred.
                    var tFade = JoystickInput.anim / JoystickInput.animSpeed;

                    // Handle animation progress for pressing and releasing the joystick.
                    if (JoystickInput.IsDown && JoystickInput.anim < JoystickInput.animSpeed)
                    {
                        JoystickInput.anim += Time.deltaTime;
                        isModified = true;
                    }
                    if (!JoystickInput.IsDown && JoystickInput.anim > 0)
                    {
                        JoystickInput.anim -= Time.deltaTime;
                        isModified = true;

                        // Smoothly move the handle back to the center.
                        Handle.anchoredPosition = Vector2.Lerp(Handle.anchoredPosition, Vector2.zero, tFade);
                    }

                    if (isModified)
                    {
                        // Handle fade animation (opacity adjustment).
                        if (Anim == JoystickAnim.FadeScale || Anim == JoystickAnim.Fade)
                        {
                            joystickImage.color = Color.Lerp(Color.clear, JoystickInput.Joystick, tFade);
                            handleImage.color   = Color.Lerp(Color.clear, JoystickInput.Handle, tFade);
                        }

                        // Handle scale animation (size adjustment).
                        if (Anim == JoystickAnim.FadeScale || Anim == JoystickAnim.Scale)
                        {
                            handleImage.rectTransform.sizeDelta   = Vector3.Lerp(JoystickInput.HandleScale / 5, JoystickInput.HandleScale, tFade);
                            joystickImage.rectTransform.sizeDelta = Vector3.Lerp(JoystickInput.JoystickScale / 1.1f, JoystickInput.JoystickScale, tFade);
                        }
                    }

                    // Disable updates if no animation changes occurred.
                    if (!isModified)
                        NeedUpdate = false;

                    break;
            }
        }
    }
}
