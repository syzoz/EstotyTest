using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

/// <summary>
/// Manages the main UI panel and initializes other UI panels for the game.
/// Handles the transition to gameplay by setting up required UI components.
/// </summary>
public class SyzozMain : SyzozRect
{
    private SyzozCanvas Panels;       // Reference to the canvas containing all UI panels.
    private SyzozInput  Input;        // Reference to the input panel for player controls.
    private SyzozUI     SyzozUI;      // Reference to the in-game UI for displaying game stats and information.
    private SyzozFinish Finish;
    private SyzozManager Manager;
    private SyzozSoundManager Sound;

    private Transform ContainerUI;    // Container for dynamically instantiated UI panels.

    /// <summary>
    /// Injection of Zenject
    /// </summary>
    [Inject]
    public void Construct(SyzozManager manager, SyzozSoundManager sound)
    {
        Manager = manager;
        Sound = sound;
    }

    private void Start()
    {
        LookingNull();
    }

    /// <summary>
    /// Initializes the main UI with references to the panel manager and container for spawned panels.
    /// </summary>
    /// <param name="panels">The SyzozCanvas object managing all UI panels.</param>
    /// <param name="container">The container for dynamically created UI elements.</param>
    public void Init(SyzozCanvas panels, Transform container)
    {
        Panels      = panels;             // Store the panel manager reference.
        ContainerUI = container;          // Store the container reference.
    }

    /// <summary>
    /// Starts the game by ensuring required UI panels are active and initializes the gameplay.
    /// </summary>
    public void StartGame()
    {
        Sound.PlaySound(SoundType.Select); // Play Sound Effect Directly

        // If the input or in-game UI panels are not already initialized, instantiate and initialize them.
        if (Input == null || SyzozUI == null)
        {
            Input   = Panels.GetPanel<SyzozInput>(UIType.Input, ContainerUI); // Load and initialize the input panel.
            SyzozUI = Panels.GetPanel<SyzozUI>(UIType.InGame, ContainerUI);   // Load and initialize the in-game UI panel.
        }
        else
        {
            Input.Show();    // Show the input panel if it is already initialized.
            SyzozUI.Show();  // Show the in-game UI panel if it is already initialized.
        }

        // Notify the SyzozManager to start the game logic.
        Manager.StartGame();

        // Hide the main UI panel after the game starts.
        Hide();
    }

    /// <summary>
    /// Show Finish Panel Based On Curernt Status theres only GameOver cause theres no Goals on Game Endless Based Game On Waves As Described In Document
    /// </summary>
    public void ShowFinish()
    {
        Finish = Panels.GetPanel<SyzozFinish>(UIType.Finish, ContainerUI);
    }

    /// <summary>
    /// Show The Reload Setting When the user reload the Gameplay
    /// </summary>
    public void Reload()
    {
        Finish.Hide();
        SyzozUI.ShowHited(false);
    }

    public void LookingNull()
    {
        Sound = FindFirstObjectByType<SyzozSoundManager>();
        Manager = FindFirstObjectByType<SyzozManager>();
    }
}
