using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

	public float damage;
	public float duration;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody.AddForce(transform.forward*3, ForceMode.VelocityChange);
		duration--;
		if(duration < 0) {
			Destroy(this.gameObject);
		}
	}
}
