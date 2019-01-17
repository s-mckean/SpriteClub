using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitOfDespair : MonoBehaviour {

	public float resetDelay = 1f;
	
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player")) {
			StartCoroutine (ResetIn(other));
		}
	}

    IEnumerator ResetIn(Collider other)
    {
        yield return new WaitForSeconds(resetDelay);
        if (other.CompareTag("Player"))
        {
            GameManager.instance.ResetToStart();
        }
	}


}
