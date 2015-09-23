using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract parent script that controls projectiles
/// </summary>
[RequireComponent(typeof(Collider2D))]
public abstract class ProjScript : MonoBehaviour 
{
    #region Fields

    protected float damage;
    protected bool hit = false;
    protected Vector2 targetPosition;
    protected string targetTag;

    #endregion

    #region Public Methods

    /// <summary>
    /// Sets the projectile's position and direction
    /// </summary>
    /// <param name="fromPosition">the position of the projectile</param>
    /// <param name="toPosition">the target position</param>
    public void SetLocationAndDirection(Vector2 fromPosition, Vector2 toPosition)
    {
        // Calculates shot angle
        float shotAngle = 0;
        if (toPosition.x - fromPosition.x > 0)
        { shotAngle = Mathf.Asin((toPosition.y - fromPosition.y) / Vector2.Distance(toPosition, fromPosition)); }
        else
        { shotAngle = Mathf.PI - Mathf.Asin((toPosition.y - fromPosition.y) / Vector2.Distance(toPosition, fromPosition)); }

        // Sets position and direction
        transform.position = fromPosition;
        transform.localRotation = Quaternion.Euler(0, 0, shotAngle * Mathf.Rad2Deg);

        targetPosition = toPosition;
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
            other.gameObject.GetComponent<CharacterScript>().Damage(damage);
            hit = true;
        }
        else if (other.gameObject.layer == Constants.GROUND_LAYER)
        { hit = true; }
    }

    ///// <summary>
    ///// Update is called once per frame
    ///// </summary>
    //protected virtual void Update()
    //{

    //}

    #endregion
}
