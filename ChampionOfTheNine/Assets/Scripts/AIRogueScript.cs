using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls AI rogue characters
/// </summary>
public class AIRogueScript : AIScript
{
    #region Properties

    /// <summary>
    /// Returns the tag of this character's target
    /// </summary>
    public override string TargetTag
    { get { return Constants.PLAYER_TAG; } }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected override void Start()
    {
        base.Start();
        targetRange = Constants.RANGER_AI_RANGE;
    }

    /// <summary>
    /// Attacks the target
    /// </summary>
    protected override void Attack()
    {
        if (!character.OnGlobalCooldown)
        {
            armDirection(((RogueScript)character).GetPredictedShotAngle(target.transform.position, Constants.BASIC_ARROW_SPEED));
            mainAbility();
        }
    }

    #endregion
}
