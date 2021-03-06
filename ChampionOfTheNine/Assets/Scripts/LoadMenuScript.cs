﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Script that controls the load menu
/// </summary>
public class LoadMenuScript : PauseMenuWFilesScript
{
    #region Public Methods

    /// <summary>
    /// Handles the load button being pressed
    /// </summary>
    public void LoadButtonPressed()
    {
        // Checks if a level has been chosen
        if (GameManager.Instance.CurrentSaveName != "")
        {
            // Hides the load menu
            gameObject.SetActive(false);

            // Loads the level
            GameManager.Instance.LoadLevel(Constants.MAP_SCENE, audioSource);
        }
    }

    /// <summary>
    /// Handles the delete file button being pressed
    /// </summary>
    public void DeleteFileButtonPressed()
    {
        audioSource.PlayOneShot(clickSound);
        // Checks if a level has been chosen
        if (GameManager.Instance.CurrentSaveName != "")
        {
            // Hides the load menu
            gameObject.SetActive(false);

            // Deletes the level
            GameManager.Instance.DeleteSave();
            GameManager.Instance.CurrentSaveName = "";
        }
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Updates the menu on enable
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();

        // Resets the level name
        GameManager.Instance.CurrentSaveName = "";
    }

    #endregion
}
