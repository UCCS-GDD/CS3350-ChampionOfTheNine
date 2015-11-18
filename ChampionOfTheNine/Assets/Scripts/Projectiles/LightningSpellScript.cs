using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Script that controls a lightning spell
/// </summary>
public class LightningSpellScript : ProjScript
{
    #region Protected Methods

    /// <summary>
    /// Initializes the projectile
    /// </summary>
    /// <param name="targetTag">the tag of the targeted characters</param>
    protected override void Initialize(string targetTag)
    {
        moveSpeed = 0;
        damage = Constants.LIGHTNING_DAMAGE;
        base.Initialize(targetTag);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected override void NotPausedUpdate()
    { }

    /// <summary>
    /// Handles the lightning continuing to collide with something
    /// </summary>
    /// <param name="other">the other collider</param>
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }

    #endregion
}
