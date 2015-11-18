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

    [SerializeField]GameObject explosion;
    [SerializeField]GameObject axe;
    float leapTargetX;
    bool leaping = false;
    bool hasAxe = true;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets whether or not the warrior is leaping
    /// Also sets if the warrior is controllable
    /// </summary>
    public bool Leaping
    {
        get { return leaping; }
        set 
        { 
            leaping = value;
            Controllable = !value;
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Updates the character; not called on normal update cycle, called by controller
    /// </summary>
    public override void UpdateChar()
    {
        base.UpdateChar();

        // Checks for leap finishing
        if (Leaping)
        {
            if (Mathf.Abs(transform.position.x - leapTargetX) < Constants.LEAP_TARGET_WINDOW && rbody.velocity.x != 0)
            {
                rbody.velocity = Vector2.down * 10;
            }
            else if (Grounded && !gCDTimer.IsRunning)
            {
                Leaping = false;
                ((GameObject)Instantiate(explosion, transform.position, transform.rotation)).GetComponent<ExplosionScript>().Initialize(Constants.LEAP_DAMAGE, targetTag);
            }
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
        if (hasAxe && !Leaping && !gCDTimer.IsRunning && !secondaryCDTimer.IsRunning)
        {
            FireStraightProjectileAttack(axe, Constants.AXE_ENERGY, gCDTimer, Constants.AXE_DAMAGE, Constants.AXE_SPEED);
            hasAxe = false;
            secondaryCDTimer.Start();
            secondaryCDTimer.IsRunning = false;
        }
    }

    /// <summary>
    /// Fires the character's power ability
    /// </summary>
    protected override void FirePowerAbility()
    {
        if (!gCDTimer.IsRunning && !powerCDTimer.IsRunning)
        {
            float leapAngle = Utilities.CalculateLaunchAngle(transform.position, Utilities.MousePosition, Constants.LEAP_SPEED, Constants.CHAR_GRAV_SCALE);
            if (!float.IsNaN(leapAngle))
            {
                Leaping = true;
                powerCDTimer.Start();
                gCDTimer.Start();

                leapTargetX = Utilities.MousePosition.x;
                //transform.localRotation = Quaternion.Euler(0, 0, leapAngle);
                rbody.velocity = new Vector2(Mathf.Cos(leapAngle * Mathf.Deg2Rad) * Constants.LEAP_SPEED, 
                    Mathf.Sin(leapAngle * Mathf.Deg2Rad) * Constants.LEAP_SPEED);
            }
        }
    }

    /// <summary>
    /// Fires the character's special ability
    /// </summary>
    protected override void FireSpecialAbility()
    {

    }

    /// <summary>
    /// Handles when the warrior enters a collision
    /// </summary>
    /// <param name="collision">the collsion</param>
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        // Picks up the axe pickup
        if (!hasAxe && collision.gameObject.tag == Constants.AXE_PICKUP_TAG)
        {
            hasAxe = true;
            Destroy(collision.gameObject);
            secondaryCDTimer.IsRunning = true;
        }
    }

    #endregion
}
