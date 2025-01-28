/// <summary>
/// Represents the type of a character in the game.
/// </summary>
public enum CharacterType
{
    AI,      // An enemy AI-controlled character.
    Player,  // The player's character.
    Friend   // A friendly AI or ally.
}

/// <summary>
/// Defines the type of joystick behavior.
/// </summary>
public enum JoystickType
{
    Follow,  // The joystick follows the touch point on the screen.
    Fixed    // The joystick remains in a fixed position on the screen.
}

/// <summary>
/// Defines the types of joystick animations available.
/// </summary>
public enum JoystickAnim
{
    Fade,      // Joystick fades in and out.
    Static,    // No animation applied.
    Scale,     // Joystick scales in and out.
    FadeScale  // Combines fade and scale animations.
}

/// <summary>
/// Represents the status of a character during gameplay.
/// </summary>
public enum CharacterStatus
{
    Dead,     // The character is dead and cannot act.
    Survive,  // The character is surviving but in critical condition.
    Alive     // The character is fully alive and active.
}

/// <summary>
/// Defines the types of animations that a character can play.
/// </summary>
public enum CharacterAnimation
{
    Dead,   // Death animation.
    Hit,    // Animation for when the character takes damage.
    Walk,   // Walking animation.
    Run,    // Running animation.
    Idle    // Idle (standing still) animation.
}

/// <summary>
/// Represents the types of UI panels in the game.
/// </summary>
public enum UIType
{
    Main,    // Main menu panel.
    Input,   // Input-related panel, such as a joystick.
    InGame,  // In-game UI panel (e.g., stats, health bars).
    Finish   // Finish Panel UI In Game
}

public enum SoundType
{
    Dead,
    Hit0,
    Hit1,
    LevelUp,
    Lose,
    Meele0,
    Meele1,
    Range,
    Select,
    Win
}