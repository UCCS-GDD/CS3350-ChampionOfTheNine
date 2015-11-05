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
    Timer drainTimer;
    List<SpriteRenderer> beams;
    List<DamagableObjectScript> drainTargets;
    Dictionary<Color, Color> beamColors;

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
        specialCDTimer = new Timer(Constants.DRAIN_CD);
        lightningTimer = new Timer(Constants.LIGHTNING_CAST_TIME);
        lightningTimer.Register(LightningTimerFinished);
        beams = new List<SpriteRenderer>();
        drainTimer = new Timer(Constants.DRAIN_TIME);
        drainTimer.Register(DrainTimerFinished);
        drainTargets = new List<DamagableObjectScript>();
        beamColors = new Dictionary<Color, Color>();
        beamColors.Add(Constants.BEAM_COLOR_1, Constants.BEAM_COLOR_2);
        beamColors.Add(Constants.BEAM_COLOR_2, Constants.BEAM_COLOR_1);

        // Loads sounds
        mainAbilitySound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.ICE_CAST_SND);
        secondaryAbilitySound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.LIGHTNING_CAST_SND);
        powerAbilitySound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.METEOR_CAST_SND);
        specialAbilitySound = Resources.Load<AudioClip>(Constants.SND_FOLDER + Constants.DRAIN_SND);
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

        // Updates lightning
        if (lightningProj != null)
        {
            if (Energy >= Constants.LIGHTNING_COST_PER_SEC * Time.deltaTime)
            {
                lightningProj.SetLocationAndDirection(fireLocation.position, ShotAngle);
                Energy -= (Constants.LIGHTNING_COST_PER_SEC * Time.deltaTime);
                lightningTimer.Update();
            }
            else
            { lightningTimer.Finish(); }
        }

        try
        {
            // Updates beams
            if (drainTimer.IsRunning)
            {
                // Updates flash
                if (drainTimer.ElapsedSeconds % Constants.DRAIN_FLASH_TIME < Time.deltaTime)
                {
                    foreach (SpriteRenderer beam in beams)
                    { beam.color = beamColors[beam.color]; }
                }

                // Damages targets and increases mana
                for (int i = drainTargets.Count - 1; i >= 0; i--)
                {
                    if (drainTargets[i].Damage(Constants.DRAIN_DAMAGE * (Time.deltaTime / Constants.DRAIN_TIME)))
                    { Energy = Mathf.Min(maxEnergy, Energy + (Constants.DRAIN_MANA_PER_TARGET * (Time.deltaTime / Constants.DRAIN_TIME))); }
                    else
                    { drainTargets.RemoveAt(i); }
                }

                drainTimer.Update();
            }
        }
        catch (System.NullReferenceException) { }
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
        if (!gCDTimer.IsRunning && !specialCDTimer.IsRunning && !drainTimer.IsRunning)
        {
            gCDTimer.Start();
            drainTimer.Start();
            specialCDTimer.Start();

            // Send beams to all nearby enemies
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
            foreach (GameObject obj in enemies)
            {
                if (Vector2.Distance(fireLocation.position, obj.transform.position) < Constants.DRAIN_RANGE)
                {
                    drainTargets.Add(obj.GetComponent<DamagableObjectScript>());
                    Vector2 topLocation = new Vector2((obj.transform.position.x - fireLocation.position.x) / 2,
                        Mathf.Max(obj.transform.position.y - fireLocation.position.y, 0) + Random.Range(Constants.DRAIN_MIN_HEIGHT, 
                        Constants.DRAIN_MAX_HEIGHT)) + (Vector2)fireLocation.position;
                    Vector2 location = fireLocation.position;
                    bool goingLeft = fireLocation.position.x > obj.transform.position.x;

                    // Calculate beam
                    for (float i = 1; i <= Constants.DRAIN_SEGMENTS; i++)
                    { CalcAndSpawnBeam(i, ref location, topLocation, fireLocation.position, goingLeft); }
                    for (float i = Constants.DRAIN_SEGMENTS - 1; i >= 0; i--)
                    { CalcAndSpawnBeam(i, ref location, topLocation, obj.transform.position, goingLeft); }
                }
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

    /// <summary>
    /// Handles the drain timer finishing
    /// </summary>
    protected virtual void DrainTimerFinished()
    {
        foreach (SpriteRenderer beam in beams)
        { Destroy(beam.gameObject); }
        beams.Clear();
        drainTargets.Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="num"></param>
    /// <param name="location"></param>
    /// <param name="topLocation"></param>
    /// <param name="endpointLocation"></param>
    protected void CalcAndSpawnBeam(float num, ref Vector2 location, Vector2 topLocation, Vector2 endpointLocation, bool goingLeft)
    {
        Vector2 oldLocation = location;
        location.x = Mathf.Lerp(endpointLocation.x, topLocation.x, num / Constants.DRAIN_SEGMENTS);
        location.y = Mathf.Lerp(endpointLocation.y, topLocation.y, 1 - Mathf.Pow(0.5f, num));
        float distance = Vector2.Distance(oldLocation, location);
        float angle = Mathf.Asin((location.y - oldLocation.y) / distance) * Mathf.Rad2Deg;
        if (goingLeft)
        { angle = 180 - angle; }
        GameObject beamInst = (GameObject)Instantiate(beam, oldLocation, Quaternion.Euler(0, 0, angle));
        beamInst.transform.localScale = new Vector3(distance, 1, 1);
        beams.Add(beamInst.GetComponent<SpriteRenderer>());
    }

    #endregion
}
