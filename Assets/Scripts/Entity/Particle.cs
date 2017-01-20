using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {


	public float life;
	void Start () {

	}

	// Update is called once per frame
	void Update ()
	{
		Destroy(gameObject, life);
	}
}
