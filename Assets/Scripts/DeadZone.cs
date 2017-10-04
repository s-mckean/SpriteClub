using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        ResetToStart(other);
    }

    private void ResetToStart(Collider obj)
    {
        PlayerController playerController = obj.GetComponent<PlayerController>();
        obj.transform.position = playerController.startPosition;
        playerController.GetComponent<Rigidbody>().velocity = Vector3.zero;
        playerController.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
