using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Moved speed and speedCap to GameManager since they need to be accessible to other scripts
	public float jumpForce, maxDepth, partySpeed, fadeTime;
	public bool restrictingDepth;
	public Vector3 startPosition;
	public Color defaultColor, targetColor;
	Rigidbody rb;
	float radius, health;
	Collider[] colliders;

	Renderer renderer;
	float timeLeft = 1.0f;
	bool partying = false;

	// Use this for initialization
	void Start () 
	{
		radius = GetComponent<SphereCollider> ().radius + 0.2f;
		startPosition = transform.position;
		rb = GetComponent<Rigidbody> ();
		health = 0f;
		renderer = GetComponent<Renderer> ();
		renderer.sharedMaterial.color = defaultColor;
	}

	void FixedUpdate()
	{

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		if (restrictingDepth) {
			ClampPosition ();
		}

		rb.AddForce (movement * GameManager.instance.speed);

		if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
			rb.AddForce (Vector3.up * jumpForce);
		}

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Boost(movement);
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
	}

	// Moved collision detection to CoinController and EndZone
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy") && !other.isTrigger) {
			GameManager.instance.DamagePlayer ();
		}
	}

	void ClampPosition()
	{
		Vector3 clampedPosition = transform.position;
		clampedPosition.z = Mathf.Clamp (transform.position.z, -maxDepth, maxDepth);
		transform.position = clampedPosition;
	}

	public float GetHealth()
	{
		return health;
	}

	public void SetHealth(float newHealth)
	{
		health = newHealth;
	}

	public void PartyTime()
	{
		if (!partying) {
			StartCoroutine ("LerpIt");
		}
	}

	IEnumerator LerpIt()
	{
		partying = true;
		while (timeLeft > Time.deltaTime) {
			renderer.sharedMaterial.color = Color.Lerp (defaultColor, targetColor, Time.deltaTime * partySpeed / timeLeft);
			timeLeft -= Time.deltaTime * partySpeed;
			yield return null;
		}

		timeLeft = 1.0f;

		float hangover = partySpeed / fadeTime;
		while (timeLeft > Time.deltaTime) {
			renderer.sharedMaterial.color = Color.Lerp (targetColor, defaultColor, Time.deltaTime * hangover  / timeLeft);
			timeLeft -= Time.deltaTime * hangover;
			yield return null;
		}

		renderer.sharedMaterial.color = defaultColor;
		timeLeft = 1.0f;
		partying = false;
	}

	void OnTouchDown(Point axisData) 
	{
		Vector3 movement = new Vector3 (axisData.x, 0f, axisData.z);
		rb.AddForce (movement * GameManager.instance.speed);
	}

	void OnClick()
	{
		if (IsGrounded()) {
			rb.AddForce (Vector3.up * jumpForce);
		}
	}

    void Boost(Vector3 movement)
    {
            if (health > 0)
            {
                Debug.Log("Boost activate!");
                rb.AddForce(movement * 60);
            }
   
    }
	public void cannonBoost(Vector3 BlastForce) {
		rb.AddForce(BlastForce);
	}
}