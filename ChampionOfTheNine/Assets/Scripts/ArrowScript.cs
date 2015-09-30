using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls basic ranger arrows
/// </summary>
public class ArrowScript : ProjScript
{
    #region Protected Methods

    /// <summary>
    /// Initializes the projectile
    /// </summary>
    /// <param name="targetTag">the tag of the targeted characters</param>
    protected override void Initialize(string targetTag)
    {
        moveSpeed = Constants.BASIC_ARROW_SPEED;
        damage = Constants.BASIC_ARROW_DAMAGE;
        base.Initialize(targetTag);
    }

    /// <summary>
    /// Handles the projectile colliding with something
    /// </summary>
    /// <param name="other">the other collider</param>
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (hit != HitType.None)
        { Destroy(gameObject); }
    }

    #endregion
}
