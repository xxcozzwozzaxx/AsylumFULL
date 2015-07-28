using UnityEngine;
using System.Collections;

public class WarpDoctor : MonoBehaviour 
{

	public Texture2D[] images;
	public GameObject mesh;
	private int index = -1;
	public float imgSpeed = 0.03f;

	void Start() {

    	InvokeRepeating("LoopPng", 0, imgSpeed); // change last value until you get desired animation effect...

	}

 

	void LoopPng() {

    	index++;

    	if (index==images.Length) 

        index = 0;

	}

 

	void Update() {
		
		mesh.GetComponent<Renderer>().materials[0].SetTexture("_MainTex",images[index]);
		
		
	}
	


 


	
	
	
	
}
