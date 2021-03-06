﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Script that controls the save menu
/// </summary>
public class SaveMenuScript : PauseMenuWFilesScript
{
    #region Fields

    public InputField inputField;   // The menu's input field

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles the save button being pressed
    /// </summary>
    public void SaveButtonPressed()
    {
        // Checks if a level name has been chosen
        if (GameManager.Instance.CurrentSaveName != "")
        {
            // Hides the save menu
            gameObject.SetActive(false);

            // Saves the level
            GameManager.Instance.CreateNewSavegame(CharacterType.Ranger, audioSource);
        }
    }

    /// <summary>
    /// Handles the input field value being changed
    /// </summary>
    /// <param name="input">the input</param>
    public void HandleInputValueChanged(string input)
    {
        GameManager.Instance.CurrentSaveName = input;
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Handles clicking on a level file option
    /// </summary>
    /// <param name="value">the new value</param>
    protected override void LevelFileValueChanged(bool value)
    {
        base.LevelFileValueChanged(value);

        // Updates the input field text
        inputField.text = GameManager.Instance.CurrentSaveName;
    }

    #endregion
}
