using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Script that controls an ice spell
/// </summary>
public class IceSpellScript : ProjScript
{
    #region Protected Methods

    /// <summary>
    /// Initializes the projectile
    /// </summary>
    /// <param name="targetTag">the tag of the targeted characters</param>
    /// <param name="damage">the projectile's damage</param>
    /// <param name="moveSpeed">the projectile's movement speed</param>
    protected override void Initialize(string targetTag, float damage, float moveSpeed)
    {
        GameManager.Instance.SpawnParticle(Constants.ICE_PART, transform.position);
        base.Initialize(targetTag, damage, moveSpeed);
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
            GameManager.Instance.SpawnParticle(Constants.ICE_PART, transform.position); 
            Destroy(gameObject);
        }
    }

    #endregion
}
