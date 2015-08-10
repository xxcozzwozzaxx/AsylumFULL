using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// to do:
// patient to move to room
public enum MovementState {
	Waiting,
	Wander,
	Crazy,
	MovingTo,
	Patrol,
	Dancing,
	Sleeping
};

public class Patient : MonoBehaviour {
	public PatientStats MyStats = new PatientStats();
	public TurnManager MyTurnManager;
	public float OriginalSpeed = 4f;
	public float MaxMovement = 4f;
	public Vector3 Direction;
	public bool movingRight = false;
	//public List<string> PossibleNames = new List<string>();

	// Use this for initialization
	void Start () 
	{
		MyStats.MyPatient = (this);

		//agent = GetComponent<NavMeshAgent> ();

		//OrigX = transform.position.z;
		MyTurnManager = GetManager.GetTurnManager ();
	}
	// Update is called once per frame
	void Update () {
	}
}
