using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the in-level world
/// </summary>
public class WorldScript : MonoBehaviour
{
    #region Fields

    GameObject player;
    Vector3 playerLocation;

    GameObject[] parallaxBackgrounds;

    bool debugMode = true;

    [SerializeField]GameObject[] cloudPrefabs;
    [SerializeField]GameObject enemyCastle;
	int[] levels = new int[Constants.MAP_LENGTH];
	float elevationWeight = 1;
	float heightDifferenceWeight = 1;

	//Time of day stuff
	GameObject starrySky;
	GameObject daySky;
	GameObject sunMoon;
	GameObject darkness;
	Sprite sun;
	Sprite moon;
	bool changeToDay = false, changeToNight = false;
	AudioSource BGM;
	AudioClip daySound;
	AudioClip nightSound;
	bool changeMusicToDay = false;
	bool runOnce = true;

    float cameraHalfWidth = 0;

    #endregion

    #region Properties



    #endregion

    #region Public Methods



    #endregion

    #region Protected Methods



    #endregion

    #region Private Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG);
        playerLocation = player.transform.position;

        parallaxBackgrounds = GameObject.FindGameObjectsWithTag(Constants.PARALLAX_BACKGROUND_TAG);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {

    }


    private void FillMapArray()
    {

    }

    #endregion
}
