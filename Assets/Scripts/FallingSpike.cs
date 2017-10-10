using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour {

	public float triggerDistance = 8f;

	BoxCollider trigger;
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		trigger = GetComponent<BoxCollider> ();
		SetTriggerDistance ();
		rb = GetComponent<Rigidbody> ();
	}
	
	void SetTriggerDistance()
	{
		Vector3 triggerPosition = trigger.center;
		triggerPosition.x = -triggerDistance;
		trigger.center = triggerPosition;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) {
			rb.isKinematic = false;
		}
	}
}
