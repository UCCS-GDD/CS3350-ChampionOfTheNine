﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls menu UI
/// </summary>
public class MenuUIScript : MonoBehaviour
{
    #region Fields

    string surveyLink = "";

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles the play button being pressed
    /// </summary>
    public void PlayButtonPressed()
    {
        Application.LoadLevel(Constants.CHAR_CREATE_SCENE);
        //// Go to tutorial if no save, otherwise go to map
        //if (GameManager.Instance.Saves.Count == 0)
        //{ 
        //    //Application.LoadLevel(Constants.TUTORIAL_SCENE);
        //    Application.LoadLevel(Constants.CHAR_CREATE_SCENE);
        //}
        //else
        //{ Application.LoadLevel(Constants.MAP_SCENE); }
    }

    /// <summary>
    /// Handles the quit button being pressed
    /// </summary>
    public void QuitButtonPressed()
    {
        Application.Quit();
    }

    /// <summary>
    /// Handles the back button being pressed
    /// </summary>
    public void BackButtonPressed()
    {
        Application.LoadLevel(Constants.MAIN_MENU_SCENE);
    }

    /// <summary>
    /// Handles the tutorial button being pressed
    /// </summary>
    public void TutorialButtonPressed()
    {
        Application.LoadLevel(Constants.TUTORIAL_SCENE);
    }

    /// <summary>
    /// Handles the survey button being pressed
    /// </summary>
    public void SurveyButtonPressed()
    {
        Application.OpenURL(surveyLink);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    private void Start()
    {
        GameManager.Instance.Paused = false;
    }

    ///// <summary>
    ///// Update is called once per frame
    ///// </summary>
    //private void Update()
    //{

    //}

    #endregion
}
