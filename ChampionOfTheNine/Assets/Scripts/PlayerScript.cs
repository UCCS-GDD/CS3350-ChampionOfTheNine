using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the player
/// </summary>
public class PlayerScript : CharacterControllerScript
{
    #region Properties

    /// <summary>
    /// Returns the tag of this character's target
    /// </summary>
    public override string TargetTag
    { get { return Constants.ENEMY_TAG; } }

    #endregion

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

        // Handles arm movement
        Vector2 mousePosition = Constants.MousePosition;
        float armAngle = Mathf.Asin((mousePosition.y - character.Arm.transform.position.y) / Vector2.Distance(mousePosition, character.Arm.transform.position));
        if (mousePosition.x - character.Arm.transform.position.x < 0)
        { armAngle = Mathf.PI - armAngle; }
        armDirection(armAngle * Mathf.Rad2Deg);

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
