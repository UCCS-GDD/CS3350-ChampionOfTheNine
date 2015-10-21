using UnityEngine;
using System.Collections;

/// <summary>
/// Script that handles dynamic level generation
/// </summary>
public class DynamicLevelGeneration : MonoBehaviour 
{
	bool debugMode = true;


    [SerializeField]GameObject enemyCastle;
	int[] levels = new int[Constants.MAP_LENGTH];
	float elevationWeight = 1;
	float heightDifferenceWeight = 1;
	float timeOfDay = 0; // 0 - 1 values
	
	/// <summary>
	/// Start is called once on object creation
	/// </summary>
	void Start () {
		//elevationWeight = Random.Range (.25f, .50f);
		//heightDifferenceWeight = Random.Range (0.00f, 1.2f);
		elevationWeight = Constants.ELEVATION_CHANGE_WEIGHT + Random.Range (-Constants.ELEVATION_CHANGE_OFFSET, Constants.ELEVATION_CHANGE_OFFSET);;
		heightDifferenceWeight = Constants.HEIGHT_DIFFERENCE_WEIGHT + Random.Range (-Constants.HEIGHT_DIFFERENCE_OFFSET, Constants.HEIGHT_DIFFERENCE_OFFSET);
		timeOfDay = Random.Range (0.00f, .75f);
		if (timeOfDay < .35) {
			timeOfDay = 0;
		}

		GameObject.Find ("Darkness").GetComponent<SpriteRenderer> ().color = new Color (0, 0, 0, timeOfDay);

		if (debugMode) {
			Debug.Log ("Elevation weight: " + elevationWeight);
			Debug.Log ("Height Difference Weight: " + heightDifferenceWeight);
			Debug.Log ("Time of day: " + timeOfDay);
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

		DrawMap ();
	}

    /// <summary>
    /// Gets the next height value based on the previous height, the weight, and randomness
    /// </summary>
    /// <param name="previous">the previous height</param>
    /// <returns>the next height</returns>
	int NextHeight(int previous)
	{
		//use weight to decide if we should change direction or not.
		if (Random.Range (0.00f, 1.00f) <= elevationWeight) {
			//change direction
			if (Random.Range (0.00f, 1.00f) >= (heightDifferenceWeight))
			{
				//go down
				return previous - 1;
			} 
			else
			{
				//go up
				return previous + 1;
			}
		} else {
			return previous;
		}
	}

    /// <summary>
    /// Places objects to create the map based on the generated terrain levels
    /// </summary>
	void DrawMap()
	{
		Transform groundParent = GameObject.Find ("Ground").transform;

		//draws each of the top blocks
        GameObject groundPrefab = Resources.Load<GameObject>(Constants.PREFAB_FOLDER + Constants.GROUND_PREFAB);
        GameObject groundUnderPrefab = Resources.Load<GameObject>(Constants.PREFAB_FOLDER + Constants.GROUND_UNDER_PREFAB);
		for (int i = 0; i < levels.Length; i++) 
		{
			GameObject newObject = Instantiate(groundPrefab) as GameObject;
			newObject.transform.SetParent(groundParent);
			newObject.transform.position = new Vector2(i, levels[i]);

			//draws the blocks under the top
			for (int j = 1; j <= Constants.SOIL_HEIGHT; j++)
			{
                GameObject soil = Instantiate(groundUnderPrefab) as GameObject;
				soil.transform.SetParent(groundParent);
				soil.transform.position = new Vector2(i, levels[i] - j);
			}
		}

        // Spawns enemy castle
		enemyCastle = Instantiate(enemyCastle, new Vector2(levels.Length - 4, levels[levels.Length - 4] + 1), transform.rotation) as GameObject;
		GenerateParallaxObjects ();
	}

    /// <summary>
    /// Generates the parallax objects for the map
    /// </summary>
	void GenerateParallaxObjects()
	{
		float horizontalPosition = 0;
		float verticalPosition = 0;

		//generate clouds on background2
		for (int i = 0; i < (int)(Constants.MAP_LENGTH * Constants.CLOUD_DENSITY); i++) 
        {
            horizontalPosition = Random.Range(0, Constants.MAP_LENGTH);
			verticalPosition = Random.Range ((float)levels[(int)horizontalPosition] - 10.00f, (float)levels[(int)horizontalPosition] + 20.00f);
			GameObject newObject = Instantiate (Resources.Load ("Prefabs/Cloud" + Random.Range (1, 4).ToString())) as GameObject;
			newObject.transform.SetParent(GameObject.Find ("Background2").transform);
			newObject.transform.position = new Vector3(horizontalPosition, verticalPosition, 2);
			newObject.transform.localScale = newObject.transform.localScale * Random.Range (Constants.CLOUD_SCALE_MIN, Constants.CLOUD_SCALE_MAX);
		}

		//generate clouds on background1
        for (int i = 0; i < (int)(Constants.MAP_LENGTH * Constants.CLOUD_DENSITY); i++)
        {
            horizontalPosition = Random.Range(0, Constants.MAP_LENGTH);
			verticalPosition = Random.Range ((float)levels[(int)horizontalPosition] - 10.00f, (float)levels[(int)horizontalPosition] + 20.00f);
			GameObject newObject = Instantiate (Resources.Load ("Prefabs/Cloud" + Random.Range (1, 4).ToString())) as GameObject;
			newObject.transform.SetParent(GameObject.Find ("Background1").transform);
			newObject.transform.position = new Vector3(horizontalPosition, verticalPosition, 1);
			newObject.transform.localScale = newObject.transform.localScale * Random.Range (Constants.CLOUD_SCALE_MIN, Constants.CLOUD_SCALE_MAX);
		}
	}

	void GenerateBackgroundImage()
	{
		//Basically just going to load whatever image is selected by the random generator
		//will tie in with the map details
	}

	void GenerateWeatherEffects()
	{
		//fog = Just going to add an image over the canvas and scale its alpha value.
		//thunderstorm = need rain art, and ill simply have it loop over the canvas so it looks like rain is falling. Occasional bright flashes for lightning
	}

}
