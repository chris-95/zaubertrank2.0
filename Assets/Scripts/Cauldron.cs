using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour {

	GameObject potion;
	bool isTrigger = false;

	public Material potion1PopMat;

	private void OnTriggerEnter(Collider collider) {
		if (collider.CompareTag("Potion1")) {
			isTrigger = true;
			potion = collider.gameObject;

			Debug.Log (potion);
			StartCoroutine (PotionRotation(potion));
		}
	}

	private void OnTriggerExit(Collider collider) {
		if (collider.CompareTag("Potion1")) {
			isTrigger = false;
		}
	
	}

	IEnumerator PotionRotation(GameObject potion) {
		while(isTrigger) {
			if (Vector3.Dot(potion.transform.up, Vector3.up) < 0) {
				GameObject material1 = GameObject.FindGameObjectWithTag ("BubblePopLarge");
				material1.GetComponent<Renderer> ().material = potion1PopMat;
			}



			yield return new WaitForSeconds(1.0f);
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
