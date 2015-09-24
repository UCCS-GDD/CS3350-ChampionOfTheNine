using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls exploding ranger arrows
/// </summary>
public class ExpArrowScript : PlayerProjScript
{
    #region Fields

    [SerializeField]GameObject explosion;

    #endregion

    #region Public Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fromPosition"></param>
    /// <param name="toPosition"></param>
    public override void Initialize(Vector2 fromPosition, Vector2 toPosition)
    {
        moveSpeed = Constants.EXP_ARROW_SPEED;
        damage = Constants.EXP_ARROW_DAMAGE;
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
        { 
            Destroy(gameObject);
            Instantiate(explosion, transform.position, transform.localRotation);
        }
    }

    #endregion
}
