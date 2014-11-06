using UnityEngine;
using System.Collections;

public class BasicEnemy : MonoBehaviour {

	private Transform player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(transform.position, player.transform.position) < 10f) {
			transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.1f);
		}
	}
}
