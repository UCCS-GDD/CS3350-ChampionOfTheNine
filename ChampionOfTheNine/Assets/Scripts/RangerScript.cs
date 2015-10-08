using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script for a ranger character
/// </summary>
public class RangerScript : CharacterScript
{
    #region Fields

    [SerializeField]GameObject arrow;
    [SerializeField]GameObject pierceArrow;
    [SerializeField]GameObject expArrow;
    [SerializeField]Image expCDBar;
    [SerializeField]Image pierceCDBar;
    [SerializeField]Image pierceBar;
    [SerializeField]Image boostCDBar;
    [SerializeField]Image boostBar;

    Timer pierceShootWindow;
    Timer pierceShootCD;
    Timer pierceAbilityCD;
    Timer expArrowCD;
    Timer boostTimer;
    Timer boostCD;

    float cooldownMult = 1;
    float arrowSpeedMult = 1;
    float arrowDamageMult = 1;
    float energyRegenMult = 1;

    #endregion

    #region Public Methods

    /// <summary>
    /// Calculates the angle at which the character should fire to hit the target position
    /// </summary>
    /// <param name="targetPosition">the target position</param>
    /// <returns>the angle</returns>
    public float GetPredictedShotAngle(Vector2 targetPosition, float arrowSpeed)
    {
        Vector2 disp = (Vector2)fireLocation.position - targetPosition;

        // Calculates equation components
        float speedSquared = Mathf.Pow(arrowSpeed, 2);
        float topSqrt = Mathf.Sqrt(Mathf.Pow(speedSquared, 2) - (Physics2D.gravity.y * ((Physics2D.gravity.y * Mathf.Pow(disp.x, 2)) + 
            (2 * disp.y * speedSquared))));
        float bottom = Physics2D.gravity.y * disp.x;

        // Calculates angles
        float angle1 = Mathf.Atan((speedSquared + topSqrt) / bottom) * Mathf.Rad2Deg;
        float angle2 = Mathf.Atan((speedSquared - topSqrt) / bottom) * Mathf.Rad2Deg;

        // Picks and returns better angle
        if (disp.x > 0)
        { return Mathf.Max(angle1, angle2) + 180; }
        else
        { return Mathf.Min(angle1, angle2); }
    }

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
        expArrowCD = new Timer(Constants.EXP_ARROW_CD);
        boostTimer.Register(HandleBoostTimerFinishing);
        pierceShootWindow.Register(HandlePierceWindowFinishing);

        // Loads sounds
        mainAbilitySound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.RANGER_SHOOT_SND);
        secondaryAbilitySound = mainAbilitySound;
        powerAbilitySound = mainAbilitySound;
        specialAbilitySound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.RANGER_SPECIAL_SND);
        base.Start();
    }

    /// <summary>
    /// Fires a projectile attack
    /// </summary>
    /// <param name="prefab">the projectile prefab</param>
    /// <param name="energyCost">the energy cost of the attack</param>
    /// <param name="cooldown">the cooldown timer to start</param>
    /// <returns>the projectile, if one was fired</returns>
    protected override ProjScript FireProjectileAttack(GameObject prefab, float energyCost, Timer cooldown)
    {
        ProjScript projectile = base.FireProjectileAttack(prefab, energyCost, cooldown);
        if (projectile != null)
        {
            Utilities.PlaySoundPitched(audioSource, mainAbilitySound);
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

        // Simple rangers don't use any of these - change if we give more arrows to AI
        if (!simple)
        {
            // Updates timers
            pierceShootCD.Update();
            pierceAbilityCD.Update();
            pierceShootWindow.Update();
            expArrowCD.Update();
            boostTimer.Update();
            boostCD.Update();

            // Updates cooldown bars
            expCDBar.fillAmount = 1 - (expArrowCD.ElapsedSeconds / expArrowCD.TotalSeconds);
            pierceCDBar.fillAmount = 1 - (pierceAbilityCD.ElapsedSeconds / pierceAbilityCD.TotalSeconds);
            boostCDBar.fillAmount = 1 - (boostCD.ElapsedSeconds / boostCD.TotalSeconds);
            boostBar.fillAmount = 1 - (boostTimer.ElapsedSeconds / boostTimer.TotalSeconds);
            pierceBar.fillAmount = 1 - (pierceShootWindow.ElapsedSeconds / pierceShootWindow.TotalSeconds);
        }
    }

    /// <summary>
    /// Fires the character's main ability
    /// </summary>
    protected override void FireMainAbility() 
    {
        FireProjectileAttack(arrow, Constants.BASIC_ARROW_COST, gcTimer);
    }

    /// <summary>
    /// Fires the character's secondary ability
    /// </summary>
    protected override void FireSecondaryAbility()
    {
        if (!expArrowCD.IsRunning)
        {
            ProjScript projectile = FireProjectileAttack(expArrow, Constants.EXP_ARROW_COST, gcTimer);
            if (projectile != null)
            { expArrowCD.Start(); }
        }
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
            FireProjectileAttack(pierceArrow, Constants.PIERCE_ARROW_COST, pierceShootCD);
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

            audioSource.PlayOneShot(specialAbilitySound);
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
