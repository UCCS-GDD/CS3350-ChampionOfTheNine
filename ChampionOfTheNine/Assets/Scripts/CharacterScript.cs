using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract parent class for character scripts
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterControllerScript))]
public abstract class CharacterScript : DamagableObjectScript
{
    #region Fields

    [SerializeField]protected Transform fireLocation;
    [SerializeField]protected AudioSource walkAudio;
    [SerializeField]Image energyBar;
    [SerializeField]Image[] gcdBars;
    [SerializeField]Transform groundCheck;
    [SerializeField]LayerMask whatIsGround;
    [SerializeField]GameObject arm;

    protected Timer gcTimer;
    protected float maxEnergy;
    protected float moveSpeed;
    protected float jumpSpeed;
    float energy;
    string targetTag;

    Rigidbody2D rbody;
    Animator animator;

    protected AudioClip jumpSound;
    protected AudioClip landSound;
    protected AudioClip mainAbilitySound;
    protected AudioClip secondaryAbilitySound;
    protected AudioClip powerAbilitySound;
    protected AudioClip specialAbilitySound;

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

            // Sets energy bar if it exists
            if (energyBar != null)
            { energyBar.fillAmount = energy / maxEnergy; }
        }
    }

    /// <summary>
    /// Gets whether or not the character is grounded
    /// </summary>
    public bool Grounded
    { get { return Physics2D.OverlapCircle(groundCheck.position, Constants.GROUND_CHECK_RADIUS, whatIsGround); } }

    /// <summary>
    /// Gets the character's global cooldown
    /// </summary>
    public Timer GCD
    { get { return gcTimer; } }

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
        if (gcTimer.IsRunning)
        {
            gcTimer.Update();
            foreach (Image bar in gcdBars)
            { bar.fillAmount = 1 - (gcTimer.ElapsedSeconds / gcTimer.TotalSeconds); }
        }
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
        walkAudio.clip = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.CHAR_WALK_SND);
        hitSound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.CHAR_HIT_SND);
        deathSound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.CHAR_DEATH_SND);
        jumpSound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.CHAR_JUMP_SND);
        landSound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.CHAR_LAND_SND);

        // Registers for character controller input
        CharacterControllerScript controller = GetComponent<CharacterControllerScript>();
        controller.Register(Jump, FireMainAbility, FireSecondaryAbility, FirePowerAbility, FireSpecialAbility, Move, SetArmAngle);
        targetTag = controller.TargetTag;
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

        // Handles horizontal movement
        float movement = input * moveSpeed;
        rbody.velocity = new Vector2(movement, rbody.velocity.y);
    }

    /// <summary>
    /// Makes the character jump
    /// </summary>
    protected virtual void Jump()
    {
        rbody.velocity = new Vector2(0, jumpSpeed);
    }

    /// <summary>
    /// Sets the character's arm angle
    /// </summary>
    protected virtual void SetArmAngle(float angle)
    {
        // Flips the character if needed
        float armAngle = angle;
        if (transform.localScale.x != -1 && angle > 90 && angle < 270)
        { transform.localScale = new Vector3(-1, 1, 1); }
        else if (transform.localScale.x == -1)
        {
            armAngle = 180 - armAngle;
            if (angle <= 90 || angle >= 270)
            { transform.localScale = new Vector3(1, 1, 1); }
        }

        arm.transform.rotation = Quaternion.Euler(0, 0, armAngle);
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
            GameObject projectile = GameObject.Instantiate(prefab);
            projScript = projectile.GetComponent<ProjScript>();
            float shotAngle = arm.transform.rotation.eulerAngles.z;
            if (transform.localScale.x < 0)
            { shotAngle = 180 - shotAngle; }
            projScript.Initialize(fireLocation.position, shotAngle, targetTag);

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
        GetComponent<CharacterControllerScript>().Death();
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
}
