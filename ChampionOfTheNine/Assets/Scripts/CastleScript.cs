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

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected void Start()
    {
        maxHealth = Constants.CASTLE_HEALTH;
        hitSound = GameManager.Instance.GameSounds[Constants.CASTLE_HIT_SND];
        deathSound = GameManager.Instance.GameSounds[Constants.CASTLE_DEATH_SND];
        spriteRenderer = GetComponent<SpriteRenderer>();
        base.Initialize();
    }

    #endregion

    /// <summary>
    /// Handles the castle dying
    /// </summary>
    protected override void Death()
    {
        GameManager.Instance.PlaySoundPitched(audioSource, deathSound);
        spriteRenderer.sprite = destroyedSprite;
        WorldScript.Instance.Defeat(gameObject.tag);

        try
        { gameObject.GetComponent<EnemySpawner>().enabled = false; }
        catch { }
    }
}
