using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour {

	public float triggerDistance = 8f;
	public List<GameObject> spikes = new List<GameObject>();
	public float minSpikeFallTime, maxSpikeFallTime;

	BoxCollider trigger;
	Rigidbody rb;
	int numberOfSpikes;

	// Use this for initialization
	void Start () {
		trigger = GetComponent<BoxCollider> ();
		SetTriggerDistance ();
		rb = GetComponent<Rigidbody> ();
		numberOfSpikes = spikes.Count;
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
		for (int i = 0; i < numberOfSpikes; i ++) {			
			int index = Random.Range (0, spikes.Count);
			spikes[index].GetComponent<Rigidbody> ().isKinematic = false;
			spikes.Remove (spikes[index]);
			yield return new WaitForSeconds (Random.Range(minSpikeFallTime, maxSpikeFallTime));
		}
		yield return null;
	}

}
