using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOLaboratories.Cauldron
{
    public class UVScroll : MonoBehaviour {

        /// <summary>
        /// name of texture to scroll UV's on
        /// </summary>
        public string textureName = "_MainTex";

        /// <summary>
        /// The material to scroll UV's on
        /// </summary>
        public Material PredefinedMaterial;

        /// <summary>
        /// How fast to scroll
        /// </summary>
        public Vector2 Scroll = new Vector2(1,0);

        /// <summary>
        /// Current UV offset
        /// </summary>
        Vector2 uvOffset = Vector2.zero;

	    // Update is called once per frame
	    void Update () {
            uvOffset += Scroll * Time.deltaTime;

            if (Scroll.x == 0) uvOffset.x = 0;
            if (Scroll.y == 0) uvOffset.y = 0;

            // if material is set then use that as scroll target
            if (PredefinedMaterial)
            {
                PredefinedMaterial.SetTextureOffset(textureName, uvOffset);
            }
            else
            {
                // if no material is set get MeshRenderer and scroll it's sharedMaterial
                var renderer = GetComponent<MeshRenderer>();
                if(renderer != null)
                {
                    renderer.sharedMaterial.SetTextureOffset(textureName, uvOffset);
                }
            }

	    }
    }

}

