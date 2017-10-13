using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject player;
    public Text coinsText;
    public Text YouWin;
    public static GameManager instance = null;
	public Image healthBar;
	public float healthIncrement, damageIncrement = 0.05f;
	public float speed, speedCap;

	PlayerController controller;
	bool gameEnded = false;
    int currentCheckpoint, coinsCollected = 0;
	Vector3 checkpointPosition;

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
	}
	
	// Update is called once per frame
	void Update () {
		
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
/*
    public void ResetToStart(Collider obj)
	{
		if (obj.CompareTag ("Player")) {
			PlayerController playerController = obj.GetComponent<PlayerController> ();
			obj.transform.position = playerController.startPosition;
			playerController.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			playerController.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		}
	}
*/
    public void ResetToStart()
    {
        if (gameEnded)
        {
            YouWin.GetComponent<Text>().enabled = false;
            gameEnded = false;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        /*GameObject obj = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = obj.GetComponent<PlayerController>();
        obj.transform.position = playerController.startPosition;
        playerController.GetComponent<Rigidbody>().velocity = Vector3.zero;
        playerController.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;*/
    }

	public void ResetToCheckpoint()
	{
		player.transform.position = checkpointPosition;
		player.GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}

    // Consolidated into IncrementCoins()
/*
    public void UpdateCoinsText(int totalCoins)
    {
        coinsText.text = totalCoins.ToString();
    }
*/
    public void PlayerWin()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            YouWin.GetComponent<Text>().enabled = true;
            Invoke("ResetToStart", 5f);
        }
    }

	public void IncreaseHealth()
	{
		controller.SetHealth (controller.GetHealth() + healthIncrement);
		healthBar.fillAmount += healthIncrement;
	}

	public void DamagePlayer() 
	{
		Debug.Log ("Health -1!");
		// decrement health and update UI
		// decide if spikes can hurt player one or more times #decide
		healthBar.fillAmount -= damageIncrement;
		controller.SetHealth (controller.GetHealth() - damageIncrement);
	}

	public void SetCheckpoint(Vector3 checkpoint, int checkpointNum)
	{
		currentCheckpoint = checkpointNum;
		checkpointPosition = checkpoint;
	}

	public int GetLastCheckpoint()
	{
		return currentCheckpoint;
	}
}
