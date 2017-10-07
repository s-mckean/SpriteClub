using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed, jumpForce, maxDepth;
	Rigidbody rb;
	bool canJump;
    int coinsCollected = 0;

	public bool restrictingDepth;
    public Vector3 startPosition;

	// touchscreen vars
	Vector2 touchOrigin = -Vector2.one;

	// Use this for initialization
	void Start () 
	{
        startPosition = transform.position;
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

		#if UNITY_IOS

		if(Input.touchCount > 0)
		{
			foreach(Touch t in Input.touches)
			{
				if(t.phase == TouchPhase.Began)
				{
					Debug.Log("Tap worked");
				}
				else if(t.phase == TouchPhase.Stationary)
				{
					Debug.Log("Hold worked");
				}
			}
		}


		float pointer_x = Input.GetAxis("Mouse X");
		float pointer_y = Input.GetAxis("Mouse Y");

		if (Input.touchCount > 0)
		{
			pointer_x = Input.touches[0].deltaPosition.x;
			pointer_y = Input.touches[0].deltaPosition.y;
		}

		#endif
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
        if (other.CompareTag("Ring"))
        {
            Destroy(other.gameObject);
            IncrementCoins();
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

    void IncrementCoins()
    {
        coinsCollected++;
        GameManager.instance.UpdateCoinsText(coinsCollected);
    }

}
