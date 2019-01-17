using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Moved speed and speedCap to GameManager since they need to be accessible to other scripts
    public float jumpForce = 375f, maxDepth = 1f, partySpeed = 10f, fadeTime = 6f, boostDamageMultiplier = 3f, boostSpeed = 120f, boostFadeTime = 3f;
    public bool restrictingDepth;
    public Vector3 startPosition;
    public Color defaultColor, targetColor;

    Rigidbody rb;
    float radius, health, damageIncrement;
    Collider[] colliders;

	Renderer render;
    float timeLeft = 1.0f;
    bool oneTime, partying = false;

    // Use this for initialization
    void Start()
    {
        radius = GetComponent<SphereCollider>().radius + 0.2f;
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        health = 0f;
		damageIncrement = GameManager.instance.damageIncrement;
        render = GetComponent<Renderer>();
        render.sharedMaterial.color = defaultColor;
    }

    void FixedUpdate()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (restrictingDepth)
        {
            ClampPosition();
        }

        rb.AddForce(movement * GameManager.instance.speed);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Boost(movement);
        }


    }

    bool IsGrounded()
    {
        colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            if (collider.name != "Player" && collider.CompareTag("Ground"))
                return true;
        }
        return false;
    }

    // Moved collision detection to CoinController and EndZone
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !other.isTrigger)
        {
            GameManager.instance.DamagePlayer();
        }
    }

    void ClampPosition()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.z = Mathf.Clamp(transform.position.z, -maxDepth, maxDepth);
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
        if (!partying)
        {
            StartCoroutine("LerpIt");
        }
    }

    IEnumerator LerpIt()
    {
        partying = true;
        while (timeLeft > Time.deltaTime)
        {
            render.sharedMaterial.color = Color.Lerp(defaultColor, targetColor, Time.deltaTime * partySpeed / timeLeft);
            timeLeft -= Time.deltaTime * partySpeed;
            yield return null;
        }

        timeLeft = 1.0f;

        float hangover = partySpeed / fadeTime;
        while (timeLeft > Time.deltaTime)
        {
            render.sharedMaterial.color = Color.Lerp(targetColor, defaultColor, Time.deltaTime * hangover / timeLeft);
            timeLeft -= Time.deltaTime * hangover;
            yield return null;
        }

        render.sharedMaterial.color = defaultColor;
        timeLeft = 1.0f;
        partying = false;
    }

    void OnTouchDown(Point axisData)
    {
        Vector3 movement = new Vector3(axisData.x, 0f, axisData.z);
        rb.AddForce(movement * GameManager.instance.speed);
    }

    void OnClick()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce);
        }
    }

    void Boost(Vector3 movement)
    {
		
		if (health > boostDamageMultiplier * damageIncrement)
        {
			GameManager.instance.DamagePlayer (boostDamageMultiplier);
            rb.AddForce(movement * boostSpeed);
			GameManager.instance.SubtractOneBoost ();
			StartCoroutine("BoostTrail");
        }
    }

    IEnumerator BoostTrail()
    {
        GetComponent<TrailRenderer>().enabled = true;
		yield return new WaitForSeconds(boostFadeTime);
        GetComponent<TrailRenderer>().enabled = false;
        yield return null;
    }
	public void cannonBoost(Vector3 BlastForce) 
	{
		rb.AddForce(BlastForce);
	}

}
