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
    [SerializeField]GameObject expArrow;

    #endregion

    #region Properties

    

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected override void Start()
    {
        // Change this later
        maxHealth = Constants.RANGER_HEALTH;
        moveSpeed = Constants.RANGER_MOVE_SPEED;
        jumpSpeed = Constants.RANGER_JUMP_SPEED;
        maxEnergy = Constants.RANGER_ENERGY;
        gcTimer = new Timer(Constants.RANGER_GCD);
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
    }

    /// <summary>
    /// Fires the character's main ability
    /// </summary>
    protected override void FireMainAbility() 
    {
        FireProjectileAttack(arrow, Constants.BASIC_ARROW_COST);
    }

    /// <summary>
    /// Fires the character's secondary ability
    /// </summary>
    protected override void FireSecondaryAbility()
    {
        FireProjectileAttack(expArrow, Constants.EXP_ARROW_COST);
    }

    /// <summary>
    /// Fires the character's power ability
    /// </summary>
    protected override void FirePowerAbility()
    {

    }

    /// <summary>
    /// Fires the character's special ability
    /// </summary>
    protected override void FireSpecialAbility()
    {

    }

    #endregion
}
