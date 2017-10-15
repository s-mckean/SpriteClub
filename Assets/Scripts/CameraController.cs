using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
	public float heightAbovePlayer = 5f;
	public float cameraDistance = 11f;

    Vector3 offset;

	// Use this for initialization
	void Start () {
		SyncWithPlayer ();
        offset = transform.position - player.transform.position;
	}

	void SyncWithPlayer()
	{
		Vector3 startingPosition = transform.position;
		startingPosition.x = player.transform.position.x;
		startingPosition.y = player.transform.position.y + heightAbovePlayer;
		startingPosition.z = -cameraDistance;
		transform.position = startingPosition;
	}

	void LateUpdate () {
		if (player != null) transform.position = player.transform.position + offset;
	}
}
