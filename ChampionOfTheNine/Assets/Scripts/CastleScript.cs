using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls a castle
/// </summary>
public class CastleScript : DamagableObjectScript
{
    #region Fields



    #endregion

    #region Properties



    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected override void Start()
    {
        maxHealth = Constants.CASTLE_HEALTH;
        hitSound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.CASTLE_HIT_SND);
        deathSound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.CASTLE_DEATH_SND);
        base.Start();
    }

    ///// <summary>
    ///// Update is called once per frame
    ///// </summary>
    //private void Update()
    //{

    //}

    #endregion

    /// <summary>
    /// Handles the castle dying
    /// </summary>
    protected override void Death()
    {
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        Destroy(gameObject);
    }
}
