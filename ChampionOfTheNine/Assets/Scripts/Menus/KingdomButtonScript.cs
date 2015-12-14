using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls a kingdom button
/// </summary>
public class KingdomButtonScript : MonoBehaviour
{
    #region Fields


    #endregion

    #region Properties

    public KingdomName Kingdom
    { get; set; }

    #endregion

    #region Protected Methods



    #endregion

    #region Private Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    private void Start()
    {
        // Replace interactable part of this code later
        Button buttonScript = GetComponent<Button>();
        //buttonScript.interactable = Kingdom == KingdomName.One;
        buttonScript.onClick.AddListener(Pressed);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {

    }

    /// <summary>
    /// Handles the kingdom button being pressed
    /// </summary>
    private void Pressed()
    {
        GameManager.Instance.LoadGameLevel(Kingdom);
    }

    #endregion
}
