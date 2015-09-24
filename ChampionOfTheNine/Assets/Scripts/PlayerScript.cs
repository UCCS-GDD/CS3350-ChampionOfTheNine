using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the player
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioListener))]
public class PlayerScript : CharacterScript
{
    #region Fields

    protected float moveSpeed;
    protected float jumpSpeed;
    [SerializeField]Transform groundCheck;
    [SerializeField]LayerMask whatIsGround;
    [SerializeField]GameObject arrow;

    float energy;
    float maxEnergy;
    [SerializeField]Image energyBar;

    Rigidbody2D rbody;
    bool grounded = false;

    Timer cooldownTimer;

    #endregion

    #region Properties

    /// <summary>
    /// Gets and sets the character's energy, setting the energy bar appropriately
    /// </summary>
    private float Energy
    {
        get { return energy; }
        set
        {
            energy = value;
            energyBar.fillAmount = energy / maxEnergy;
        }
    }

    /// <summary>
    /// Gets the mouse position in world space
    /// </summary>
    public static Vector2 MousePosition
    { get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); } }

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
        energy = maxEnergy;
        rbody = GetComponent<Rigidbody2D>();
        cooldownTimer = new Timer(Constants.BASIC_ARROW_CD);
        base.Start();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // Handles jumping
        float vMovement = 0;
        grounded = Physics2D.OverlapCircle(groundCheck.position, Constants.GROUND_CHECK_RADIUS, whatIsGround);
        if (grounded && Input.GetButtonDown("Jump"))
        { vMovement = jumpSpeed; }

        // Handles horizontal movement
        float hMovement = Input.GetAxis("Horizontal") * moveSpeed;
        rbody.velocity = new Vector2(hMovement, rbody.velocity.y + vMovement);

        // Updates energy
        if (energy < maxEnergy)
        { Energy = Mathf.Min(maxEnergy, energy + (Constants.RANGER_REGEN * Time.deltaTime)); }

        // Handles firing (change some of this later)
        if (Input.GetAxis("Fire1") > 0 && !cooldownTimer.IsRunning && energy >= Constants.BASIC_ARROW_COST)
        {
            // Creates the projectile
            GameObject projectile = GameObject.Instantiate(arrow);
            projectile.GetComponent<ProjScript>().Initialize(transform.position, MousePosition);

            //// Plays sound
            //audio.PlayOneShot(fireSound);

            // Subtracts energy and starts the cooldown
            energy -= Constants.BASIC_ARROW_COST;
            cooldownTimer.Start();
        }
        cooldownTimer.Update();
    }

    #endregion
}
