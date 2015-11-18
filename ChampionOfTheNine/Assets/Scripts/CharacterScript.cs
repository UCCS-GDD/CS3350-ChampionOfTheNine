using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract parent class for character scripts
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class CharacterScript : DamagableObjectScript
{
    #region Fields

    [SerializeField]protected Transform fireLocation;
    [SerializeField]protected AudioSource walkAudio;
    [SerializeField]Transform groundCheck;
    [SerializeField]LayerMask whatIsGround;
    [SerializeField]GameObject arm;

    protected Rigidbody2D rbody;
    protected Timer secondaryCDTimer;
    protected Timer powerCDTimer;
    protected Timer specialCDTimer;
    protected Timer gCDTimer;
    protected float maxEnergy;
    protected float moveSpeed;
    protected float jumpSpeed;
    protected string targetTag;
    float energy;
    bool controllable = true;

    Animator animator;
    Vector3 baseScale;
    Vector3 flippedScale;

    protected AudioClip jumpSound;
    protected AudioClip landSound;
    protected AudioClip mainAbilitySound;
    protected AudioClip secondaryAbilitySound;
    protected AudioClip powerAbilitySound;
    protected AudioClip specialAbilitySound;

    protected MovementHandler energyChanged = Blank;

    #endregion

    #region Properties

    /// <summary>
    /// Gets and sets the character's energy, setting the energy bar appropriately
    /// </summary>
    protected float Energy
    {
        get { return energy; }
        set
        {
            energy = value;
            energyChanged(energy / maxEnergy);
        }
    }

    /// <summary>
    /// Gets the angle at which to shoot a projectile
    /// </summary>
    protected float ShotAngle
    {
        get
        {
            float shotAngle = arm.transform.rotation.eulerAngles.z;
            if (transform.localScale.x < 0)
            { shotAngle = 180 - shotAngle; }
            return shotAngle;
        }
    }

    /// <summary>
    /// Gets and sets whether or not the character is controllable
    /// </summary>
    protected bool Controllable
    {
        get { return controllable; }
        set
        {
            if (!value)
            { rbody.velocity = Vector2.zero; }
            controllable = value;
        }
    }

    /// <summary>
    /// Gets whether or not the character is grounded
    /// </summary>
    public bool Grounded
    { get { return Physics2D.OverlapCircle(groundCheck.position, Constants.GROUND_CHECK_RADIUS, whatIsGround); } }

    /// <summary>
    /// Gets the character's global cooldown timer
    /// </summary>
    public Timer GCDTimer
    { get { return gCDTimer; } }

    /// <summary>
    /// Gets the character's secondary cooldown timer
    /// </summary>
    public Timer SecondaryCDTimer
    { get { return secondaryCDTimer; } }

    /// <summary>
    /// Gets the character's power cooldown timer
    /// </summary>
    public Timer PowerCDTimer
    { get { return powerCDTimer; } }

    /// <summary>
    /// Gets the character's special cooldown timer
    /// </summary>
    public Timer SpecialCDTimer
    { get { return specialCDTimer; } }

    /// <summary>
    /// Gets the character's arm object
    /// </summary>
    public GameObject Arm
    { get { return arm; } }

    #endregion

    #region Public Methods

    /// <summary>
    /// Updates the character; not called on normal update cycle, called by controller
    /// </summary>
    public virtual void UpdateChar()
    {
        try
        {
            // Updates cooldown timers
            gCDTimer.Update();
            powerCDTimer.Update();
            secondaryCDTimer.Update();
            specialCDTimer.Update();

            animator.SetFloat(Constants.XVELOCTIY_FLAG, Mathf.Abs(rbody.velocity.x));

            // Set jump animation/play sounds
            if (Grounded)
            {
                if (!animator.GetBool(Constants.GROUNDED_FLAG))
                {
                    animator.SetBool(Constants.GROUNDED_FLAG, true);
                    Utilities.PlaySoundPitched(audioSource, landSound);
                }
            }
            else
            {
                if (animator.GetBool(Constants.GROUNDED_FLAG))
                {
                    animator.SetBool(Constants.GROUNDED_FLAG, false);
                    Utilities.PlaySoundPitched(audioSource, jumpSound);
                }
            }
        }
        catch (NullReferenceException) { }
    }

    /// <summary>
    /// Initializes the character script; called from controller
    /// </summary>
    /// <param name="controller">the controller script</param>
    /// <param name="energyChanged">the handler for when the energy changes</param>
    /// <param name="healthBar">the health bar</param>
    /// <param name="timerBars">the array of timer bars</param>
    public virtual void Initialize(CharacterControllerScript controller, MovementHandler energyChanged, Image healthBar, Image[] timerBars)
    {
        // Registers for character controller input
        this.energyChanged = energyChanged;
        controller.Register(Jump, FireMainAbility, FireSecondaryAbility, FirePowerAbility, FireSpecialAbility, Move, SetArmAngle);
        targetTag = controller.TargetTag;

        if (healthBar != null)
        { this.healthBar = healthBar; }
    }

    /// <summary>
    /// Calculates the angle at which an object should be launched to hit the target position
    /// </summary>
    /// <param name="targetPosition">the target position</param>
    /// <param name="objectSpeed">the speed at which the object will be launched</param>
    /// <param name="gravityScale">value by which to scale gravity (defaults to 1)</param>
    /// <returns>the angle</returns>
    public virtual float CalculateLaunchAngle(Vector2 targetPosition, float objectSpeed, float gravityScale = 1)
    {
        Vector2 disp = (Vector2)fireLocation.position - targetPosition;

        // Calculates equation components
        float g = Physics2D.gravity.y * gravityScale;
        float speedSquared = Mathf.Pow(objectSpeed, 2);
        float topSqrt = Mathf.Sqrt(Mathf.Pow(speedSquared, 2) - (g * ((g * Mathf.Pow(disp.x, 2)) + (2 * disp.y * speedSquared))));
        float bottom = g * disp.x;

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
        base.Start();
        Energy = maxEnergy;
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        baseScale = transform.localScale;
        flippedScale = new Vector3(-baseScale.x, baseScale.y, baseScale.z);
        walkAudio.clip = GameManager.Instance.GameSounds[Constants.CHAR_WALK_SND];
        hitSound = GameManager.Instance.GameSounds[Constants.CHAR_HIT_SND];
        deathSound = GameManager.Instance.GameSounds[Constants.CHAR_DEATH_SND];
        jumpSound = GameManager.Instance.GameSounds[Constants.CHAR_JUMP_SND];
        landSound = GameManager.Instance.GameSounds[Constants.CHAR_LAND_SND];
    }

    /// <summary>
    /// Moves the character using the given movement input
    /// </summary>
    /// <param name="input">the movement input</param>
    protected virtual void Move(float input)
    {
        // Plays/stops sound
        if (Mathf.Abs(rbody.velocity.x) > 0 && !walkAudio.isPlaying)
        { walkAudio.Play(); }
        else if (rbody.velocity.x == 0 && walkAudio.isPlaying)
        { walkAudio.Stop(); }

        if (controllable)
        {
            // Handles horizontal movement
            float movement = input * moveSpeed;
            rbody.velocity = new Vector2(movement, rbody.velocity.y);
        }
    }

    /// <summary>
    /// Makes the character jump
    /// </summary>
    protected virtual void Jump()
    {
        if (controllable)
        { rbody.velocity = new Vector2(0, jumpSpeed); }
    }

    /// <summary>
    /// Sets the character's arm angle
    /// </summary>
    protected virtual void SetArmAngle(float angle)
    {
        if (controllable)
        {
            // Flips the character if needed
            float armAngle = angle;
            if (transform.localScale.x > 0 && angle > 90 && angle < 270)
            { transform.localScale = flippedScale; }
            else if (transform.localScale.x < 0)
            {
                armAngle = 180 - armAngle;
                if (angle <= 90 || angle >= 270)
                { transform.localScale = baseScale; }
            }

            arm.transform.rotation = Quaternion.Euler(0, 0, armAngle);
        }
    }

    /// <summary>
    /// Fires a projectile attack straight forward from the character
    /// </summary>
    /// <param name="prefab">the projectile prefab</param>
    /// <param name="energyCost">the energy cost of the attack</param>
    /// <param name="cooldown">the cooldown timer to start</param>
    /// <returns>the projectile, if one was fired</returns>
    protected virtual ProjScript FireStraightProjectileAttack(GameObject prefab, float energyCost, Timer cooldown)
    {
        ProjScript projectile = FireProjectileAttack(prefab, energyCost, cooldown);
        if (projectile != null)
        { projectile.Initialize(fireLocation.position, ShotAngle, targetTag); }
        return projectile;
    }

    /// <summary>
    /// Fires a projectile attack
    /// </summary>
    /// <param name="prefab">the projectile prefab</param>
    /// <param name="energyCost">the energy cost of the attack</param>
    /// <param name="cooldown">the cooldown timer to start</param>
    /// <returns>the projectile, if one was fired</returns>
    protected virtual ProjScript FireProjectileAttack(GameObject prefab, float energyCost, Timer cooldown)
    {
        ProjScript projScript = null;
        if (energy >= energyCost)
        {
            // Creates the projectile
            projScript = Instantiate<GameObject>(prefab).GetComponent<ProjScript>();

            // Subtracts energy and starts timer
            energy -= energyCost;
            cooldown.Start();
        }
        return projScript;
    }

    /// <summary>
    /// Handles the character dying
    /// </summary>
    protected override void Death()
    {
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        try
        { GetComponent<CharacterControllerScript>().Death(); }
        catch (NullReferenceException) { }
    }

    /// <summary>
    /// Fires the character's main ability
    /// </summary>
    protected abstract void FireMainAbility();

    /// <summary>
    /// Fires the character's secondary ability
    /// </summary>
    protected abstract void FireSecondaryAbility();

    /// <summary>
    /// Fires the character's power ability
    /// </summary>
    protected abstract void FirePowerAbility();

    /// <summary>
    /// Fires the character's special ability
    /// </summary>
    protected abstract void FireSpecialAbility();

    #endregion

    private static void Blank(float value) { }
}
