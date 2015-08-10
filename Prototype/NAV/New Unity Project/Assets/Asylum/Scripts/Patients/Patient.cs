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
	public MovementState MyMovementState;
	public TurnManager MyTurnManager;
	public PatientStats MyStats = new PatientStats();
	public float OriginalSpeed = 4f;
	public float MaxMovement = 4f;
	public Vector3 Direction;
	public bool movingRight = false;
	public List<string> PossibleNames = new List<string>();
	public Vector3 MoveToPosition;
	public NavMeshAgent agent;
	public float LastTimeWander;
	public float WanderRange = 1f;
	public AudioClip MyAudioClip;
	public AudioSource MyAudioSource;

	// Use this for initialization
	void Start () 
	{
		MyStats.MyPatient = (this);

		agent = GetComponent<NavMeshAgent> ();
		//OrigX = transform.position.z;
		MyTurnManager = GetManager.GetTurnManager ();
		LastTimeWander = Time.time;
		if (gameObject.GetComponent<AudioSource> () != null) 
		{
			MyAudioSource = gameObject.GetComponent<AudioSource> ();
		}
		else
		{
			MyAudioSource = gameObject.AddComponent<AudioSource> ();
		}
	}
	public void PlayCrazySong() {
		MyAudioSource.PlayOneShot (MyAudioClip);
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Z)) {
			MyStats.increaseStat("Hallusinations", 2);
			//MoveToNewPosition(transform.position + new Vector3(Random.Range(-5,5),0,Random.Range(-5,5)));
			//MoveToRoom (0);
		}
		if (MyMovementState == MovementState.Crazy) {
			Wander ();
		}
		UpdateMoveTo ();
	}
	public void Wander() {
		if (Time.time - LastTimeWander > 0.5f) {
			LastTimeWander = Time.time;
			MoveToNewPosition(transform.position + new Vector3(Random.Range(-WanderRange,WanderRange),0,Random.Range(-WanderRange,WanderRange)));
		}
	}

	public void UpdateMoveTo() {
		if (MyMovementState != MovementState.Waiting && MyMovementState != MovementState.Sleeping) {
			agent.SetDestination (MoveToPosition);
			// if tpatient is near position, it will stop moving
			if (MyMovementState == MovementState.MovingTo && Vector3.Distance(transform.position, MoveToPosition) < 1f) {
				MyMovementState = MovementState.Waiting;
			}
		}
	}
	// overrides behaviour
	public void MoveToNewPositionCommand(Vector3 NewPosition) {
		MyMovementState = MovementState.MovingTo;
		MoveToPosition = NewPosition;
	}
	public void MoveToNewPosition(Vector3 NewPosition) {
		MoveToPosition = NewPosition;
	}

	// Makes the patient become aggressive, it will attack one nearby person each turn
	public void GoCrazy() {

	}
	/*public void GoCrazy() {
			if (OrigX - transform.position.z > MaxMovement) {
				Speed = OriginalSpeed;
				movingRight = true;
			} else if (OrigX - transform.position.z < -MaxMovement) {
				Speed = -OriginalSpeed;
				movingRight = false;
			}
			transform.Translate (0, 0, Speed * Time.deltaTime);
	}*/

	//public void Select(bool IsProfile) 
	//{
	//	IsSelected = true;
	//}
	public void AlterMovement(bool IsMovement) {
		//if (!IsMovement)
		//	Speed = 0;
		//else
		//	Speed = OriginalSpeed;
		Rigidbody MyRigid = gameObject.GetComponent<Rigidbody> ();
		if (MyRigid != null)
			MyRigid.isKinematic = IsMovement;
	}
}
