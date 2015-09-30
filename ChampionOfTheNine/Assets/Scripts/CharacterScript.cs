using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract parent class for character scripts
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterControllerScript))]
public abstract class CharacterScript : MonoBehaviour
{
    #region Fields

    [SerializeField]Image healthBar;
    [SerializeField]Image energyBar;
    [SerializeField]Transform groundCheck;
    [SerializeField]LayerMask whatIsGround;

    protected AudioSource audioSource;
    protected Timer gcTimer;
    protected float maxHealth;
    protected float maxEnergy;
    protected float moveSpeed;
    protected float jumpSpeed;
    float health;
    float energy;
    string targetTag;

    Rigidbody2D rbody;

    #endregion

    #region Properties

    /// <summary>
    /// Gets and sets the character's health, setting the health bar appropriately
    /// </summary>
    protected float Health
    {
        get { return health; }
        set
        {
            health = value;

            // Sets health bar if it exists
            if (healthBar != null)
            { healthBar.fillAmount = health / maxHealth; }
        }
    }

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
    {
        get
        {
            return Physics2D.OverlapCircle(groundCheck.position, Constants.GROUND_CHECK_RADIUS, whatIsGround);
        }
    }

    /// <summary>
    /// Gets whether or not the character is on the global cooldown
    /// </summary>
    public bool OnGlobalCooldown
    { get { return gcTimer.IsRunning; } }

    #endregion

    #region Public Methods

    /// <summary>
    /// Damages the character by the given amount
    /// </summary>
    /// <param name="amount">the amount</param>
    public virtual void Damage(float amount)
    {
        // Subtracts from the health
        Health -= amount;
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected virtual void Start()
    {
        Health = maxHealth;
        Energy = maxEnergy;
        audioSource = GetComponent<AudioSource>();
        rbody = GetComponent<Rigidbody2D>();

        // Registers for character controller input
        CharacterControllerScript controller = GetComponent<CharacterControllerScript>();
        controller.Register(Jump, FireMainAbility, FireSecondaryAbility, FirePowerAbility, FireSpecialAbility, Move);
        targetTag = controller.TargetTag;
    }

    /// <summary>
    /// Moves the character using the given movement input
    /// </summary>
    /// <param name="input">the movement input</param>
    protected virtual void Move(float input)
    {
        // Handles horizontal movement
        float movement = input * moveSpeed;
        rbody.velocity = new Vector2(movement, rbody.velocity.y);
    }

    /// <summary>
    /// Makes the character jump
    /// </summary>
    protected virtual void Jump()
    {
        rbody.velocity += new Vector2(0, jumpSpeed);
    }

    /// <summary>
    /// Updates the character
    /// </summary>
    protected virtual void Update()
    {
        gcTimer.Update();
    }

    /// <summary>
    /// Fires a projectile attack
    /// </summary>
    /// <param name="prefab">the projectile prefab</param>
    /// <param name="energyCost">the energy cost of the attack</param>
    /// <returns>the projectile, if one was fired</returns>
    protected virtual ProjScript FireProjectileAttack(GameObject prefab, float energyCost)
    {
        ProjScript projScript = null;
        if (energy >= energyCost)
        { 
            // Creates the projectile
            GameObject projectile = GameObject.Instantiate(prefab);
            projScript = projectile.GetComponent<ProjScript>();
            projScript.Initialize(transform.position, Constants.MousePosition, targetTag);

            // Subtracts energy
            energy -= energyCost;
        }
        return projScript;
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
