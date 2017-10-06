using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject hero;
    public Text coinsText;
    public static GameManager instance = null;

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

	public static void ResetToStart(Collider obj)
	{
		if (obj.CompareTag ("Player")) {
			PlayerController playerController = obj.GetComponent<PlayerController> ();
			obj.transform.position = playerController.startPosition;
			playerController.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			playerController.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		}
	}

    public void UpdateCoinsText(int totalCoins)
    {
		coinsText.text = totalCoins.ToString();
    }
}
