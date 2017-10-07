﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

	public float rotationSpeed = 65;
	public Color defaultColor;
	public Color selectedColor;
	Material mat;

	void Start() {
		mat = GetComponent<Renderer> ().material;
	}

	// Update is called once per frame
	void FixedUpdate () {
        transform.Rotate(Vector3.back, Time.deltaTime * rotationSpeed);
	}

	void OnTouchDown() 
	{
		mat.color = selectedColor;
	}

	void OnTouchUp() 
	{
		mat.color = defaultColor;
	}

	void OnTouchStay() 
	{
		mat.color = selectedColor;
	}

	void OnTouchExit()
	{
		mat.color = defaultColor;
	}

}
