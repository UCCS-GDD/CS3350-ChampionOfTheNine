using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Script for a mage character
/// </summary>
public class MageScript : CharacterScript
{
    #region Fields

    [SerializeField]GameObject ice;
    [SerializeField]GameObject meteor;
    [SerializeField]GameObject lightning;

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected override void Start()
    {
        // Needs changing to mage
        // Sets fields
        maxHealth = Constants.RANGER_HEALTH;
        moveSpeed = Constants.RANGER_MOVE_SPEED;
        jumpSpeed = Constants.RANGER_JUMP_SPEED;
        maxEnergy = Constants.RANGER_ENERGY;
        gCDTimer = new Timer(Constants.RANGER_GCD);
        powerCDTimer = new Timer(Constants.PIERCE_ABILITY_CD);
        specialCDTimer = new Timer(Constants.RANGER_BOOST_CD);
        secondaryCDTimer = new Timer(Constants.EXP_ARROW_CD);

        // Loads sounds
        mainAbilitySound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.RANGER_SHOOT_SND);
        secondaryAbilitySound = mainAbilitySound;
        powerAbilitySound = mainAbilitySound;
        specialAbilitySound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.RANGER_SPECIAL_SND);
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
        
    }

    /// <summary>
    /// Fires the character's special ability
    /// </summary>
    protected override void FireSpecialAbility()
    {

    }

    #endregion
}
