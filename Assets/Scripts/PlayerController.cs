﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed, jumpForce, maxDepth;
	public bool restrictingDepth;
	public Vector3 startPosition;
    public float speedCap;

	Rigidbody rb;
	float radius, distanceToGround;
    int coinsCollected = 0;

	Collider[] colliders;

	// Use this for initialization
	void Start () 
	{
		radius = GetComponent<SphereCollider> ().radius + 0.2f;
        startPosition = transform.position;
		rb = GetComponent<Rigidbody> ();
		distanceToGround = GetComponent<Collider> ().bounds.extents.y;
	}

	void FixedUpdate()
	{

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		if (restrictingDepth) {
			ClampPosition ();
		}

		rb.AddForce (movement * speed);

		if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
			rb.AddForce (Vector3.up * jumpForce);
		}

	}

	// Update is called once per frame
	void Update () {

	}

	bool IsGrounded()
	{
		colliders = Physics.OverlapSphere (transform.position, radius);
		foreach (Collider collider in colliders) {
			if (collider.name != "Player" && collider.CompareTag ("Ground"))
				return true;
		}
		return false;
//		return Physics.Raycast (transform.position, Vector3.down, distanceToGround + 0.1f);
	}

	void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("Ring")) {
            Destroy(other.gameObject);
            IncrementCoins();
        }
        if (other.CompareTag("Exit"))
        {
            Debug.Log("You Win");
        }
	}

	void ClampPosition()
	{
		Vector3 clampedPosition = transform.position;
		clampedPosition.z = Mathf.Clamp (transform.position.z, -maxDepth, maxDepth);
		transform.position = clampedPosition;
	}

    void IncrementCoins()
    {
        coinsCollected++;

        if (speed < speedCap)
        {
            speed++;
        }

        GameManager.instance.UpdateCoinsText(coinsCollected);
    }

	void OnTouchDown(Point axisData) 
	{
		Vector3 movement = new Vector3 (axisData.x, 0f, axisData.z);
		rb.AddForce (movement * speed);
	}

	void OnClick()
	{
		if (IsGrounded()) {
			rb.AddForce (Vector3.up * jumpForce);
		}
	}

}
