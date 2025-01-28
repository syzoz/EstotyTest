using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for character behaviors, implementing animation handling.
/// Provides a unified way to handle character animations using an Animator component.
/// </summary>
public class SyzozBase : MonoBehaviour, SyzozHandleAnimation
{
    public Animator thisanim; // Reference to the Animator component controlling the character's animations.

    /// <summary>
    /// Handles animations for the character based on the specified animation status.
    /// </summary>
    /// <param name="status">The character's current animation status (e.g., Dead, Idle, Run, Hit).</param>
    public void HandleAnimation(CharacterAnimation status)
    {
        switch (status)
        {
            case CharacterAnimation.Dead:
                // Play the "Dead" animation and set the "Dead" boolean parameter to true.
                thisanim.Play("Dead");
                thisanim.SetBool("Dead", true);
                break;

            case CharacterAnimation.Idle:
                // Play the "Idle" animation.
                thisanim.Play("Idle");
                break;

            case CharacterAnimation.Run:
                // Play the "Run" animation.
                thisanim.Play("Run");
                break;

            case CharacterAnimation.Hit:
                // Trigger the "Hit" animation once.
                thisanim.SetTrigger("Hit");
                break;
        }
    }
}