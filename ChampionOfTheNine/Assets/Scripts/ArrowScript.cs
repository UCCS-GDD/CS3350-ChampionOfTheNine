using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls basic ranger arrows
/// </summary>
public class ArrowScript : PlayerProjScript
{
    #region Fields



    #endregion

    #region Public Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fromPosition"></param>
    /// <param name="toPosition"></param>
    public override void Initialize(Vector2 fromPosition, Vector2 toPosition)
    {
        moveSpeed = Constants.BASIC_ARROW_SPEED;
        damage = Constants.BASIC_ARROW_DAMAGE;
        base.Initialize(fromPosition, toPosition);
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
        if (hit)
        { Destroy(gameObject); }
    }

    #endregion
}
