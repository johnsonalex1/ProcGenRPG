using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float hp, maxHP;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (hp < 0) {
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag.Equals("PlayerAttack")) {
			Attack attack = other.gameObject.GetComponent<Attack>();
			hp -= attack.damage;
		}
	}
}
