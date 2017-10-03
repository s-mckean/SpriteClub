using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 1.75f;
	public float jumpForce = 250f;
	Rigidbody rb;
	bool canJump;

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
		canJump = true;
	}

	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);

		if (Input.GetKeyDown(KeyCode.Space) && canJump) {
			rb.AddForce (Vector3.up * jumpForce);
		}
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ground")) {
			canJump = true;
		}
	}

	void OnTriggerExit(Collider other) 
	{
		if (other.CompareTag("Ground")) {
			canJump = false;
		}
	}

}
