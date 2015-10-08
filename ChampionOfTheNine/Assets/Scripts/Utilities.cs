using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Class for various utility functions
/// </summary>
public static class Utilities
{
    /// <summary>
    /// Gets the mouse position in world space
    /// </summary>
    public static Vector2 MousePosition
    { get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); } }

    /// <summary>
    /// Plays a sound with a randomly modified pitch
    /// </summary>
    /// <param name="source">the audio source</param>
    /// <param name="clip">the sound</param>
    public static void PlaySoundPitched(AudioSource source, AudioClip clip)
    {
        source.pitch = 1 + Random.Range(-Constants.PITCH_CHANGE, Constants.PITCH_CHANGE);
        source.PlayOneShot(clip);
    }
}
