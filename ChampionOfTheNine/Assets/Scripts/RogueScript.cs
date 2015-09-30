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
    Timer boostTimer;
    Timer boostCD;

    float cooldownMult = 1;
    float arrowSpeedMult = 1;
    float arrowDamageMult = 1;
    float energyRegenMult = 1;

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
        boostTimer = new Timer(Constants.RANGER_BOOST_TIME);
        boostCD = new Timer(Constants.RANGER_BOOST_CD);
        boostTimer.Register(HandleBoostTimerFinishing);
        pierceShootWindow.Register(HandlePierceWindowFinishing);
        base.Start();
    }

    /// <summary>
    /// Fires a projectile attack
    /// </summary>
    /// <param name="prefab">the projectile prefab</param>
    /// <param name="energyCost">the energy cost of the attack</param>
    /// <returns>the projectile, if one was fired</returns>
    protected override ProjScript FireProjectileAttack(GameObject prefab, float energyCost)
    {
        ProjScript projectile = base.FireProjectileAttack(prefab, energyCost);
        if (projectile != null)
        {
            projectile.ChangeDamage(arrowDamageMult);
            projectile.ChangeSpeed(arrowSpeedMult);
        }
        return projectile;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected override void Update()
    {
        base.Update();

        // Updates energy
        if (Energy < maxEnergy)
        { Energy = Mathf.Min(maxEnergy, Energy + (Constants.RANGER_REGEN * energyRegenMult * Time.deltaTime)); }

        // Updates timers
        pierceShootCD.Update();
        pierceAbilityCD.Update();
        pierceShootWindow.Update();
        boostTimer.Update();
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
        if (!boostCD.IsRunning)
        {
            // Change multipliers
            moveSpeed = Constants.RANGER_MOVE_SPEED * Constants.RANGER_BOOST_MOVE_MULT;
            jumpSpeed = Constants.RANGER_JUMP_SPEED * Constants.RANGER_BOOST_JUMP_MULT;
            cooldownMult = Constants.RANGER_BOOST_CD_MULT;
            arrowSpeedMult = Constants.RANGER_BOOST_ARROW_SPEED_MULT;
            arrowDamageMult = Constants.RANGER_BOOST_ARROW_DAMAGE_MULT;
            energyRegenMult = Constants.RANGER_BOOST_ENERGY_REGEN_MULT;

            boostTimer.Start();
            boostCD.Start();
        }
    }

    /// <summary>
    /// Handles the pierce ability window finishing
    /// </summary>
    protected void HandlePierceWindowFinishing()
    {
        pierceAbilityCD.Start();
    }

    /// <summary>
    /// Handles the boost timer finishing
    /// </summary>
    protected void HandleBoostTimerFinishing()
    {
        // Change multipliers
        moveSpeed = Constants.RANGER_MOVE_SPEED;
        jumpSpeed = Constants.RANGER_JUMP_SPEED;
        cooldownMult = 1;
        arrowSpeedMult = 1;
        arrowDamageMult = 1;
        energyRegenMult = 1;
    }

    /// <summary>
    /// Resets the cooldown timer lengths
    /// </summary>
    protected void UpdateTimerLengths()
    {
        gcTimer.TotalSeconds = Constants.RANGER_GCD * cooldownMult;
        pierceShootCD.TotalSeconds = Constants.PIERCE_SHOOT_CD * cooldownMult;
        pierceAbilityCD.TotalSeconds = Constants.PIERCE_ABILITY_CD * cooldownMult;
    }

    #endregion
}
