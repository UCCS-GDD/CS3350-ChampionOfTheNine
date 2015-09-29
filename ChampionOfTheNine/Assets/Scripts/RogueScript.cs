using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script for a rogue character
/// </summary>
public class RogueScript : CharacterScript
{
    #region Fields

    [SerializeField]GameObject arrow;
    [SerializeField]GameObject pierceArrow;
    [SerializeField]GameObject expArrow;

    Timer pierceShootWindow;
    Timer pierceShootCD;
    Timer pierceAbilityCD;

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected override void Start()
    {
        // Sets fields
        maxHealth = Constants.RANGER_HEALTH;
        moveSpeed = Constants.RANGER_MOVE_SPEED;
        jumpSpeed = Constants.RANGER_JUMP_SPEED;
        maxEnergy = Constants.RANGER_ENERGY;
        gcTimer = new Timer(Constants.RANGER_GCD);
        pierceShootWindow = new Timer(Constants.PIERCE_SHOOT_WINDOW);
        pierceShootCD = new Timer(Constants.PIERCE_SHOOT_CD);
        pierceAbilityCD = new Timer(Constants.PIERCE_ABILITY_CD);
        pierceShootWindow.Register(HandlePierceWindowFinishing);
        base.Start();
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected override void Update()
    {
        base.Update();

        // Updates energy
        if (Energy < maxEnergy)
        { Energy = Mathf.Min(maxEnergy, Energy + (Constants.RANGER_REGEN * Time.deltaTime)); }

        // Updates timers
        pierceShootCD.Update();
        pierceAbilityCD.Update();
        pierceShootWindow.Update();
    }

    /// <summary>
    /// Fires the character's main ability
    /// </summary>
    protected override void FireMainAbility() 
    {
        FireProjectileAttack(arrow, Constants.BASIC_ARROW_COST);
        gcTimer.Start();
    }

    /// <summary>
    /// Fires the character's secondary ability
    /// </summary>
    protected override void FireSecondaryAbility()
    {
        FireProjectileAttack(expArrow, Constants.EXP_ARROW_COST);
        gcTimer.Start();
    }

    /// <summary>
    /// Fires the character's power ability
    /// </summary>
    protected override void FirePowerAbility()
    {
        // Fires piercing arrow ability if possible
        if (!pierceAbilityCD.IsRunning && !pierceShootCD.IsRunning)
        {
            // Starts window if this is the first shot
            if (!pierceShootWindow.IsRunning)
            { pierceShootWindow.Start(); }

            // Fires arrow
            FireProjectileAttack(pierceArrow, Constants.PIERCE_ARROW_COST);
            pierceShootCD.Start();
        }
    }

    /// <summary>
    /// Fires the character's special ability
    /// </summary>
    protected override void FireSpecialAbility()
    {

    }

    /// <summary>
    /// Handles the pierce ability window finishing
    /// </summary>
    protected void HandlePierceWindowFinishing()
    {
        pierceAbilityCD.Start();
    }

    #endregion
}
