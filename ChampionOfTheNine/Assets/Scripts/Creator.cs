using UnityEngine;
using System.Collections;

public class Creator : MonoBehaviour {

	GameObject character;
	bool first = true;
	public GameObject knight;
	public GameObject ranged;
	public GameObject mage;

	public void Warrior()
	{
		if(first)
		{
			character = Instantiate(knight);
			first = false;
		}
		else
		{
			Destroy(character);
			character = Instantiate(knight);

		}
	}
	public void Ranger()
	{
		if(first)
		{
			character = Instantiate(ranged);
			first = false;
		}
		else
		{
			Destroy(character);
			character = Instantiate(ranged);
				
		}
	}
	public void Mage()
	{
		if(first)
		{
			character = Instantiate(mage);
			first = false;
		}
		else
		{
			Destroy(character);
			character = Instantiate(mage);
				
		}
	}
	public void Red(float newRed)
	{

	}
	
	public void Green(float newGreen)
	{

	}
	
	public void Blue(float newBlue)
	{

	}
}
