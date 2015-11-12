using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Script that controls an ice spell
/// </summary>
public class IceSpellScript : ProjScript
{
    #region Fields

    [SerializeField]GameObject particle;

    #endregion

    #region Protected Methods

    /// <summary>
    /// Initializes the projectile
    /// </summary>
    /// <param name="targetTag">the tag of the targeted characters</param>
    protected override void Initialize(string targetTag)
    {
        moveSpeed = Constants.ICE_SPEED;
        damage = Constants.ICE_DAMAGE;
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
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected void Start()
    {
        Instantiate(particle, transform.position, transform.rotation);
    }

    /// <summary>
    /// Handles the projectile being destroyed
    /// </summary>
    protected void OnDestroy()
    {
        Instantiate(particle, transform.position, transform.rotation);
    }

    #endregion
}
