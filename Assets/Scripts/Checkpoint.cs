using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public int checkpointNumber;

	void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player")) {
			return;
		}	

		Vector3 position = other.transform.position;
		// assuming levels only move right for now in demo..
		if (GameManager.instance.GetLastCheckpoint () < checkpointNumber) {
			GameManager.instance.SetCheckpoint (position, checkpointNumber);
		}

	}

}
