using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// to do:
// patient to move to room
public enum MovementState {
	Waiting,
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
<<<<<<< HEAD
=======

		agent = GetComponent<NavMeshAgent> ();
>>>>>>> b0b580941b722a8a140d744ec195344c970cf781
		//OrigX = transform.position.z;
		MyTurnManager = GetManager.GetTurnManager ();
	}
	// Update is called once per frame
	void Update () {
		//if (Input.GetKeyDown (KeyCode.Z)) {
			//MyStats.increaseStat("Hallusinations", 2);
			//MoveToNewPosition(transform.position + new Vector3(Random.Range(-5,5),0,Random.Range(-5,5)));
			//MoveToRoom (0);
		//}
	}
}
