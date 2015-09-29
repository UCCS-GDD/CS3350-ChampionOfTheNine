using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls exploding ranger arrows
/// </summary>
public class ExpArrowScript : ProjScript
{
    #region Fields

    [SerializeField]GameObject explosion;

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
        moveSpeed = Constants.EXP_ARROW_SPEED;
        damage = Constants.EXP_ARROW_DAMAGE;
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
        if (hit)
        { 
            Destroy(gameObject);
            Instantiate(explosion, transform.position, transform.localRotation);
        }
    }

    #endregion
}
