using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player")) {
			return;
		}	

		Vector3 position = other.transform.position;
		// assuming levels only move right for now in demo..
		if (GameManager.instance.GetCheckpointPosition ().x < position.x) {
			GameManager.instance.SetCheckpoint (position);
		}

	}

}
