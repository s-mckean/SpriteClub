using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {
	public GameObject player;
	private Vector3 eulerAngles;
	private Vector3 cannonBlastForce;
	private float yValue;
	private float xValue;
	private GameObject wheel;
	private float angle;
	private float blastForce = 4500;
	// Use this for initialization
	void Start () {
		wheel = GameObject.FindGameObjectWithTag ("Wheel");
		player = GameObject.FindGameObjectWithTag ("Player");
		angle = wheel.transform.rotation.eulerAngles.z;
		yValue = (angle / 90) * blastForce;
		Debug.Log (angle);
		xValue = ((Mathf.Abs(angle - 90)) / 90) * blastForce;
		Debug.Log (wheel.transform.rotation.eulerAngles.z);
	}

	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			Debug.Log ("Gottem");
			player.GetComponent<PlayerController> ().transform.position = wheel.transform.position;
			player.GetComponent<PlayerController>().cannonBoost(new Vector3 (xValue, yValue, 0.0f));
			Debug.Log (yValue);
		}
	}
}
