using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	public float speed;

	private Animator playerAnim;

	// Use this for initialization
	void Start () {
		playerAnim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		playerAnim.SetFloat("Speed",Input.GetAxis("Vertical"));
		playerAnim.SetFloat("Direction",Input.GetAxis("Horizontal"));
		if(playerAnim.GetFloat("Speed") == 0) {
			playerAnim.transform.Rotate(new Vector3(0f, playerAnim.GetFloat("Direction"), 0f));
		}
		if(playerAnim.GetFloat("Speed") > 0.5 && Input.GetKey(KeyCode.Q)) {
			playerAnim.SetTrigger("Roll");
		}

		if(playerAnim.GetFloat("Speed") > 0.5 && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) {
			if(playerAnim.speed < 1.5f) {
				playerAnim.speed += Time.deltaTime;
			}
		} else {
			playerAnim.speed = 1f;
		}
		GetComponent<CapsuleCollider>().height = 0.1787377f - 0.1f*playerAnim.GetFloat("ColliderHeight");
		GetComponent<CapsuleCollider>().center = new Vector3(0,0.09f - 0.09f*playerAnim.GetFloat("ColliderY"), 0f);

		if(playerAnim.GetFloat("Speed") > 0.5) {
			RaycastHit info = new RaycastHit();
			if(Physics.Raycast(new Ray(this.transform.position + new Vector3(0f, 1f, 0f), this.transform.forward), out info, playerAnim.GetFloat("Speed")*1.5f)) {
				playerAnim.SetFloat("Speed",Mathf.Min(Input.GetAxis("Vertical"),0));
			}
		}

		if(Input.GetMouseButton(0)) {
			playerAnim.SetTrigger("Attack1");
		}

		//rigidbody.AddForce((transform.forward + transform.right) * speed * Input.GetAxis("Vertical") * 60, ForceMode.Force);
		//rigidbody.AddForce((transform.right - transform.forward) * speed * Input.GetAxis("Horizontal") * 50, ForceMode.Force);
	}
}
