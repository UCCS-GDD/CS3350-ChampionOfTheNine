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
        set { kingdomLocations = value; }
    }

    /// <summary>
    /// Gets the player type
    /// </summary>
    public CharacterType PlayerType
    {
        get { return playerType; }
    }

    #endregion
}
