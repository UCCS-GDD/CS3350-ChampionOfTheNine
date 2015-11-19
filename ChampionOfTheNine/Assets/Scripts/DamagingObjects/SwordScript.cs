using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls a a warrior's sword
/// </summary>
public class SwordScript : DamagingObjectScript
{
    /// <summary>
    /// Updates the object while it isn't paused
    /// </summary>
    protected override void NotPausedUpdate()
    { }

    /// <summary>
    /// Handles the sword continuing to collide with something
    /// </summary>
    /// <param name="other">the other collider</param>
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
}
