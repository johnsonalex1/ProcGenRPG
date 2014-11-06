using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public List<Item> inventory = new List<Item>();
	private Weapon activeWeapon;

	// Use this for initialization
	void Start () {
		activeWeapon = (Weapon)inventory[0];
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			GameObject attack = activeWeapon.attackOBJ;
			Instantiate(attack, transform.position, transform.localRotation);
		}
	}
}
