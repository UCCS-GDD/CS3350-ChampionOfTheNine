using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract parent script that controls characters
/// </summary>
[RequireComponent(typeof(AudioSource))]
public abstract class CharacterScript : MonoBehaviour
{
    #region Fields

    [SerializeField]protected float maxHealth;
    protected float health;
    protected AudioSource audioSource;

    public Image healthBar;

    #endregion

    #region Properties

    /// <summary>
    /// Gets and sets the character's health, setting the health bar appropriately
    /// </summary>
    private float Health
    {
        get { return health; }
        set
        {
            health = value;
            healthBar.fillAmount = health / maxHealth;
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
        audioSource = GetComponent<AudioSource>();
    }

    #endregion
}
