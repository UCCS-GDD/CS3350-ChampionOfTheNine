using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls basic ranger arrows
/// </summary>
public class ArrowScript : ProjScript
{
    #region Fields



    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the projectile
    /// </summary>
    /// <param name="fromPosition">the position of the projectile</param>
    /// <param name="toPosition">the target position</param>
    /// <param name="targetTag">the tag of the targeted characters</param>
    public override void Initialize(Vector2 fromPosition, Vector2 toPosition, string targetTag)
    {
        moveSpeed = Constants.BASIC_ARROW_SPEED;
        damage = Constants.BASIC_ARROW_DAMAGE;
        base.Initialize(fromPosition, toPosition, targetTag);
    }

    #endregion

    #region Protected Methods

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
