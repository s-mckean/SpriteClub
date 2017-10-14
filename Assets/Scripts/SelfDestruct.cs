using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {

	public float destroyTime = 1.5f;

	void OnTriggerEnter(Collider other)
	{
		if (this.CompareTag ("Bottom")) { 
			return;
		}

		if (other.CompareTag("Ground")) {
			Destroy (this.gameObject, destroyTime);
		}
	}
}
