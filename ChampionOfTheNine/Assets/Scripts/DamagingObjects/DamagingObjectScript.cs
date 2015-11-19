using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract parent script that controls an object that damages things
/// </summary>
[RequireComponent(typeof(Collider2D))]
public abstract class DamagingObjectScript : PauseableObjectScript
{
    #region Fields

    protected float damage;
    protected string targetTag;
    protected HitType hit = HitType.None;

    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the object
    /// </summary>
    /// <param name="damage">the damage</param>
    /// <param name="targetTag">the tag of the targeted characters</param>
    public virtual void Initialize(float damage, string targetTag)
    {
        base.Initialize();
        this.damage = damage;
        this.targetTag = targetTag;
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Handles the projectile colliding with something
    /// </summary>
    /// <param name="other">the other collider</param>
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // Checks for if enemy
        if (other.gameObject.tag == targetTag)
        {
            other.gameObject.GetComponent<DamagableObjectScript>().Damage(damage);
            hit = HitType.Target;
        }
        else if (other.gameObject.layer == Constants.GROUND_LAYER)
        { hit = HitType.Ground; }
    }

    #endregion
}
