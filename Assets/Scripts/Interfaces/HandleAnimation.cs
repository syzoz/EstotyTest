using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for handling character animations.
/// Defines a contract for implementing animation updates based on character status.
/// </summary>
public interface SyzozHandleAnimation
{
    /// <summary>
    /// Updates the animation state based on the given character status.
    /// </summary>
    /// <param name="status">The animation status to be applied (e.g., Idle, Run, Dead).</param>
    void HandleAnimation(CharacterAnimation status);
}