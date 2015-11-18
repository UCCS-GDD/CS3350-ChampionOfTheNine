using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the player
/// </summary>
public class PlayerScript : CharacterControllerScript
{
    #region Fields

    Image energyBar;
    Image[] gcdBars;
    Image secondaryCDBar;
    Image powerCDBar;
    Image specialCDBar;

    #endregion

    #region Properties

    /// <summary>
    /// Returns the tag of this character's target
    /// </summary>
    public override string TargetTag
    { get { return Constants.ENEMY_TAG; } }

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles the character dying
    /// </summary>
    public override void Death()
    {
        WorldScript.Instance.Defeat(gameObject.tag);
        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<SpriteRenderer>());
        Destroy(this);
        Destroy(character.Arm);
    }

    /// <summary>
    /// Initializes the player
    /// </summary>
    public virtual void Initialize(Image healthBar, Image energyBar, Image[] gcdBars, Image[] timerBars, Image secondaryCDBar, Image powerCDBar, Image specialCDBar)
    {
        this.energyBar = energyBar;
        base.Initialize(healthBar, timerBars);
        this.gcdBars = gcdBars;
        this.secondaryCDBar = secondaryCDBar;
        this.powerCDBar = powerCDBar;
        this.specialCDBar = specialCDBar;
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected override void NotPausedUpdate()
    {
        base.NotPausedUpdate();

        // Handles horizontal movement
        movement(Input.GetAxis("Horizontal"));

        // Handles jumping
        if (Input.GetButtonDown("Jump") && character.Grounded)
        { jumpAbility(); }

        // Handles arm movement
        armDirection(Utilities.GetAngleDegrees(character.Arm.transform.position, Utilities.MousePosition));

        // Handles firing
        if (Input.GetAxis("SpecialFire") > 0)
        { specialAbility(); }
        if (!character.GCDTimer.IsRunning && Input.GetAxis("PowerFire") > 0)
        { powerAbility(); }
        if (!character.GCDTimer.IsRunning && Input.GetAxis("SecondaryFire") > 0)
        { secondaryAbility(); }
        if (!character.GCDTimer.IsRunning && Input.GetAxis("MainFire") > 0)
        { mainAbility(); }

        // Moves the camera
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y + 3, Camera.main.transform.position.z);

        // Updates cooldown bars
        foreach (Image bar in gcdBars)
        { bar.fillAmount = 1 - (character.GCDTimer.ElapsedSeconds / character.GCDTimer.TotalSeconds); }
        secondaryCDBar.fillAmount = 1 - (character.SecondaryCDTimer.ElapsedSeconds / character.SecondaryCDTimer.TotalSeconds);
        powerCDBar.fillAmount = 1 - (character.PowerCDTimer.ElapsedSeconds / character.PowerCDTimer.TotalSeconds);
        specialCDBar.fillAmount = 1 - (character.SpecialCDTimer.ElapsedSeconds / character.SpecialCDTimer.TotalSeconds);
    }

    /// <summary>
    /// Handles the character's energy level changing
    /// </summary>
    /// <param name="pct">the percentage of energy the player has</param>
    protected override void CharacterEnergyChanged(float pct)
    {
        energyBar.fillAmount = pct;
    }

    #endregion
}
