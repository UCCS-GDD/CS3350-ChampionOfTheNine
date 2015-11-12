using UnityEngine;
using System.Collections;

public class BloodParticle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("DeathClock", 2);
	}

	void DeathClock()
	{
		Destroy (gameObject);
	}
}
