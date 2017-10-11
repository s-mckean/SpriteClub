using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour {

	public float triggerDistance = 8f;
	public GameObject[] spikes;

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
			if (transform.childCount == 0) {
				rb.isKinematic = false;
			} else {
				StartCoroutine ("FallRandomly");
			}
		}
	}

	IEnumerator FallRandomly()
	{
		foreach (GameObject child in spikes) {
			child.GetComponent<Rigidbody> ().isKinematic = false;
			child.transform.parent = null;
			yield return new WaitForSeconds (Random.Range(0.0f, 0.1f));
		}
		yield return null;
	}

}
