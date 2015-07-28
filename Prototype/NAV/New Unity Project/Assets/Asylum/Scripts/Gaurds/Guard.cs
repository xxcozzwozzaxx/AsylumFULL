using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Anything that isn't Guard related activities don't belong here in Guardville

public class Guard : MonoBehaviour {

	// scene references
	public GuardStats MyStats;
	public TurnManager MyTurnManager;
	public Transform followPatient;
	public NavMeshAgent agent;
	public bool FollowingPatient = false;
	public GameObject LastSelectPatient;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		MyTurnManager = GetManager.GetTurnManager ();
	}

	// Update is called once per frame
	void Update () 
	{
		if (FollowingPatient) {
			agent.SetDestination (followPatient.position);
		}
	}




	// All the guard commands
	
	public void MoveToLocation(Vector3 NewLocation) {
		//Debug.Log ("Mouse Hit Tile!");
		agent.SetDestination (NewLocation);
		//Debug.DrawLine (hitInfo.point, hitInfo.point + new Vector3 (0, 3, 0), Color.red, 3);
		gameObject.GetComponent<CharacterSelector>().IsSelected = false;
		FollowingPatient = false;
		gameObject.GetComponent<CharacterSelector>().HasBeenOrdered = true;
	}
	
	public void MoveToPatient() 
	{
		MyTurnManager.AddTurnMoveToPatient (gameObject, LastSelectPatient);
		gameObject.GetComponent<CharacterSelector>().HasBeenOrdered = true;
	}
	
	public void FollowPatient(GameObject PatientTransform) {
		followPatient = PatientTransform.transform; 
		FollowingPatient = true;
		gameObject.GetComponent<CharacterSelector>().IsSelected = false;
		gameObject.GetComponent<CharacterSelector>().HasBeenOrdered = true;
	}
}
