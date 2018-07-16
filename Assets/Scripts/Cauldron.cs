using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour {

	GameObject potion;
	bool isTrigger = false;

    public Material potion1BubblePopMatLarge;
    public Material potion1BubblePopMatLarge2;
    public Material potion1BubblePopMatSmall;
    public Material potion1BubblePopMatSmall2;
    public Material smoke;
    public Material swirlparticles;
    public Material cauldron_goo;
    public Material cauldron_goo2;
	public Color potion1 = new Color32 ();

    private List<GameObject> recipe;



    private void OnTriggerEnter(Collider collider) {
		if (collider.CompareTag("Potion1") || collider.CompareTag("Potion2") || collider.CompareTag("Potion3")) {
			isTrigger = true;
			potion = collider.gameObject;
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

                // Andere Materialien der Bubbles etc bei richtiger Anwendung des Tranks
				GameObject.FindGameObjectWithTag ("BubblePopLarge").GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", new Color32(255,0,0,1));
				GameObject.FindGameObjectWithTag ("BubblePopLarge2").GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", new Color32(255,0,0,1));
				GameObject.FindGameObjectWithTag("BubblePopSmall").GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", new Color32(255,0,0,1));
				GameObject.FindGameObjectWithTag("BubblePopSmall2").GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", new Color32(255,0,0,1));
                
				//GameObject.FindGameObjectWithTag("Smoke").GetComponent<Renderer>().material = smoke;
				GameObject.FindGameObjectWithTag("Smoke").GetComponent<Renderer>().material.SetColor("_Color", new Color32(255,255,0,255));
                //GameObject.FindGameObjectWithTag("SwirlParticles").GetComponent<Renderer>().material = swirlparticles;
				GameObject.FindGameObjectWithTag("SwirlParticles").GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", new Color32(0,0,255,1));

                // Ändere Materialien vom Cauldron bei richtiger Anwendung
                Material[] mats = GameObject.FindGameObjectWithTag("Cauldron_goo").GetComponent<Renderer>().materials;
				mats[1].SetColor("_Color", new Color32(255,0,0,255));
                GameObject.FindGameObjectWithTag("Cauldron_goo").GetComponent<Renderer>().materials = mats;

                // Schalte Licht aus wenn erste Zutat richtig eingefüllt worden ist und leere das Gefäß
				Destroy (potion.transform.Find("GlowBottle").GetComponent<Light>());
                potion.transform.Find("Full").gameObject.active = false;
                int index = recipe.IndexOf(potion);
                if(index < recipe.Count - 1) {
                    GameObject nextPotion = recipe[index + 1];
                    nextPotion.transform.Find("GlowBottle").GetComponent<Light>().enabled = true;
                }
                

            }
			yield return new WaitForSeconds(1.0f);
		}
	}

	// Use this for initialization
	void Start () {
        // Initialisiere das Rezept
        recipe = new List<GameObject>();
        recipe.Add(GameObject.FindGameObjectWithTag("Potion1"));
        recipe.Add(GameObject.FindGameObjectWithTag("Potion2"));
        recipe.Add(GameObject.FindGameObjectWithTag("Potion3"));

		Debug.Log(GameObject.FindGameObjectWithTag ("BubblePopLarge").GetComponent<ParticleSystemRenderer>().material.GetColor("_EmissionColor"));

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
