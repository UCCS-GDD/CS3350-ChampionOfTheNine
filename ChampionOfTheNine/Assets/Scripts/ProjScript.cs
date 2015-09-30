using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract parent script that controls projectiles
/// </summary>
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class ProjScript : MonoBehaviour 
{
    #region Fields

    protected float damage;
    protected float moveSpeed;
    protected HitType hit = HitType.None;
    protected string targetTag;
    Rigidbody2D rbody;

    #endregion

    #region Public Methods
    
    /// <summary>
    /// Initializes the projectile
    /// </summary>
    /// <param name="fromPosition">the position of the projectile</param>
    /// <param name="toPosition">the target position</param>
    /// <param name="targetTag">the tag of the targeted characters</param>
    public virtual void Initialize(Vector2 fromPosition, Vector2 toPosition, string targetTag)
    {
        Initialize(targetTag);
        SetLocationAndDirection(fromPosition, toPosition);
    }

    /// <summary>
    /// Initializes the projectile
    /// </summary>
    /// <param name="position">the position of the projectile</param>
    /// <param name="angle">the angle, in degrees</param>
    /// <param name="targetTag">the tag of the targeted characters</param>
    public virtual void Initialize(Vector2 position, float angle, string targetTag)
    {
        Initialize(targetTag);
        SetLocationAndDirection(position, angle);
    }

    /// <summary>
    /// Changes the speed of the projectile by the given multiplier
    /// </summary>
    /// <param name="multiplier">the speed multiplier</param>
    public virtual void ChangeSpeed(float multiplier)
    {
        rbody.velocity *= multiplier;
    }

    /// <summary>
    /// Changes the damage of the projectile by the given multiplier
    /// </summary>
    /// <param name="multiplier">the damage multiplier</param>
    public virtual void ChangeDamage(float multiplier)
    {
        damage *= multiplier;
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Initializes the projectile
    /// </summary>
    /// <param name="targetTag">the tag of the targeted characters</param>
    protected virtual void Initialize(string targetTag)
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.velocity = new Vector2(moveSpeed, 0);
        this.targetTag = targetTag;
    }

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
            hit = HitType.Target;
        }
        else if (other.gameObject.layer == Constants.GROUND_LAYER)
        { hit = HitType.Ground; }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected virtual void Update()
    {
        // Updates the projectile angle
        float shotAngle = Mathf.Atan(rbody.velocity.y / rbody.velocity.x);
        if (rbody.velocity.x < 0)
        { shotAngle -= Mathf.PI; }

        transform.localRotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * shotAngle);
    }

    /// <summary>
    /// Sets the projectile's position and direction
    /// </summary>
    /// <param name="fromPosition">the position of the projectile</param>
    /// <param name="toPosition">the target position</param>
    protected void SetLocationAndDirection(Vector2 fromPosition, Vector2 toPosition)
    {
        // Calculates shot angle
        float shotAngle = Mathf.Asin((toPosition.y - fromPosition.y) / Vector2.Distance(toPosition, fromPosition));
        if (toPosition.x - fromPosition.x < 0)
        { shotAngle = Mathf.PI - shotAngle; }

        // Sets position and direction
        transform.position = fromPosition;
        transform.localRotation = Quaternion.Euler(0, 0, shotAngle * Mathf.Rad2Deg);

        rbody.velocity = new Vector2(Mathf.Cos(shotAngle) * moveSpeed, Mathf.Sin(shotAngle) * moveSpeed);
    }

    /// <summary>
    /// Sets the projectile's position and direction
    /// </summary>
    /// <param name="position">the position of the projectile</param>
    /// <param name="angle">the angle, in degrees</param>
    protected void SetLocationAndDirection(Vector2 position, float angle)
    {
        // Sets position and direction
        transform.position = position;
        transform.localRotation = Quaternion.Euler(0, 0, angle);

        rbody.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * moveSpeed, Mathf.Sin(angle * Mathf.Deg2Rad) * moveSpeed);
    }

    #endregion
}
