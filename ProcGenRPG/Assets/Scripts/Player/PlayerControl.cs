using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rigidbody.AddForce((transform.forward + transform.right) * speed * Input.GetAxis("Vertical") * 60, ForceMode.Force);
		rigidbody.AddForce((transform.right - transform.forward) * speed * Input.GetAxis("Horizontal") * 50, ForceMode.Force);
	}
}
