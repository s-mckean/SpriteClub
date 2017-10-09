using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour {

	public float timeUntilFall, evilParticleSpeed;
	public Material fallingMaterial;
	public Color evilParticleColor;
	
	Rigidbody rb;
	ParticleSystem.MainModule particles;
	Renderer render;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		particles = GetComponentInChildren<ParticleSystem> ().main;
		render = gameObject.GetComponent<Renderer> ();
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player")) {
			TurnEvil ();
			StartCoroutine ("Fall");
		}
	}

	void TurnEvil()
	{
		particles.startColor = evilParticleColor;
		particles.startSpeed = -evilParticleSpeed;
	}

	IEnumerator Fall()
	{
		yield return new WaitForSeconds (timeUntilFall);
		render.material = fallingMaterial;
		rb.isKinematic = false;
		yield return null;
	}

}
