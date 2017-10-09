using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthController : MonoBehaviour {

	PlayerController controller;
	public float newMaxDepth = 2.25f;

	float initialMaxDepth;

	void Start() {
		controller = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		initialMaxDepth = controller.maxDepth;
	}

	void OnTriggerEnter(Collider other)
	{
		if (gameObject.CompareTag("Entrance")) {
			controller.maxDepth = newMaxDepth;
		}
		if (gameObject.CompareTag("Exit")) {
			controller.maxDepth = initialMaxDepth;
		}
	}

}
