using UnityEngine;
using System.Collections;

/// <summary>
/// Script that handles dynamic level generation
/// </summary>
public class DynamicLevelGeneration : MonoBehaviour 
{
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
	
	/// <summary>
	/// Start is called once on object creation
	/// </summary>
	void Start () {

        Random.seed = 71;

		starrySky = GameObject.Find ("starrySky");
		daySky = GameObject.Find ("daySky");
		sunMoon = GameObject.Find ("sunMoon");
		darkness = GameObject.Find ("darkness");
		sun = Resources.Load<Sprite> ("Sprites/Level/sun");
		moon = Resources.Load<Sprite> ("Sprites/Level/moon");
		BGM = GameObject.Find ("_BGMsound").GetComponent<AudioSource> ();
		BGM.volume = Constants.BGM_MAX_VOLUME;
		daySound = Resources.Load<AudioClip> ("Sounds/LordOfTheLand");
		nightSound = Resources.Load<AudioClip> ("Sounds/crickets");
        //InvokeRepeating ("HandleTimeOfDay", 0, .1f);

		elevationWeight = Constants.ELEVATION_CHANGE_WEIGHT + Random.Range (-Constants.ELEVATION_CHANGE_OFFSET, Constants.ELEVATION_CHANGE_OFFSET);;
		heightDifferenceWeight = Constants.HEIGHT_DIFFERENCE_WEIGHT + Random.Range (-Constants.HEIGHT_DIFFERENCE_OFFSET, Constants.HEIGHT_DIFFERENCE_OFFSET);
		

		if (debugMode) {
			Debug.Log ("Elevation weight: " + elevationWeight);
			Debug.Log ("Height Difference Weight: " + heightDifferenceWeight);
		}

		//creates "platform" for the castle on the left
		for (int i = 0; i < Constants.PLATFORM_LENGTH; i++)
		{
			levels[i] = Constants.BASE_LEVEL;
		}

		//fills in the rest of the aray.
        int mapLengthMinusPlatform = Constants.MAP_LENGTH - Constants.PLATFORM_LENGTH;
        for (int i = Constants.PLATFORM_LENGTH; i < mapLengthMinusPlatform; i++) 
		{
			levels[i] = NextHeight(levels[i - 1]);
		}

		//creates platform for right castle
        for (int i = mapLengthMinusPlatform; i < Constants.MAP_LENGTH; i++) 
		{
            levels[i] = levels[mapLengthMinusPlatform - 1];
		}

        cameraHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;

		DrawMap ();
	}

    

    /// <summary>
    /// Places objects to create the map based on the generated terrain levels
    /// </summary>
	void DrawMap()
	{
        //Transform groundParent = GameObject.Find ("Ground").transform;

		//draws each of the top blocks
        GameObject groundPrefab = Resources.Load<GameObject>(Constants.PREFAB_FOLDER + Constants.GROUND_PREFAB);
        GameObject groundUnderPrefab = Resources.Load<GameObject>(Constants.PREFAB_FOLDER + Constants.GROUND_UNDER_PREFAB);
		for (int i = 0; i < levels.Length; i++) 
		{
			GameObject newObject = Instantiate(groundPrefab) as GameObject;
            //newObject.transform.SetParent(groundParent);
			newObject.transform.position = new Vector2(i, levels[i]);

			//draws the blocks under the top
			for (int j = 1; j <= Constants.SOIL_HEIGHT; j++)
			{
                GameObject soil = Instantiate(groundUnderPrefab) as GameObject;
                //soil.transform.SetParent(groundParent);
				soil.transform.position = new Vector2(i, levels[i] - j);
			}
		}

        // Spawns enemy castle
		enemyCastle = Instantiate(enemyCastle, new Vector2(levels.Length - 4, levels[levels.Length - 4] + 1), transform.rotation) as GameObject;
		GenerateParallaxObjects ();
	}

    /// <summary>
    /// Gets the next height value based on the previous height, the weight, and randomness
    /// </summary>
    /// <param name="previous">the previous height</param>
    /// <returns>the next height</returns>
    int NextHeight(int previous)
    {
        //use weight to decide if we should change direction or not.
        if (Random.Range(0.00f, 1.00f) <= elevationWeight)
        {
            //change direction
            if (Random.Range(0.00f, 1.00f) >= (heightDifferenceWeight))
            {
                //go down
                return previous - 1;
            }
            else
            {
                //go up
                return previous + 1;
            }
            return previous - (int)Mathf.Sign(Random.Range(0.00f, 1.00f) - heightDifferenceWeight);
        }
        else
        {
            return previous;
        }
    }

    /// <summary>
    /// Generates the parallax objects for the map
    /// </summary>
	void GenerateParallaxObjects()
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
                verticalPosition = Random.Range((float)levels[(int)horizontalPosition] + 3.00f, (float)levels[(int)horizontalPosition] + 11.00f);
                GameObject newObject = Instantiate(cloudPrefabs[Random.Range(0, cloudPrefabs.Length)]) as GameObject;
                newObject.transform.SetParent(bg.transform);
                newObject.transform.position = new Vector3(horizontalPosition, verticalPosition, 2);
                newObject.transform.localScale *= Random.Range(Constants.CLOUD_SCALE_MIN, Constants.CLOUD_SCALE_MAX);
            }
        }
	}

	/// <summary>
	/// Update is called once per frame
	/// </summary>
	void Update()
	{
		//simply rotates the stars to look better
		starrySky.transform.Rotate (0, 0, Constants.STAR_ROTATION_SPEED * Time.deltaTime);

		//moves the sunMoon back and forth and changes direction if it reaches distance to travel from middle
		if (sunMoon.GetComponent<SpriteRenderer> ().sprite == sun) {
			sunMoon.transform.localPosition = new Vector3(sunMoon.transform.localPosition.x + ((cameraHalfWidth * 2 * Constants.QUARTER_CYCLE) / Time.deltaTime), sunMoon.transform.localPosition.y, sunMoon.transform.localPosition.z);
			if (sunMoon.transform.localPosition.x > cameraHalfWidth)
			{
				sunMoon.GetComponent<SpriteRenderer> ().sprite = moon;
			}
		}
		else if (sunMoon.GetComponent<SpriteRenderer> ().sprite == moon) {
            sunMoon.transform.localPosition = new Vector3(sunMoon.transform.localPosition.x - ((cameraHalfWidth * 2 * Constants.QUARTER_CYCLE) / Time.deltaTime), sunMoon.transform.localPosition.y, sunMoon.transform.localPosition.z);
			if (sunMoon.transform.localPosition.x < -cameraHalfWidth)
			{
				sunMoon.GetComponent<SpriteRenderer> ().sprite = sun;
			}
		}

		/*//Check to see if we should start changing to day or night
		if (sunMoon.GetComponent<SpriteRenderer> ().sprite == sun && sunMoon.transform.localPosition.x > cameraHalfWidth - Constants.DISTANCE_TO_START_CHANGE) {
			changeToNight = true;
		} else if (sunMoon.GetComponent<SpriteRenderer> ().sprite == moon && sunMoon.transform.localPosition.x < -cameraHalfWidth + Constants.DISTANCE_TO_START_CHANGE) {
			changeToDay = true;
		}

		//if we should change, change accordingly.
		if (changeToDay) {
			if (runOnce)
			{
				runOnce = false;
				changeMusicToDay = true;
				InvokeRepeating ("ChangeMusic", 0, .1f);
			}
			if (BGM.clip == nightSound)
			{
				BGM.volume -= Constants.BGM_MAX_VOLUME / 5;
			}
			else if (BGM.clip == daySound && BGM.volume <= Constants.BGM_MAX_VOLUME - (Constants.BGM_MAX_VOLUME / 5))
			{
				BGM.volume += Constants.BGM_MAX_VOLUME / 5;
			}
			if (BGM.volume == 0)
			{
				BGM.clip = daySound;
			}

			if (darkness.GetComponent<SpriteRenderer>().color.a > Constants.MIN_DARKNESS_ALPHA + (1 / Constants.LENGTH_OF_CYCLE))
			{
				darkness.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, darkness.GetComponent<SpriteRenderer>().color.a - (1 / Constants.LENGTH_OF_CYCLE));
			} else
			{
				darkness.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, Constants.MIN_DARKNESS_ALPHA);
			}
			if (daySky.GetComponent<SpriteRenderer>().color.a < (1 - (1 / Constants.LENGTH_OF_CYCLE)))
			{
				daySky.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, daySky.GetComponent<SpriteRenderer>().color.a + (1 / Constants.LENGTH_OF_CYCLE));
			}
			else
			{
				daySky.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
				changeToDay = false;
				runOnce = true;
			}
		} else if (changeToNight) {
			if (runOnce)
			{
				runOnce = false;
				changeMusicToDay = false;
				InvokeRepeating ("ChangeMusic", 0, .1f);
			}
			if (BGM.clip == daySound)
			{
				BGM.volume -= Constants.BGM_MAX_VOLUME / 5;
			}
			else if (BGM.clip == nightSound && BGM.volume <= Constants.BGM_MAX_VOLUME - (Constants.BGM_MAX_VOLUME / 5))
			{
				BGM.volume += Constants.BGM_MAX_VOLUME / 5;
			}
			if (BGM.volume == 0)
			{
				BGM.clip = nightSound;
			}

			if (darkness.GetComponent<SpriteRenderer>().color.a < Constants.MAX_DARKNESS_ALPHA - (1 / Constants.LENGTH_OF_CYCLE))
			{
				darkness.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, darkness.GetComponent<SpriteRenderer>().color.a + (1 / Constants.LENGTH_OF_CYCLE));
			} else
			{
				darkness.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, Constants.MAX_DARKNESS_ALPHA);
			}
			if (daySky.GetComponent<SpriteRenderer>().color.a > (1 / Constants.LENGTH_OF_CYCLE))
			{
				daySky.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, daySky.GetComponent<SpriteRenderer>().color.a - (1 / Constants.LENGTH_OF_CYCLE));
			}
			else
			{
				daySky.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
				changeToNight = false;
				runOnce = true;
			}
		}*/
	}

	void ChangeMusic()
	{
		if (changeMusicToDay) {
			if (BGM.clip == nightSound) {
				BGM.volume -= Constants.BGM_MAX_VOLUME / 20;
			} else if (BGM.clip == daySound && BGM.volume <= Constants.BGM_MAX_VOLUME - (Constants.BGM_MAX_VOLUME / 20)) {
				BGM.volume += Constants.BGM_MAX_VOLUME / 20;
			} else {
				CancelInvoke ("ChangeMusic");
			}
			if (BGM.volume <= .05f) {
				BGM.Stop ();
				BGM.clip = daySound;
				BGM.Play ();
			}
		} else {
			if (BGM.clip == daySound)
			{
				BGM.volume -= Constants.BGM_MAX_VOLUME / 20;
			}
			else if (BGM.clip == nightSound && BGM.volume <= Constants.BGM_MAX_VOLUME - (Constants.BGM_MAX_VOLUME / 20))
			{
				BGM.volume += Constants.BGM_MAX_VOLUME / 20;
			}
			else {
				CancelInvoke ("ChangeMusic");
			}
			if (BGM.volume <= .05f)
			{
				BGM.Stop ();
				BGM.clip = nightSound;
				BGM.Play ();
			}
		}
	}

	void GenerateWeatherEffects()
	{
		//fog = Just going to add an image over the canvas and scale its alpha value.
		//thunderstorm = need rain art, and ill simply have it loop over the canvas so it looks like rain is falling. Occasional bright flashes for lightning
	}

}
