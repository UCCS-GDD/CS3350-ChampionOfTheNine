using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls a castle
/// </summary>
public class CastleScript : DamagableObjectScript
{
    #region Fields

    [SerializeField]Sprite destroyedSprite;
    SpriteRenderer spriteRenderer;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        Utilities.PlaySoundPitched(audioSource, deathSound);
        spriteRenderer.sprite = destroyedSprite;
		gameObject.transform.FindChild ("EnemySpawnLocation").GetComponent<EnemySpawner> ().CancelInvoke ();
    }
}
