using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Class that holds sky state information
/// </summary>
public class SkyState
{
    #region Fields

    SkyStateType nextState;
    Color startSkyColor;
    Color endSkyColor;
    float timeInState;
    Color startDarkness;
    Color endDarkness;

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    public SkyState(SkyStateType nextState, Color startSkyColor, Color endSkyColor, float timeInState, Color startDarkness, Color endDarkness)
    {
        this.nextState = nextState;
        this.startSkyColor = startSkyColor;
        this.endSkyColor = endSkyColor;
        this.timeInState = timeInState;
        this.startDarkness = startDarkness;
        this.endDarkness = endDarkness;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the starting sky color for this state
    /// </summary>
    public Color StartSkyColor
    {
        get { return startSkyColor; }
    }

    /// <summary>
    /// Gets the ending sky color for this state
    /// </summary>
    public Color EndSkyColor
    {
        get { return endSkyColor; }
    }

    /// <summary>
    /// Gets the time spent in this state
    /// </summary>
    public float TimeInState
    {
        get { return timeInState; }
    }

    /// <summary>
    /// Gets the state after this state
    /// </summary>
    public SkyStateType NextState
    {
        get { return nextState; }
    }

    /// <summary>
    /// The starting darkness alpha for this state
    /// </summary>
    public Color StartDarkness
    {
        get { return startDarkness; }
    }

    /// <summary>
    /// The ending darkness alpha for this state
    /// </summary>
    public Color EndDarkness
    {
        get { return endDarkness; }
    }

    #endregion

    #region Public Methods



    #endregion
}
