﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public GameObject player, deathParticles, fireworks;
	public Text coinsText;
	public GameObject YouWin;
	public static GameManager instance = null;
	public  GameObject boostParent;
	public Image healthBar, boostBar;
	public float healthIncrement, damageIncrement = 0.05f, nextLevelDelay = 5f;
	public float speed, speedCap, restartDelay;

	PlayerController controller;
	bool gameEnded = false, restartLevelOnFall = false, canSubtract = false;
	int currentCheckpoint, coinsCollected = 0, numberOfBoostBars = 0, maxBoostBars, healthCounter = 0;
	float healthBarWidth, boostBarWidth;
	Vector3 checkpointPosition, boostBarOrigin;
	List<Image> boostBars = new List<Image> ();

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
	}

	// Use this for initialization
	void Start () {
		controller = player.GetComponent<PlayerController> ();
		checkpointPosition = player.transform.position;
		healthBar.fillAmount = 0;
		healthBarWidth = healthBar.GetComponent<RectTransform> ().sizeDelta.x;
		boostBarWidth = boostBar.GetComponent<RectTransform> ().sizeDelta.x;
		maxBoostBars = Mathf.RoundToInt(healthBarWidth / boostBarWidth);
		boostBarOrigin = boostBar.GetComponent<RectTransform> ().anchoredPosition3D;
	}

	public void IncrementCoins()
	{
		coinsCollected++;

		if (speed < speedCap)
		{
			speed++;
		}
		controller.PartyTime ();
		coinsText.text = coinsCollected.ToString();
	}

	public void ResetToStart()
	{
		if (gameEnded)
		{
			YouWin.SetActive(false);
			gameEnded = false;
		}
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ResetToCheckpoint()
	{
		if (restartLevelOnFall) {
			ResetToStart ();
		}
		player.transform.position = checkpointPosition;
		player.GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}

	public void PlayerWin()
	{
		if (fireworks != null)
			fireworks.SetActive (true);
		if (!gameEnded)
		{
			gameEnded = true;
			YouWin.SetActive(true);
		}
	}

	public void IncreaseHealth()
	{
		controller.SetHealth (controller.GetHealth() + healthIncrement);
		healthBar.fillAmount += healthIncrement;
		AddBoostBars ();
	}

	public void DamagePlayer(float multiplier = 1f) 
	{
		if (healthBar.fillAmount < damageIncrement) {
			KillPlayer ();
		}

		healthBar.fillAmount -= multiplier * damageIncrement;
		controller.SetHealth (controller.GetHealth() - multiplier * damageIncrement);
		SubtractBoostBars ();
	}

	void KillPlayer()
	{
		Vector3 deathPosition = player.transform.position;
		Destroy(player.gameObject);
		Instantiate (deathParticles, deathPosition, Quaternion.identity);
		Invoke("ResetToStart", restartDelay);
	}

	public void SetCheckpoint(Vector3 checkpoint, int checkpointNum, bool restart)
	{
		currentCheckpoint = checkpointNum;
		checkpointPosition = checkpoint;
		restartLevelOnFall = restart;
	}

	public int GetLastCheckpoint()
	{
		return currentCheckpoint;
	}

	public void ChangeScene()
	{
		int totalScenes = SceneManager.sceneCountInBuildSettings;
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

		if (currentSceneIndex == totalScenes - 1) PlayerWin();
		else  {
			if (fireworks != null) fireworks.SetActive (true);
			Invoke ("LoadNextScene", nextLevelDelay);
		}

	}

	void LoadNextScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void GameOverButtons(bool restart)
	{
		if (restart) SceneManager.LoadScene(0);
		else Application.Quit();
	}

	public void AddBoostBars()
	{
		if (numberOfBoostBars < maxBoostBars) {

			float boostChunk = damageIncrement * controller.boostDamageMultiplier;
			float newBarHealth = (numberOfBoostBars + 1) * boostChunk;

			for (int i = 0; i < boostBars.Count; i++) {
				Vector2 barPosition = boostBars [i].GetComponent<RectTransform> ().anchoredPosition;
				barPosition.x += GameManager.instance.damageIncrement * healthBarWidth;
				boostBars [i].GetComponent<RectTransform> ().anchoredPosition = barPosition;
			}
			if (controller.GetHealth () >= newBarHealth) {
				CreateNewBar ();
				numberOfBoostBars++;
				canSubtract = true;
			} else {
				canSubtract = false;
			}

			if (healthCounter > 0) {
				healthCounter--;
			}

		}
	}

	public void SubtractBoostBars()
	{
		if (boostBars.Count > 0) {

			if (canSubtract) {

				healthCounter++;
				switch (healthCounter) {

				case 1:
					Destroy (boostBars [boostBars.Count - 1].gameObject);
					boostBars.RemoveAt (boostBars.Count - 1);
					numberOfBoostBars--;
					break;
				case 3:
					healthCounter = 0;
					break;
				default:
					break;

				}
			}

			for (int i = 0; i < boostBars.Count; i++) {
				Vector2 barPosition = boostBars [i].GetComponent<RectTransform> ().anchoredPosition;
				barPosition.x -= GameManager.instance.damageIncrement * healthBarWidth;
				boostBars [i].GetComponent<RectTransform> ().anchoredPosition = barPosition;
			}
		}
	}

	public void SubtractOneBoost()
	{
        if (boostBars.Count > 0)
        {
            Destroy(boostBars[boostBars.Count - 1].gameObject);
            boostBars.RemoveAt(boostBars.Count - 1);
            numberOfBoostBars--;

            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < boostBars.Count; i++)
                {
                    Vector2 barPosition = boostBars[i].GetComponent<RectTransform>().anchoredPosition;
                    barPosition.x -= GameManager.instance.damageIncrement * healthBarWidth;
                    boostBars[i].GetComponent<RectTransform>().anchoredPosition = barPosition;
                }
            }
        }
	}

	void CreateNewBar()
	{
		Image newBar = Instantiate (boostBar, boostBarOrigin, Quaternion.identity);
		newBar.transform.SetParent (boostParent.transform);
		newBar.GetComponent<RectTransform> ().anchoredPosition = boostBarOrigin;
		ResetBarToOrigin (newBar);
		boostBars.Add (newBar);
	}

	void ResetBarToOrigin(Image bar)
	{
		RectTransform rect = bar.GetComponent<RectTransform> ();

		Quaternion rot = rect.localRotation;
		rot.x = 0f;
		rect.localRotation = rot;

		Vector3 pos = rect.anchoredPosition3D;
		pos.z = 0f;
		rect.anchoredPosition3D = pos;

		Vector3 scale = rect.localScale;
		scale = Vector3.one;
		rect.localScale = scale;
	}


}
