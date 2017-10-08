using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {

	public bool usingRaycasts;
	public GameObject player;

	public float movementScale = 3.5f;
	public float clickThreshold = 0.25f;
	float horizontal, vertical, mouseTimeDown;
	Point axisData = new Point ();

	public LayerMask touchInputMask;
	List<GameObject> touchList = new List<GameObject>();
	GameObject[] touchesOld;
	RaycastHit hit;

	// Update is called once per frame
	void Update () {

#if UNITY_EDITOR

		if (usingRaycasts) {
			RaycastInEditor();
		} else {
			TouchInEditor();
		}

#endif

#if !UNITY_STANDALONE

		if (usingRaycasts) {
			RaycastInIOS();
		} else {
			TouchInIOS();
		}

#endif


	}



	void RaycastInEditor()
	{
		if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) {

			touchesOld = new GameObject[touchList.Count];
			touchList.CopyTo(touchesOld);
			touchList.Clear();

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, touchInputMask)) {

				GameObject recipient = hit.transform.gameObject;
				touchList.Add(recipient);

				if (Input.GetMouseButtonDown(0)) {
					recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
				}
				if (Input.GetMouseButtonUp(0)) {
					recipient.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
				}
				if (Input.GetMouseButton(0)) {
					recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
				}

			}

			foreach (GameObject g in touchesOld) {
				if (!touchList.Contains(g)) {
					g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
				}
			}
		}

	}

	void RaycastInIOS() 
	{
		if (Input.touchCount > 0) {

			touchesOld = new GameObject[touchList.Count];
			touchList.CopyTo(touchesOld);
			touchList.Clear();

			foreach (Touch touch in Input.touches) {

				Ray ray = Camera.main.ScreenPointToRay(touch.position);

				if (Physics.Raycast(ray, out hit, touchInputMask)) {

					GameObject recipient = hit.transform.gameObject;
					touchList.Add(recipient);

					if (touch.phase == TouchPhase.Began) {
						recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
					}
					if (touch.phase == TouchPhase.Ended) {
						recipient.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
					}
					if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
						recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
					}
					if (touch.phase == TouchPhase.Canceled) {
						recipient.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
					}

				}
			}

			foreach (GameObject g in touchesOld) {
				if (!touchList.Contains(g)) {
					g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
				}
			}
		}	
	}


	void TouchInEditor()
	{
		if (Input.GetMouseButton (0)) {

			horizontal = Input.mousePosition.x - Camera.main.WorldToScreenPoint(player.transform.position).x;
			vertical = Input.mousePosition.y - Camera.main.WorldToScreenPoint (player.transform.position).y;

			axisData.x = horizontal / Screen.width * movementScale;
			axisData.z = vertical / Screen.height * movementScale;

			player.SendMessage ("OnTouchDown", axisData, SendMessageOptions.DontRequireReceiver);
		}
		if (Input.GetMouseButtonDown(0)) {
			mouseTimeDown = Time.time;
		}
		if (Input.GetMouseButtonUp(0)) {
			if (Time.time - mouseTimeDown <= clickThreshold) {
				player.SendMessage ("OnClick", SendMessageOptions.DontRequireReceiver);
			}
		}

	}

	void TouchInIOS()
	{
		
	}


}
