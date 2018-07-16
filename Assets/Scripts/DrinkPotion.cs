using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkPotion : MonoBehaviour {

	GameObject testPotion;

	private void OnTriggerEnter(Collider collider) {
		if (collider.CompareTag("DrinkBottle")) {
			Debug.Log ("trigger");
			testPotion = collider.gameObject;
			GameObject player = GameObject.FindGameObjectWithTag ("Player").gameObject;
			Vector3 playerPosition = player.transform.localScale;
			playerPosition = new Vector3(0.3f, 0.3f, 0.3f);

			player.transform.localScale = playerPosition;
		
		}
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
