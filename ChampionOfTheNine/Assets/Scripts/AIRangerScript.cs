using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls AI ranger characters
/// </summary>
public class AIRangerScript : AIScript
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
        if (!character.GCD.IsRunning)
        {
            // 1/3 does nothing
            float choice = Random.Range(0, 2);
            if (choice == 0)
            {
                armDirection(((RangerScript)character).GetPredictedShotAngle(target.transform.position, Constants.BASIC_ARROW_SPEED) +
                    Random.Range(-Constants.RANGER_AI_ANGLE_RANGE, Constants.RANGER_AI_ANGLE_RANGE));
                mainAbility();
            }
            else
            {
                character.GCD.Start();
            }
        }
    }

    #endregion
}
