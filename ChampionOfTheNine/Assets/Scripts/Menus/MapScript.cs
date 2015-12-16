using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls MapScript
/// </summary>
public class MapScript : MenuUIScript
{
    #region Fields

    [SerializeField]KingdomButtonScript[] kingdomButtons;

    #endregion

    #region Public Methods



    #endregion

    #region Protected Methods



    #endregion

    #region Properties



    #endregion

    #region Private Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < kingdomButtons.Length; i++)
        { kingdomButtons[i].Initialize(i); }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {

    }

    #endregion
}
