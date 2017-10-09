using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject hero;
    public Text coinsText;
    public Text YouWin;
    public static GameManager instance = null;

    public float speed, speedCap;
    private bool gameEnded = false;
    private int coinsCollected = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
		
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
}
