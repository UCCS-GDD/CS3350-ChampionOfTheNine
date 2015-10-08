﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls menu UI
/// </summary>
public class UIScript : MonoBehaviour
{
    #region Fields



    #endregion

    #region Public Methods

    /// <summary>
    /// Handles the play button being pressed
    /// </summary>
    public void PlayButtonPressed()
    {
        Application.LoadLevel("DynamicLevel");
    }

    #endregion

    #region Private Methods

    ///// <summary>
    ///// Start is called once on object creation
    ///// </summary>
    //private void Start()
    //{

    //}

    ///// <summary>
    ///// Update is called once per frame
    ///// </summary>
    //private void Update()
    //{

    //}

    #endregion
}
