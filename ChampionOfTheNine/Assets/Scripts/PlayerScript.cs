using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the player
/// </summary>
public class PlayerScript : CharacterControllerScript
{
    #region Protected Methods

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected override void Update()
    {
        // Handles horizontal movement
        movement(Input.GetAxis("Horizontal"));

        // Handles jumping
        if (Input.GetButtonDown("Jump") && character.Grounded)
        { jumpAbility(); }

        // Handles firing
        if (!character.OnGlobalCooldown)
        {
            if (Input.GetAxis("SpecialFire") > 0)
            { specialAbility(); }
            if (Input.GetAxis("MainFire") > 0)
            { mainAbility(); }
            else if (Input.GetAxis("SecondaryFire") > 0)
            { secondaryAbility(); }
            else if (Input.GetAxis("PowerFire") > 0)
            { powerAbility(); }
        }
    }

    #endregion
}
