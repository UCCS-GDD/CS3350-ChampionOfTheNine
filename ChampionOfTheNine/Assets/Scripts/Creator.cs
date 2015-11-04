using UnityEngine;
using System.Collections;

public class Creator : MonoBehaviour {

	GameObject character;
    GameObject playable;
	bool first = true;
	public GameObject knight;
	public GameObject ranged;
	public GameObject mage;
    public GameObject knightPlay;
    public GameObject rangedPlay;
    public GameObject magePlay;
    

	public void Warrior()
	{
		if(first)
		{
			character = Instantiate(knight);
            playable = Instantiate(rangedPlay, new Vector3(0f, -3.9f), Quaternion.identity) as GameObject;
            GameManager.Instance.Type = GameManager.CharacterTypes.Knight;
			first = false;
		}
		else
		{
			Destroy(character);
			character = Instantiate(knight);
            Destroy(playable);
            playable = Instantiate(rangedPlay, new Vector3(0f, -3.9f), Quaternion.identity) as GameObject;
            GameManager.Instance.Type = GameManager.CharacterTypes.Knight;
		}
	}
	public void Ranger()
	{
		if(first)
		{
			character = Instantiate(ranged);
            playable = Instantiate(rangedPlay, new Vector3(0f, -3.9f), Quaternion.identity) as GameObject;
            GameManager.Instance.Type = GameManager.CharacterTypes.Ranger;
			first = false;
		}
		else
		{
			Destroy(character);
			character = Instantiate(ranged);
            Destroy(playable);
            playable = Instantiate(rangedPlay, new Vector3(0f, -3.9f), Quaternion.identity) as GameObject;
            GameManager.Instance.Type = GameManager.CharacterTypes.Ranger;
		}
	}
	public void Mage()
	{
		if(first)
		{
			character = Instantiate(mage);
            playable = Instantiate(magePlay, new Vector3(0f, -3.9f), Quaternion.identity) as GameObject;
            GameManager.Instance.Type = GameManager.CharacterTypes.Mage;
            first = false;
		}
		else
		{
			Destroy(character);
			character = Instantiate(mage);
            Destroy(playable);
            playable = Instantiate(magePlay, new Vector3(0f, -3.9f), Quaternion.identity) as GameObject;
            GameManager.Instance.Type = GameManager.CharacterTypes.Mage;
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
