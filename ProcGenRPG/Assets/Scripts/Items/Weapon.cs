using UnityEngine;
using System.Collections;

public class Weapon : Item {

	private Attack attack;
	public GameObject attackOBJ;

	// Use this for initialization
	void Start () {
		attack = attackOBJ.GetComponent<Attack>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public Attack GetAttack() {
		return attack;
	}
}
