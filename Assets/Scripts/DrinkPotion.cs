using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkPotion : MonoBehaviour {

	GameObject testPotion;
	TestPotion cauldron_goo;

	private void OnTriggerEnter(Collider collider) {
		if (collider.CompareTag("DrinkBottle") && cauldron_goo.ableToDrink) {
			Debug.Log ("trigger");
			testPotion = collider.gameObject;
			testPotion.transform.Find("Full").gameObject.active = false;
			Destroy (testPotion.transform.Find ("GlowBottle").GetComponent<Light> ());

			GameObject player = GameObject.FindGameObjectWithTag ("Player").gameObject;
			Vector3 playerScale = player.transform.localScale;
			playerScale = new Vector3(0.3f, 0.3f, 0.3f);

			player.transform.localScale = playerScale;

			Vector3 playerPosition = player.transform.position;
			playerPosition = new Vector3 (0f, 0f, -1f);
			player.transform.position = playerPosition;
		
		}
	}

	// Use this for initialization
	void Start () {
		cauldron_goo = GameObject.Find ("Cauldron_goo").GetComponent<TestPotion> ();
	}

	// Update is called once per frame
	void Update () {

	}
}
