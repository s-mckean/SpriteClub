using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

	public float rotationSpeed = 65;

	// Update is called once per frame
	void FixedUpdate () {
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
	}

}
