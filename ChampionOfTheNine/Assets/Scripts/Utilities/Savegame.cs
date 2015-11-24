using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Class that holds game save information
/// </summary>
[Serializable]
public class Savegame
{
    #region Fields

    Dictionary<int, KingdomName> kingdomLocations;
    Dictionary<KingdomName, KingdomStatus> kingdomStatuses;
    Dictionary<InputType, InputButton> inputs;
    CharacterType playerType;

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    public Savegame(CharacterType playerType)
    {
        kingdomLocations = new Dictionary<int, KingdomName>();
        kingdomStatuses = new Dictionary<KingdomName, KingdomStatus>();
        inputs = new Dictionary<InputType, InputButton>();
        inputs.Add(InputType.Main, new InputButton(0));
        inputs.Add(InputType.Secondary, new InputButton(1));
        inputs.Add(InputType.Power, new InputButton("e"));
        inputs.Add(InputType.Special, new InputButton("r"));
        this.playerType = playerType;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the saved kingdom statuses
    /// </summary>
    public Dictionary<KingdomName, KingdomStatus> KingdomStatuses
    {
        get { return kingdomStatuses; }
    }

    /// <summary>
    /// Gets the saved kingdom locations
    /// </summary>
    public Dictionary<int, KingdomName> KingdomLocations
    {
        get { return kingdomLocations; }
    }

    /// <summary>
    /// Gets the player type
    /// </summary>
    public CharacterType PlayerType
    {
        get { return playerType; }
        set { playerType = value; }
    }

    /// <summary>
    /// Gets the inputs dictionary
    /// </summary>
    public Dictionary<InputType, InputButton> Inputs
    { get { return inputs; } }

    #endregion
}
