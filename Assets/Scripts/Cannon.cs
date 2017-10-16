using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {
	public GameObject player;
	private Vector3 eulerAngles;
	private Vector3 cannonBlastForce;
	private float yValue;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		eulerAngles = transform.rotation.eulerAngles;
		yValue = ((1000f * (eulerAngles.x)) / 90);
		Debug.Log (eulerAngles.x);
	}

	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			Debug.Log ("Gottem");
			player.GetComponent<PlayerController>().cannonBoost(new Vector3 (2000f, yValue, 0.0f));
			Debug.Log (yValue);
		}
	}
}
