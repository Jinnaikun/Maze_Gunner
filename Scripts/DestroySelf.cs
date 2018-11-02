using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour {


	public float destroyDelay;

	void Start () 
	{
		Destroy (gameObject, destroyDelay);
	}
}
