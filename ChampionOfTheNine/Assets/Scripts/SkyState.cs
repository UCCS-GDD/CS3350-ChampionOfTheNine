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
    Color startDarkness;
    Color endDarkness;
    float timeInState;
    float startVolume;
    float endVolume;
    AudioClip endAudio;

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    public SkyState(SkyStateType nextState, Color startSkyColor, Color endSkyColor, float timeInState, Color startDarkness, Color endDarkness,
        float startVolume = Constants.BGM_MAX_VOLUME, float endVolume = Constants.BGM_MAX_VOLUME, AudioClip endAudio = null)
    {
        this.nextState = nextState;
        this.startSkyColor = startSkyColor;
        this.endSkyColor = endSkyColor;
        this.timeInState = timeInState;
        this.startDarkness = startDarkness;
        this.endDarkness = endDarkness;
        this.startVolume = startVolume;
        this.endVolume = endVolume;
        this.endAudio = endAudio;
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

    public float StartVolume
    {
        get { return startVolume; }
        set { startVolume = value; }
    }

    public float EndVolume
    {
        get { return endVolume; }
        set { endVolume = value; }
    }

    public AudioClip EndAudio
    {
        get { return endAudio; }
        set { endAudio = value; }
    }

    #endregion

    #region Public Methods



    #endregion
}
