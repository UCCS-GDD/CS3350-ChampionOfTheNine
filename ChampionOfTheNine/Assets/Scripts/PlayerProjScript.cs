using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract parent script that controls player projectiles
/// </summary>
public abstract class PlayerProjScript : ProjScript
{
    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected virtual void Start()
    {
        targetTag = Constants.ENEMY_TAG;
    }

    #endregion
}
