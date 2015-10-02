using UnityEngine;
using System.Collections;

public class DynamicLevelGeneration : MonoBehaviour 
{
    [SerializeField]GameObject enemyCastle;
	int[] levels = new int[100];
	float elevationWeight = 1;

	// Use this for initialization
	void Start () {
		elevationWeight = Random.Range (.25f, .50f);
        //Debug.Log (elevationWeight);

		//creates "platform" for the castle on the left
		for (int i = 0; i < 10; i++)
		{
			levels[i] = 5;
		}

		//fills in the rest of the aray.
		for (int i = 10; i < 90; i++) 
		{
			levels[i] = NextHeight(levels[i - 1]);
		}

		//creates platform for right castle
		for (int i = 90; i < 100; i++) 
		{
			levels[i] = levels[89];
		}

		DrawMap ();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="previous"></param>
    /// <returns></returns>
	int NextHeight(int previous)
	{
		//use weight to decide if we should change direction or not.
		if (Random.Range (0.00f, 1.00f) <= elevationWeight) {
			//change direction
			if (Random.Range (0.00f, 10.00f) >= (10 - previous))
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
    /// 
    /// </summary>
	void DrawMap()
	{
		Transform groundParent = GameObject.Find ("Ground").transform;

		//draws each of the top blocks
		for (int i = 0; i < levels.Length; i++) 
		{
			GameObject newObject = Instantiate (Resources.Load ("Prefabs/ground")) as GameObject;
			newObject.transform.SetParent(groundParent);
			newObject.transform.position = new Vector2(i, levels[i]);

			//draws the blocks under the top
			for (int j = 0; j < levels[i]; j++)
			{
				GameObject soil = Instantiate (Resources.Load ("Prefabs/rocks")) as GameObject;
				soil.transform.SetParent(groundParent);
				soil.transform.position = new Vector2(i, j);
			}
		}

        // Spawns enemy castle
        Instantiate(enemyCastle, new Vector2(levels.Length - 4, levels[levels.Length - 4] + 1), transform.rotation);
	}

	void GenerateParallaxObjects()
	{
		//create a background and a foreground plane and place arbitrary objects at different location intervals
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
