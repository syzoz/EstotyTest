using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the game's UI elements, including health, experience, kill count, and level display.
/// Inherits from SyzozInstance to implement a singleton pattern for global access.
/// </summary>
public class SyzozUI : SyzozRect
{
    public GameObject hitedWave; // UI For Hitted Player Animation

    public Slider Health;        // UI slider representing the player's health percentage.
    public Slider Exp;           // UI slider representing the player's experience progress.

    public Text killedUI; // Text UI element displaying the number of enemies killed.
    public Text levelUI;  // Text UI element displaying the current player level.

    /// <summary>
    /// Updates the health slider value based on the player's current health.
    /// </summary>
    /// <param name="value">The current health value of the player.</param>
    public void UpdateHealth(float value)
    {
        float tPercentage = value / 100f; // Calculate the percentage (assuming max health is 100).
        Health.value = tPercentage;      // Set the slider value.
    }

    /// <summary>
    /// Updates the experience slider value.
    /// </summary>
    /// <param name="value">The normalized experience value (0 to 1).</param>
    public void UpdateExp(float value)
    {
        Exp.value = value; // Set the slider value for experience.
    }

    /// <summary>
    /// Updates the kill count UI element with the provided number.
    /// </summary>
    /// <param name="count">The total number of enemies killed.</param>
    public void UpdateKilled(int count)
    {
        killedUI.text = count.ToString(); // Display the kill count as text.
    }

    /// <summary>
    /// Updates the level display UI element with the current level.
    /// </summary>
    /// <param name="level">The player's current level.</param>
    public void UpdateLevel(int level)
    {
        levelUI.text = "Lv." + level; // Format the level as "Lv.X" and display it.
    }

    /// <summary>
    /// Show The Animation Using Courotine Delay For the user when he got Hitted
    /// </summary>
    public void ApplyHited()
    {
        ShowHited(true); // Activate the Object
        StartCoroutine(LoadingBack());
        IEnumerator LoadingBack()
        {
            yield return new WaitForSeconds(1f); // Wait For Second Untile Disable Them
            ShowHited(false);
        }
    }

    /// <summary>
    /// Enable Disable the Hited Wave Effect
    /// </summary>
    /// <param name="Status"></param>
    public void ShowHited(bool Status)
    {
        hitedWave.SetActive(Status);
    }
}