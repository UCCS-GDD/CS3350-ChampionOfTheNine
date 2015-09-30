using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract parent script for character controllers
/// </summary>
public abstract class CharacterControllerScript : MonoBehaviour
{
    #region Fields

    protected ActionHandler jumpAbility;
    protected ActionHandler mainAbility;
    protected ActionHandler secondaryAbility;
    protected ActionHandler powerAbility;
    protected ActionHandler specialAbility;
    protected MovementHandler armDirection;
    protected MovementHandler movement;
    protected CharacterScript character;

    #endregion

    #region Properties

    /// <summary>
    /// Returns the tag of this character's target
    /// </summary>
    public abstract string TargetTag
    { get; }

    #endregion

    #region Public Methods

    /// <summary>
    /// Registers for the controller's action calls
    /// </summary>
    /// <param name="jumpAbility">delegate to call on jump</param>
    /// <param name="mainAbility">delegate to call on use main ability</param>
    /// <param name="secondaryAbility">delegate to call on use secondary ability</param>
    /// <param name="powerAbility">delegate to call on use power ability</param>
    /// <param name="specialAbility">delegate to call on use special ability</param>
    /// <param name="movement">delegate to call on move</param>
    /// <param name="armDirection">delegate to call on change arm direction</param>
    public void Register(ActionHandler jumpAbility, ActionHandler mainAbility, ActionHandler secondaryAbility,
        ActionHandler powerAbility, ActionHandler specialAbility, MovementHandler movement, MovementHandler armDirection)
    {
        this.jumpAbility = jumpAbility;
        this.mainAbility = mainAbility;
        this.secondaryAbility = secondaryAbility;
        this.powerAbility = powerAbility;
        this.specialAbility = specialAbility;
        this.movement = movement;
        this.armDirection = armDirection;
    }

    /// <summary>
    /// Handles the character dying
    /// </summary>
    public abstract void Death();

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected virtual void Start()
    {
        character = GetComponent<CharacterScript>();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected abstract void Update();

    #endregion
}

#region Delegates

/// <summary>
/// Delegate for handling an action
/// </summary>
public delegate void ActionHandler();

/// <summary>
/// Delegate to handle movement
/// </summary>
/// <param name="input">the input axis value</param>
public delegate void MovementHandler(float input);

#endregion
