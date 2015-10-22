using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour 
{	
	GameObject player;
	Vector3 playerLocation;

	GameObject background1;
	GameObject background2;

	float parallaxVar = Constants.PARALLAX_SCALE;
	
	void Start ()
	{
		player = GameObject.Find ("Player");
		playerLocation = player.transform.position;
		background1 = GameObject.Find ("Background1");
		background2 = GameObject.Find ("Background2");

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (playerLocation.x > player.transform.position.x) 
		{
			background1.transform.position = new Vector3(background1.transform.position.x - (1.3f * parallaxVar),
			                                             background1.transform.position.y,
			                                             background1.transform.position.z);
			
			background2.transform.position = new Vector3(background2.transform.position.x - ( 3 * parallaxVar),
			                                             background2.transform.position.y,
			                                             background2.transform.position.z);
		}
		
		else if (playerLocation.x < player.transform.position.x) 
		{
			background1.transform.position = new Vector3(background1.transform.position.x + (1.3f * parallaxVar),
                                                         background1.transform.position.y + (player.transform.position.y - playerLocation.y),
			                                             background1.transform.position.z);
			
			background2.transform.position = new Vector3(background2.transform.position.x + ( 3 * parallaxVar),
                                                         background2.transform.position.y + (player.transform.position.y - playerLocation.y),
			                                             background2.transform.position.z);
		}
		
		playerLocation = player.transform.position;
	}
}
