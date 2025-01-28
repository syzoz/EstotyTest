//// Boot Loader For The GamePlay
using UnityEngine;
using Zenject;

// This class serves as the main entry point for initializing the game's essential systems and managers.
public class SyzozBoot : MonoBehaviour
{
    #region Variables
    [Header("Managers")]
    private SyzozManager      Manager; /// Manager Holder Data References
    private SyzozPlayer       Player;  /// Player Controller
    private SyzozWorld        World;   /// World Level Based On List Of Levels
    private SyzozCamera       Camera;  /// Camera Controller For The Player
    private SyzozMain         Main;    /// Main UI For Canvas as Parent For otehr sub Panels Canvas
    private SyzozSoundManager Sound; /// Sound Controller for the gamePlay

    [Header("Transforms")]
    public Transform ContainerUI; /// Container for Spawned Panels UI
    #endregion

    #region Functions
    [Inject]
    public void Construct(SyzozAddresable Syzoz, SyzozWeapons Weapons, SyzozActors Actors, SyzozCanvas Panels)
    {

        // Check if all the essential assets are successfully loaded.
        if (Syzoz != null && Weapons != null && Actors != null && Panels != null)
        {
            // Instantiate and initialize key game components.
            Manager = Instantiate(Syzoz.Manager);                            // Creates the main game manager.
            Player  = Instantiate(Actors.GetPlayer());                       // Creates the player instance.
            World   = Instantiate(Syzoz.GetMap());                           // Creates the game world/level.
            Camera  = Instantiate(Syzoz.Camera);                             // Creates the camera instance.
            Sound   = Instantiate(Syzoz.Sound);                              // Creates the Sound main Manager
            Main    = Panels.GetPanel<SyzozMain>(UIType.Main, ContainerUI);  // Creates the main UI panel.

            // Initialize the instantiated components with their dependencies.
            Manager.Init(Actors, Weapons, Player);           // Initializes the game manager with essential data.
            Main.Init(Panels, ContainerUI);                  // Sets up the main UI with its sub-panels.
            Camera.Init(Player);                             // Configures the camera to follow the player.
        }
        else
        {
            // Log an error if any of the required assets are missing.
            Debug.LogError("Not Found Asset Data Syzoz");
        }
    }
    #endregion
}