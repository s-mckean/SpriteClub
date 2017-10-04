using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed, jumpForce, maxDepth;
	Rigidbody rb;
	bool canJump;

	public bool restrictingDepth;
    public Vector3 startPosition;

	// Use this for initialization
	void Start () 
	{
        startPosition = transform.position;
		rb = GetComponent<Rigidbody> ();
		speed = 1.75f;
		jumpForce = 250f;
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

		if (restrictingDepth) {
			ClampPosition ();
		}

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

	void ClampPosition()
	{
		Vector3 clampedPosition = transform.position;
		clampedPosition.z = Mathf.Clamp (transform.position.z, -maxDepth, maxDepth);
		transform.position = clampedPosition;
	}

}
