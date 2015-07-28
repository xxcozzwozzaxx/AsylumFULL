using UnityEngine;
using System.Collections;

public class GuiLookAtCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		LookAtCamera ();
	}
	// sets the transform variables of the 3d canvas to always face the main camera position
	public void LookAtCamera() {
		gameObject.transform.LookAt(Camera.main.transform.position, Vector3.up);

		gameObject.transform.Rotate (new Vector3 (180, 0, 180));
	}
}
