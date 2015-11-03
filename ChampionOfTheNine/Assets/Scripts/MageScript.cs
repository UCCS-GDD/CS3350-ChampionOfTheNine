using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Script for a mage character
/// </summary>
public class MageScript : CharacterScript
{
    #region Fields

    [SerializeField]GameObject ice;
    [SerializeField]GameObject meteor;
    [SerializeField]GameObject lightning;
    [SerializeField]GameObject beam;

    LightningSpellScript lightningProj = null;
    Timer lightningTimer;

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected override void Start()
    {
        // Sets fields
        maxHealth = Constants.MAGE_HEALTH;
        moveSpeed = Constants.MAGE_MOVE_SPEED;
        jumpSpeed = Constants.MAGE_JUMP_SPEED;
        maxEnergy = Constants.MAGE_ENERGY;
        gCDTimer = new Timer(Constants.MAGE_GCD);
        secondaryCDTimer = new Timer(Constants.LIGHTNING_CD);
        powerCDTimer = new Timer(Constants.METEOR_CD);
        specialCDTimer = new Timer(Constants.MAGE_SPECIAL_CD);
        lightningTimer = new Timer(Constants.LIGHTNING_CAST_TIME);
        lightningTimer.Register(LightningTimerFinished);

        // Loads sounds
        mainAbilitySound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.ICE_CAST_SND);
        secondaryAbilitySound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.LIGHTNING_CAST_SND);
        powerAbilitySound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.METEOR_CAST_SND);
        specialAbilitySound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.MAGE_SPECIAL_SND);
        base.Start();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    public override void UpdateChar()
    {
        base.UpdateChar();

        // Updates energy
        if (Energy < maxEnergy)
        { Energy = Mathf.Min(maxEnergy, Energy + (Constants.MAGE_REGEN * Time.deltaTime)); }

        if (lightningProj != null)
        {
            lightningProj.SetLocationAndDirection(fireLocation.position, ShotAngle);
            Energy -= (Constants.LIGHTNING_COST_PER_SEC * Time.deltaTime);
            lightningTimer.Update();
        }
    }

    /// <summary>
    /// Fires the character's main ability
    /// </summary>
    protected override void FireMainAbility()
    {
        ProjScript projectile = FireStraightProjectileAttack(ice, Constants.ICE_COST, gCDTimer);
        if (projectile != null)
        { Utilities.PlaySoundPitched(audioSource, mainAbilitySound); }
    }

    /// <summary>
    /// Fires the character's secondary ability
    /// </summary>
    protected override void FireSecondaryAbility()
    {
        if (!gCDTimer.IsRunning)
        {
            lightningTimer.Start();
            if (lightningProj == null)
            {
                lightningProj = ((GameObject)Instantiate(lightning)).GetComponent<LightningSpellScript>();
                lightningProj.Initialize(fireLocation.position, ShotAngle, targetTag);
            }
        }
    }

    /// <summary>
    /// Fires the character's power ability
    /// </summary>
    protected override void FirePowerAbility()
    {
        if (!powerCDTimer.IsRunning)
        {
            ProjScript projectile = FireProjectileAttack(meteor, Constants.METEOR_COST, gCDTimer);
            if (projectile != null)
            {
                Utilities.PlaySoundPitched(audioSource, powerAbilitySound);
                powerCDTimer.Start();
                Vector2 shotLocation = (Vector2)transform.position + Constants.METEOR_START_LOC;
                projectile.Initialize(shotLocation, Utilities.GetAngleDegrees(shotLocation, Utilities.MousePosition), targetTag);
            }
        }
    }

    /// <summary>
    /// Fires the character's special ability
    /// </summary>
    protected override void FireSpecialAbility()
    {
        if (!gCDTimer.IsRunning)
        {
            gCDTimer.Start();
            Vector2 topLocation = new Vector2(3, 5) + (Vector2)fireLocation.position;
            Vector2 location = fireLocation.position;
            int totalSegments = 5;

            // Calculate random beam
            //Vector2 endLocation = (Vector2)transform.position + new Vector2(4, 0);
            //float value = 0.01f;
            //float turnSpeed = Random.Range(0.1f, 0.3f);
            //float heightVal = 1;
            //float angle = 90;
            //int i = 0;
            for (float i = 1; i <= totalSegments; i++)
            {
                Vector2 oldLocation = location;
                location = Vector2.Lerp(fireLocation.position, topLocation, i / totalSegments);
                float distance = Vector2.Distance(oldLocation, location);
                float angle = Mathf.Asin((location.y - oldLocation.y) / distance) * Mathf.Rad2Deg;
                GameObject beamInst = (GameObject)Instantiate(beam, oldLocation, Quaternion.Euler(0, 0, angle));
                beamInst.transform.localScale = new Vector3(distance, 1, 1);
            }
        }
    }

    /// <summary>
    /// Handles the lightning timer finishing
    /// </summary>
    protected virtual void LightningTimerFinished()
    {
        Destroy(lightningProj.gameObject);
        lightningProj = null;
        gCDTimer.Start();
    }

    #endregion
}
