using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPotion : MonoBehaviour {

	GameObject testPotion;
	bool isTrigger = false;
	public bool ableToDrink = false;
	Cauldron cauldron;

	private void OnTriggerEnter(Collider collider) {
		if (collider.CompareTag("DrinkBottle")) {
			isTrigger = true;
			testPotion = collider.gameObject;
			StartCoroutine (PotionRotation(testPotion));
		}
	}

	private void OnTriggerExit(Collider collider) {
		if (collider.CompareTag("DrinkBottle")) {
			isTrigger = false;
		}

	}

	IEnumerator PotionRotation(GameObject testPotion) {
		while(isTrigger) {
			if (Vector3.Dot(testPotion.transform.up, Vector3.up) < 0 && cauldron.ableToFill) {
				testPotion.transform.Find("Full").gameObject.active = true;
				ableToDrink = true;

			}
			yield return new WaitForSeconds(1.0f);
		}
	}

	// Use this for initialization
	void Start () {
		cauldron = GameObject.Find ("Cauldron").GetComponent<Cauldron> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
