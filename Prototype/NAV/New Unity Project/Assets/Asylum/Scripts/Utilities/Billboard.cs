using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	private Transform _target;

	// Use this for initialization
	void Start () {
		_target = Camera.main.transform;

	}
	
	// Update is called once per frame
	void Update () {

		transform.LookAt (_target.position);
	
	}
}
