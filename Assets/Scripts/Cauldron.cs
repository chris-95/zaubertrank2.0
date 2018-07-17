using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour {

	public List<GameObject> Children;
	GameObject potion;
	public GameObject allPotions;
	bool isTrigger = false;
	public int[] randomRecipes;
	public List<Color32> potionColors;
	int potionIndex;
	List<int> selectedPotions = new List<int>();
	public bool ableToFill = false;



    private void OnTriggerEnter(Collider collider) {
		if (collider.CompareTag("Potion")) {
			isTrigger = true;
			potion = collider.gameObject;
			StartCoroutine (PotionRotation(potion));
		}
	}

	private void OnTriggerExit(Collider collider) {
		if (collider.CompareTag("Potion")) {
			isTrigger = false;
		}
	
	}

	IEnumerator PotionRotation(GameObject potion) {
		while(isTrigger) {
			if (Vector3.Dot (potion.transform.up, Vector3.up) < 0) {
				if (potion.gameObject == Children [randomRecipes [potionIndex]]) {
					this.ChangeColorOfMaterial ();
					// Schalte Licht aus wenn erste Zutat richtig eingefüllt worden ist und leere das Gefäß
					Destroy (potion.transform.Find ("GlowBottle").GetComponent<Light> ());

					potion.transform.Find ("Full").gameObject.active = false;
					GameObject nextPotion;

					if (potionIndex < randomRecipes.Length - 1) {
						potionIndex++;
						nextPotion = Children [randomRecipes [potionIndex]];
					} else {
						nextPotion = GameObject.FindGameObjectWithTag ("DrinkBottle");
						ableToFill = true;
					}

					nextPotion.transform.Find ("GlowBottle").GetComponent<Light> ().enabled = true;
					// Wenn alle Tränke bereits geleert sind, wird die Füllung wieder aktiviert um dem Spieler mehr Feedback zu geben
					if (nextPotion.transform.Find ("Full").gameObject.active == false)
						nextPotion.transform.Find ("Full").gameObject.active = true;

				} else {
					potion.transform.Find ("Full").gameObject.active = false;
				}
			}
			yield return new WaitForSeconds(1.0f);
		}
	}

	void ChangeColorOfMaterial() {
		Color32 bubbleColor = new Color32(potionColors[potionIndex].r,potionColors[potionIndex].g,potionColors[potionIndex].b, 1);
		Color32 smokeColor = new Color32 (potionColors [potionIndex].r, potionColors [potionIndex].g, potionColors [potionIndex].b, 255);
		// Andere Materialien der Bubbles etc bei richtiger Anwendung des Tranks
		GameObject.FindGameObjectWithTag ("BubblePopLarge").GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", bubbleColor);
		GameObject.FindGameObjectWithTag ("BubblePopLarge2").GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", bubbleColor);
		GameObject.FindGameObjectWithTag("BubblePopSmall").GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", bubbleColor);
		GameObject.FindGameObjectWithTag("BubblePopSmall2").GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", bubbleColor);

		GameObject.FindGameObjectWithTag("Smoke").GetComponent<Renderer>().material.SetColor("_Color", smokeColor);
		//GameObject.FindGameObjectWithTag("SwirlParticles").GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", bubbleColor);

		// Ändere Materialien vom Cauldron bei richtiger Anwendung
		Material[] mats = GameObject.FindGameObjectWithTag("Cauldron_goo").GetComponent<Renderer>().materials;
		mats[1].SetColor("_Color", smokeColor);
		GameObject.FindGameObjectWithTag("Cauldron_goo").GetComponent<Renderer>().materials = mats;
	}

	void GetAllPotions() {
		foreach (Transform child in allPotions.transform)
		{
			if (child.tag == "Potion")
			{
				Children.Add(child.gameObject);
			}
		}
	}

	int SetRandomPotionForRecipe() {
		int currentPotion = Random.Range (0, Children.Count - 1);
		while (selectedPotions.Contains (currentPotion)) {
			currentPotion = Random.Range (0, Children.Count - 1);
		}
		return currentPotion;
	}

	// Use this for initialization
	void Start () {
        // Initialisiere das Rezept
		potionIndex = 0;
		this.GetAllPotions ();
		Color32 color1 = new Color32(255,255,0,255);

		for (int i = 0; i < randomRecipes.Length; i++) {
			int currentPotion = SetRandomPotionForRecipe ();
			randomRecipes [i] = currentPotion;
			selectedPotions.Add(currentPotion);
		}

		Children [randomRecipes [potionIndex]].transform.Find ("GlowBottle").GetComponent<Light> ().enabled = true;

		Debug.Log(color1.r);
	}

}
