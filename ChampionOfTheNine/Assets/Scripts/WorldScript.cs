﻿using UnityEngine;
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
    Dictionary<SkyStateType, SkyState> skyStates;
    SkyStateType currSkyState;
	[SerializeField]GameObject starrySky;
	[SerializeField]SpriteRenderer sky;
    [SerializeField]SpriteRenderer darkness;
	bool changeToDay = false, changeToNight = false;
	AudioSource BGM;
	AudioClip daySound;
	AudioClip nightSound;
	bool changeMusicToDay = false;
	bool runOnce = true;

    Timer skyTimer;

    #endregion

    #region Private Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    private void Start()
    {
        // SEED IS HARDCODED IN DEBUG MODE
        if (debugMode)
        { Random.seed = 71; }
        
        player = GameObject.Find(Constants.PLAYER_TAG);
        playerLocation = player.transform.position;

        currSkyState = SkyStateType.Day;
        skyStates = new Dictionary<SkyStateType, SkyState>();
        skyStates.Add(SkyStateType.Dawn, new SkyState(SkyStateType.DawnToDay, Constants.ORANGE_SKY_COLOR, Constants.ORANGE_SKY_COLOR,
            Constants.CYCLE_TIME * Constants.ORANGE_TIME_PCT, Constants.ORANGE_DARKNESS_COLOR, Constants.ORANGE_DARKNESS_COLOR));
        skyStates.Add(SkyStateType.DawnToDay, new SkyState(SkyStateType.Day, Constants.ORANGE_SKY_COLOR, Constants.DAY_SKY_COLOR,
            Constants.CYCLE_TIME * Constants.FADE_TIME_PCT, Constants.ORANGE_DARKNESS_COLOR, Constants.DAY_DARKNESS_COLOR));
        skyStates.Add(SkyStateType.Day, new SkyState(SkyStateType.DayToDusk, Constants.DAY_SKY_COLOR, Constants.DAY_SKY_COLOR,
            Constants.CYCLE_TIME * Constants.DAY_TIME_PCT, Constants.DAY_DARKNESS_COLOR, Constants.DAY_DARKNESS_COLOR));
        skyStates.Add(SkyStateType.DayToDusk, new SkyState(SkyStateType.Dusk, Constants.DAY_SKY_COLOR, Constants.ORANGE_SKY_COLOR,
            Constants.CYCLE_TIME * Constants.FADE_TIME_PCT, Constants.DAY_DARKNESS_COLOR, Constants.ORANGE_DARKNESS_COLOR));
        skyStates.Add(SkyStateType.Dusk, new SkyState(SkyStateType.DuskToNight, Constants.ORANGE_SKY_COLOR, Constants.ORANGE_SKY_COLOR,
            Constants.CYCLE_TIME * Constants.ORANGE_TIME_PCT, Constants.ORANGE_DARKNESS_COLOR, Constants.ORANGE_DARKNESS_COLOR));
        skyStates.Add(SkyStateType.DuskToNight, new SkyState(SkyStateType.Night, Constants.ORANGE_SKY_COLOR, Constants.NIGHT_SKY_COLOR,
            Constants.CYCLE_TIME * Constants.ORANGE_TIME_PCT, Constants.ORANGE_DARKNESS_COLOR, Constants.NIGHT_DARKNESS_COLOR));
        skyStates.Add(SkyStateType.Night, new SkyState(SkyStateType.NightToDawn, Constants.NIGHT_SKY_COLOR, Constants.NIGHT_SKY_COLOR,
            Constants.CYCLE_TIME * Constants.NIGHT_TIME_PCT, Constants.NIGHT_DARKNESS_COLOR, Constants.NIGHT_DARKNESS_COLOR));
        skyStates.Add(SkyStateType.NightToDawn, new SkyState(SkyStateType.Dawn, Constants.NIGHT_SKY_COLOR, Constants.ORANGE_SKY_COLOR,
            Constants.CYCLE_TIME * Constants.FADE_TIME_PCT, Constants.NIGHT_DARKNESS_COLOR, Constants.ORANGE_DARKNESS_COLOR));

        skyTimer = new Timer(skyStates[currSkyState].TimeInState);
        skyTimer.Register(SkyTimerFinished);
        skyTimer.Start();

        parallaxBackgrounds = GameObject.FindGameObjectsWithTag(Constants.PARALLAX_BACKGROUND_TAG);
        starrySky.transform.rotation = Quaternion.Euler(0, 0, Constants.SKY_START_ROT);
        
        BGM = GameObject.Find("_BGMsound").GetComponent<AudioSource>();
        BGM.volume = Constants.BGM_MAX_VOLUME;
        daySound = Resources.Load<AudioClip>("Sounds/LordOfTheLand");
        nightSound = Resources.Load<AudioClip>("Sounds/crickets");

        elevationWeight = Constants.ELEVATION_CHANGE_WEIGHT + Random.Range(-Constants.ELEVATION_CHANGE_OFFSET, Constants.ELEVATION_CHANGE_OFFSET); ;
        heightDifferenceWeight = Constants.HEIGHT_DIFFERENCE_WEIGHT + Random.Range(-Constants.HEIGHT_DIFFERENCE_OFFSET, Constants.HEIGHT_DIFFERENCE_OFFSET);


        if (debugMode)
        {
            Debug.Log("Elevation weight: " + elevationWeight);
            Debug.Log("Height Difference Weight: " + heightDifferenceWeight);
        }

        // Generates the map
        GenerateTerrain();
        GenerateParallaxObjects();

        // Spawns enemy castle
        Instantiate(enemyCastle, new Vector2(levels.Length - 4, levels[levels.Length - 4] + 1), transform.rotation);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // Updates the parallax backgrounds
        if (playerLocation != player.transform.position)
        {
            float xDirection = Mathf.Sign(player.transform.position.x - playerLocation.x);
            for (int i = 0; i < parallaxBackgrounds.Length; i++)
            {
                parallaxBackgrounds[i].transform.position = new Vector2(parallaxBackgrounds[i].transform.position.x + 
                    (Constants.PARALLAX_SCALE * Constants.PARALLAX_LEVELS[i] * xDirection), 6);
            }
            playerLocation = player.transform.position;
        }

        // Updates the sky
        skyTimer.Update();
        starrySky.transform.Rotate(0, 0, -360 * (Time.deltaTime / Constants.CYCLE_TIME));
        darkness.color = Color.Lerp(skyStates[currSkyState].StartDarkness, skyStates[currSkyState].EndDarkness, skyTimer.ElapsedSeconds / skyTimer.TotalSeconds);
        sky.color = Color.Lerp(skyStates[currSkyState].StartSkyColor, skyStates[currSkyState].EndSkyColor, skyTimer.ElapsedSeconds / skyTimer.TotalSeconds);

        ////moves the sunMoon back and forth and changes direction if it reaches distance to travel from middle
        //if (sunMoon.GetComponent<SpriteRenderer>().sprite == sun)
        //{
        //    starrySky.transform.Rotate(0, 0, -Constants.STAR_ROTATION_SPEED * Time.deltaTime);
        //    //sunMoon.transform.localPosition = new Vector3(sunMoon.transform.localPosition.x + ((cameraHalfWidth * Time.deltaTime) / Constants.QUARTER_CYCLE), sunMoon.transform.localPosition.y, sunMoon.transform.localPosition.z);
        //    if (sunMoon.transform.localPosition.x > cameraHalfWidth)
        //    {
        //        sunMoon.GetComponent<SpriteRenderer>().sprite = moon;
        //    }
        //}
        //else if (sunMoon.GetComponent<SpriteRenderer>().sprite == moon)
        //{
        //    starrySky.transform.Rotate(0, 0, Constants.STAR_ROTATION_SPEED * Time.deltaTime);
        //    //sunMoon.transform.localPosition = new Vector3(sunMoon.transform.localPosition.x - ((cameraHalfWidth * Time.deltaTime) / Constants.QUARTER_CYCLE), sunMoon.transform.localPosition.y, sunMoon.transform.localPosition.z);
        //    if (sunMoon.transform.localPosition.x < -cameraHalfWidth)
        //    {
        //        sunMoon.GetComponent<SpriteRenderer>().sprite = sun;
        //    }
        //}

        ////Check to see if we should start changing to day or night
        //if (sunMoon.GetComponent<SpriteRenderer> ().sprite == sun && sunMoon.transform.localPosition.x > cameraHalfWidth - Constants.DISTANCE_TO_START_CHANGE) {
        //    changeToNight = true;
        //} else if (sunMoon.GetComponent<SpriteRenderer> ().sprite == moon && sunMoon.transform.localPosition.x < -cameraHalfWidth + Constants.DISTANCE_TO_START_CHANGE) {
        //    changeToDay = true;
        //}

        ////if we should change, change accordingly.
        //if (changeToDay) {
        //    if (runOnce)
        //    {
        //        runOnce = false;
        //        changeMusicToDay = true;
        //        InvokeRepeating ("ChangeMusic", 0, .1f);
        //    }
        //    if (BGM.clip == nightSound)
        //    {
        //        BGM.volume -= Constants.BGM_MAX_VOLUME / 5;
        //    }
        //    else if (BGM.clip == daySound && BGM.volume <= Constants.BGM_MAX_VOLUME - (Constants.BGM_MAX_VOLUME / 5))
        //    {
        //        BGM.volume += Constants.BGM_MAX_VOLUME / 5;
        //    }
        //    if (BGM.volume == 0)
        //    {
        //        BGM.clip = daySound;
        //    }

        //    if (darkness.GetComponent<SpriteRenderer>().color.a > Constants.MIN_DARKNESS_ALPHA + (1 / Constants.QUARTER_CYCLE))
        //    {
        //        darkness.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, darkness.GetComponent<SpriteRenderer>().color.a - (1 / Constants.QUARTER_CYCLE));
        //    } else
        //    {
        //        darkness.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, Constants.MIN_DARKNESS_ALPHA);
        //    }
        //    if (daySky.GetComponent<SpriteRenderer>().color.a < (1 - (1 / Constants.QUARTER_CYCLE)))
        //    {
        //        daySky.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, daySky.GetComponent<SpriteRenderer>().color.a + (1 / Constants.QUARTER_CYCLE));
        //    }
        //    else
        //    {
        //        daySky.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        //        changeToDay = false;
        //        runOnce = true;
        //    }
        //} else if (changeToNight) {
        //    if (runOnce)
        //    {
        //        runOnce = false;
        //        changeMusicToDay = false;
        //        InvokeRepeating ("ChangeMusic", 0, .1f);
        //    }
        //    if (BGM.clip == daySound)
        //    {
        //        BGM.volume -= Constants.BGM_MAX_VOLUME / 5;
        //    }
        //    else if (BGM.clip == nightSound && BGM.volume <= Constants.BGM_MAX_VOLUME - (Constants.BGM_MAX_VOLUME / 5))
        //    {
        //        BGM.volume += Constants.BGM_MAX_VOLUME / 5;
        //    }
        //    if (BGM.volume == 0)
        //    {
        //        BGM.clip = nightSound;
        //    }

        //    //if (darkness.GetComponent<SpriteRenderer>().color.a < Constants.MAX_DARKNESS_ALPHA - (1 / Constants.QUARTER_CYCLE))
        //    //{
        //    //    darkness.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, darkness.GetComponent<SpriteRenderer>().color.a + (1 / Constants.QUARTER_CYCLE));
        //    //} else
        //    //{
        //    //    darkness.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, Constants.MAX_DARKNESS_ALPHA);
        //    //}
        //    //if (daySky.GetComponent<SpriteRenderer>().color.a > (1 / Constants.QUARTER_CYCLE))
        //    //{
        //    //    daySky.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, daySky.GetComponent<SpriteRenderer>().color.a - (1 / Constants.QUARTER_CYCLE));
        //    //}
        //    else
        //    {
        //        daySky.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        //        changeToNight = false;
        //        runOnce = true;
        //    }
        //}
    }

    /// <summary>
    /// Generates the parallax objects for the map
    /// </summary>
    private void GenerateParallaxObjects()
    {
        float horizontalPosition = 0;
        float verticalPosition = 0;

        // Generates clouds on the backgrounds
        GameObject[] parallaxBackgrounds = GameObject.FindGameObjectsWithTag(Constants.PARALLAX_BACKGROUND_TAG);
        foreach (GameObject bg in parallaxBackgrounds)
        {
            for (int i = 0; i < (int)(Constants.MAP_LENGTH * Constants.CLOUD_DENSITY); i++)
            {
                horizontalPosition = Random.Range(0, Constants.MAP_LENGTH);
                verticalPosition = Random.Range((float)levels[(int)horizontalPosition] + 3.00f, (float)levels[(int)horizontalPosition] + 12.00f);
                GameObject newObject = Instantiate(cloudPrefabs[Random.Range(0, cloudPrefabs.Length)]) as GameObject;
                newObject.transform.SetParent(bg.transform);
                newObject.transform.position = new Vector3(horizontalPosition, verticalPosition, 2);
                newObject.transform.localScale *= Random.Range(Constants.CLOUD_SCALE_MIN, Constants.CLOUD_SCALE_MAX);
            }
        }
    }

    /// <summary>
    /// Randomly generates the level terrain
    /// </summary>
    private void GenerateTerrain()
    {
        // Fill array: Left platform, middle section, right platform
        int mapLengthMinusPlatform = Constants.MAP_LENGTH - Constants.PLATFORM_LENGTH;
        for (int i = 0; i < Constants.PLATFORM_LENGTH; i++)
        { levels[i] = Constants.BASE_LEVEL; }
        for (int i = Constants.PLATFORM_LENGTH; i < mapLengthMinusPlatform; i++)
        { levels[i] = NextHeight(levels[i - 1]); }
        for (int i = mapLengthMinusPlatform; i < Constants.MAP_LENGTH; i++)
        { levels[i] = levels[mapLengthMinusPlatform - 1]; }

        // Create blocks from array
        GameObject groundPrefab = Resources.Load<GameObject>(Constants.PREFAB_FOLDER + Constants.GROUND_PREFAB);
        GameObject groundUnderPrefab = Resources.Load<GameObject>(Constants.PREFAB_FOLDER + Constants.GROUND_UNDER_PREFAB);
        for (int i = 0; i < levels.Length; i++)
        {
            GameObject newObject = Instantiate(groundPrefab) as GameObject;
            newObject.transform.position = new Vector2(i, levels[i]);

            //draws the blocks under the top
            for (int j = 1; j <= Constants.SOIL_HEIGHT; j++)
            {
                newObject = Instantiate(groundUnderPrefab) as GameObject;
                newObject.transform.position = new Vector2(i, levels[i] - j);
            }
        }
    }

    /// <summary>
    /// Changes the background music
    /// </summary>
    private void ChangeMusic()
    {
        if (changeMusicToDay)
        {
            if (BGM.clip == nightSound)
            {
                BGM.volume -= Constants.BGM_MAX_VOLUME / 20;
            }
            else if (BGM.clip == daySound && BGM.volume <= Constants.BGM_MAX_VOLUME - (Constants.BGM_MAX_VOLUME / 20))
            {
                BGM.volume += Constants.BGM_MAX_VOLUME / 20;
            }
            else
            {
                CancelInvoke("ChangeMusic");
            }
            if (BGM.volume <= .05f)
            {
                BGM.Stop();
                BGM.clip = daySound;
                BGM.Play();
            }
        }
        else
        {
            if (BGM.clip == daySound)
            {
                BGM.volume -= Constants.BGM_MAX_VOLUME / 20;
            }
            else if (BGM.clip == nightSound && BGM.volume <= Constants.BGM_MAX_VOLUME - (Constants.BGM_MAX_VOLUME / 20))
            {
                BGM.volume += Constants.BGM_MAX_VOLUME / 20;
            }
            else
            {
                CancelInvoke("ChangeMusic");
            }
            if (BGM.volume <= .05f)
            {
                BGM.Stop();
                BGM.clip = nightSound;
                BGM.Play();
            }
        }
    }

    /// <summary>
    /// Gets the next height value based on the previous height, the weight, and randomness
    /// </summary>
    /// <param name="previous">the previous height</param>
    /// <returns>the next height</returns>
    private int NextHeight(int previous)
    {
        //use weight to decide if we should change direction or not.
        if (Random.Range(0.00f, 1.00f) <= elevationWeight)
        { return previous - (int)Mathf.Sign(Random.Range(0.00f, 1.00f) - heightDifferenceWeight); }
        else
        { return previous; }
    }

    /// <summary>
    /// Handles the sky timer finishing
    /// </summary>
    private void SkyTimerFinished()
    {
        currSkyState = skyStates[currSkyState].NextState;
        skyTimer.TotalSeconds = skyStates[currSkyState].TimeInState;
        skyTimer.Start();
    }

    #endregion
}
