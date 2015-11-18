using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Script for a warrior character
/// </summary>
public class WarriorScript : CharacterScript
{
    #region Fields



    #endregion

    #region Public Methods

    /// <summary>
    /// Updates the character; not called on normal update cycle, called by controller
    /// </summary>
    public override void UpdateChar()
    {
        base.UpdateChar();

        // Checks for leap finishing
        if (!Controllable && Grounded)
        {
            Controllable = true;
        }
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected override void Start()
    {
        // Sets fields
        maxHealth = Constants.WARRIOR_HEALTH;
        moveSpeed = Constants.WARRIOR_MOVE_SPEED;
        jumpSpeed = Constants.WARRIOR_JUMP_SPEED;
        maxEnergy = Constants.WARRIOR_ENERGY;
        gCDTimer = new Timer(Constants.WARRIOR_GCD);
        secondaryCDTimer = new Timer(Constants.LIGHTNING_CD);
        powerCDTimer = new Timer(Constants.LEAP_CD);
        specialCDTimer = new Timer(Constants.DRAIN_CD);

        // Loads sounds
        mainAbilitySound = GameManager.Instance.GameSounds[Constants.ICE_CAST_SND];
        secondaryAbilitySound = GameManager.Instance.GameSounds[Constants.LIGHTNING_CAST_SND];
        powerAbilitySound = GameManager.Instance.GameSounds[Constants.METEOR_CAST_SND];
        specialAbilitySound = GameManager.Instance.GameSounds[Constants.DRAIN_SND];
        base.Start();
    }

    /// <summary>
    /// Fires the character's main ability
    /// </summary>
    protected override void FireMainAbility()
    {

    }

    /// <summary>
    /// Fires the character's secondary ability
    /// </summary>
    protected override void FireSecondaryAbility()
    {

    }

    /// <summary>
    /// Fires the character's power ability
    /// </summary>
    protected override void FirePowerAbility()
    {
        if (!gCDTimer.IsRunning && !powerCDTimer.IsRunning)
        {
            Controllable = false;
            powerCDTimer.Start();
            gCDTimer.Start();

            float leapAngle = CalculateLaunchAngle(Utilities.MousePosition, Constants.LEAP_SPEED, Constants.CHAR_GRAV_SCALE);
            //transform.localRotation = Quaternion.Euler(0, 0, leapAngle);
            rbody.velocity = new Vector2(Mathf.Cos(leapAngle * Mathf.Deg2Rad) * Constants.LEAP_SPEED, Mathf.Sin(leapAngle * Mathf.Deg2Rad) * Constants.LEAP_SPEED);
        }
    }

    /// <summary>
    /// Fires the character's special ability
    /// </summary>
    protected override void FireSpecialAbility()
    {

    }

    #endregion
}
