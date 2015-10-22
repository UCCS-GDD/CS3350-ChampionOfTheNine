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

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    public Savegame()
    {
        kingdomLocations = new Dictionary<int, KingdomName>();
        kingdomStatuses = new Dictionary<KingdomName, KingdomStatus>();
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

    #endregion

    #region Public Methods



    #endregion
}
