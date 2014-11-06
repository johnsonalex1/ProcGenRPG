using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		float mousePosX = Input.mousePosition.x;
		float mousePosY = Input.mousePosition.y;
		float screenX = Screen.width;
		float screenY = Screen.height;
		float angle;
		if (mousePosY < screenY/2) {
			angle = Mathf.Rad2Deg * Mathf.Atan(((mousePosX/screenX*2) - 1)/((mousePosY/screenY*2) - 1)) + 45 + 180;
		} else {
			angle = Mathf.Rad2Deg * Mathf.Atan(((mousePosX/screenX*2) - 1)/((mousePosY/screenY*2) - 1)) + 45;
		}
		transform.eulerAngles = new Vector3(0f, angle, 0f);
	}
}
