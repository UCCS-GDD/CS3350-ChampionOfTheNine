using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls an explosion
/// </summary>
public class ExplosionScript : PauseableObjectScript
{
    #region Fields

    float damage;
    string targetTag;

    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the explosion
    /// </summary>
    /// <param name="damage">the damage</param>
    /// <param name="targetTag">the tag of the targeted characters</param>
    public void Initialize(float damage, string targetTag)
    {
        base.Initialize();
        this.damage = damage;
        this.targetTag = targetTag;
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Updates the object while it isn't paused
    /// </summary>
    protected override void NotPausedUpdate()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Finished"))
        { Destroy(gameObject); }
    }

    /// <summary>
    /// Handles the explosion colliding with something
    /// </summary>
    /// <param name="other">the other collider</param>
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // Checks for if enemy
        if (other.gameObject.tag == targetTag)
        { other.gameObject.GetComponent<DamagableObjectScript>().Damage(damage); }
    }

    #endregion
}
