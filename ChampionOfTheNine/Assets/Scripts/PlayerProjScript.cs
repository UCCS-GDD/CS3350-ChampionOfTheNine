using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract parent script that controls player projectiles
/// </summary>
public abstract class PlayerProjScript : ProjScript
{
    #region Public Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fromPosition"></param>
    /// <param name="toPosition"></param>
    public override void Initialize(Vector2 fromPosition, Vector2 toPosition)
    {
        targetTag = Constants.ENEMY_TAG;
        base.Initialize(fromPosition, toPosition);
    }

    #endregion
}
