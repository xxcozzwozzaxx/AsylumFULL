using UnityEngine;
using System.Collections;

public class Behaviour : MonoBehaviour {
	Patient MyPatient;
	public NavMeshAgent agent;
	public AudioSource MyAudioSource;
	public AudioClip CrazyTune;
	public AudioClip SpawnedTune;
	public AudioClip AttackingTune;
	public AudioClip HallucinationTunePre;
	// Movement Data
	public MovementState MyMovementState;
	public Vector3 MoveToPosition;
	public float LastTimeWander;
	public float WanderRange = 1f;
	public float WanderCoolDown = 1.5f;

	// Use this for initialization
	void Start () {
		LastTimeWander = Time.time;
		MyPatient = gameObject.GetComponent<Patient> ();
		agent = GetComponent<NavMeshAgent> ();
		MyAudioSource = gameObject.GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		UpdateMoveTo ();
	}
	public void PlayCrazySong() {
		MyAudioSource.PlayOneShot (CrazyTune);
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

	// for wandering state
	// every 3-7 seconds, chose a new location, walk to it
	public void Wander() {
		if (Time.time - LastTimeWander > WanderCoolDown) {
			LastTimeWander = Time.time;
			MoveToNewPosition(transform.position + new Vector3(Random.Range(-WanderRange,WanderRange),0,Random.Range(-WanderRange,WanderRange)));
		}
	}
	
	// Patient attack the closest target, target's physical health -2 per turn
	public void AttackClosestTarget() {
		GameObject MyAttackTarget = FindClosestTarget (5f);
		MyAttackTarget.GetComponent<Patient> ().MyStats.increaseStat ("PhysicalHealth", -1);
		MoveToNewPosition (MyAttackTarget.transform.position - transform.forward);
	}

	public GameObject FindClosestTarget(float CheckRange) {
		int ClosestIndex = 0;
		float MyClosestDistance = 5000f;
		Collider[] hitColliders = Physics.OverlapSphere (gameObject.transform.position, CheckRange);
		for (int i = 0; i < hitColliders.Length; i++) 
		{
			Patient NearByCharacter = hitColliders [i].gameObject.GetComponent<Patient>();
			float ThisDistance = Vector3.Distance(transform.position, hitColliders[i].gameObject);
			if (NearByCharacter != null) 
			if (ThisDistance < MyClosestDistance) {
				ClosestIndex = i;
				MyClosestDistance = ThisDistance;
			}
		}
		if (ClosestIndex >= 0 && ClosestIndex < hitColliders.Length)
			return hitColliders [ClosestIndex].gameObject;
		else 
			return null;
	}
	//Create a radius around patient, for other patients within the radius: Aggression +1 per turn
	public void IncreaseAggressionRadius() {
		//GameObject[] colliders = Physics.SphereCast (MyPatient.transform.position, 5f);
		
		Collider[] hitColliders = Physics.OverlapSphere (gameObject.transform.position, CheckRange);
		for (int i = 0; i < hitColliders.Length; i++) {
			Patient NearByCharacter = hitColliders [i].gameObject.GetComponent<Patient>();
			if (MyPatient != null) {

			}
		}
	}
	
	//warning stage: fast movement, create icon above head
	public void WarningHallucination() {
		
	}
	
	//self harm stage: affect own stat, Physical Health - 2 per turn
	public void InflictSelfHarm() {
		
	}
	
	//slow movement
	public void SlowMovement() {
		
	}
	
	// No movement
	public void Stop() {
		AlterMoving (false);
	}
	public void AlterMoving(bool IsMovement) {
		Rigidbody MyRigid = gameObject.GetComponent<Rigidbody> ();
		if (MyRigid != null)
			MyRigid.isKinematic = IsMovement;
	}
}
