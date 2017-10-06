using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float rotationSpeed = 65;
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
	}
}
