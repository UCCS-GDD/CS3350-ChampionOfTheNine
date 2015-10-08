using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract parent class for scripts for objects that can be damaged
/// </summary>
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public abstract class DamagableObjectScript : MonoBehaviour
{
    #region Fields

    [SerializeField]Image healthBar;

    protected AudioSource audioSource;
    protected AudioClip hitSound;
    protected AudioClip deathSound;
    protected float maxHealth;
    float health;
    
    #endregion

    #region Properties

    /// <summary>
    /// Gets and sets the object's health, setting the health bar appropriately
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

    #endregion

    #region Public Methods

    /// <summary>
    /// Damages the character by the given amount
    /// </summary>
    /// <param name="amount">the amount</param>
    public virtual void Damage(float amount)
    {
        // Subtracts from the health and plays sound
        Health -= amount;
        Utilities.PlaySoundPitched(audioSource, hitSound);

        // Checks for death
        if (health <= 0)
        { Death(); }
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected virtual void Start()
    {
        Health = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Handles the object dying
    /// </summary>
    protected abstract void Death();

    #endregion
}
