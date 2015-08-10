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
		if (Input.GetKeyDown (KeyCode.Z)) {
			Debug.LogError ("Making bot move");
		//MyStats.increaseStat("Hallusinations", 2);
			MoveToNewPositionCommand(transform.position + new Vector3(Random.Range(-5,5),0,Random.Range(-5,5)));
		//MoveToRoom (0);
		}
		if (Input.GetKeyDown (KeyCode.X)) {
			MyMovementState = MovementState.Wander;
		}
		if (MyMovementState == MovementState.Wander) {
			Wander ();
		}
		UpdateMoveTo ();
		for (int i = 0; i < agent.path.corners.Length-1; i++)
			Debug.DrawLine(agent.path.corners[i], agent.path.corners[i+1], Color.red);		
	}
	public void PlayCrazySong() {
		MyAudioSource.PlayOneShot (CrazyTune);
	}
	
	public void UpdateMoveTo() {
		if (MyMovementState != MovementState.Waiting && MyMovementState != MovementState.Sleeping) {
			agent.SetDestination (MoveToPosition);
			if (agent.path.corners.Length >= 1) {
				if (Vector3.Distance(transform.position, agent.path.corners[1]) > 0.2f) {
					float PreviousRotationY = transform.eulerAngles.y;
					transform.LookAt(agent.path.corners[1]);
					//transform.eulerAngles = new Vector3(0f, Mathf.Lerp (PreviousRotationY, transform.eulerAngles.y+180f, Time.deltaTime*5f), 0f);
					transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y+180f, 0f);
				}
			}
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
			float ThisDistance = Vector3.Distance(transform.position, hitColliders[i].gameObject.transform.position);
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
		float CheckRange = 5f;
		Collider[] hitColliders = Physics.OverlapSphere (gameObject.transform.position, CheckRange);
		for (int i = 0; i < hitColliders.Length; i++) {
			Patient NearByCharacter = hitColliders [i].gameObject.GetComponent<Patient>();
			if (NearByCharacter != null) {
				NearByCharacter.MyStats.increaseStat ("Aggression", 1);	// adds 1 aggression to nearby characters
			}
		}
	}
	IconPopup MyIconPopup;
	//warning stage: fast movement, create icon above head
	public void WarningHallucination() {
		agent.speed = 4f;
		//MyIconPopup.ChangeState
	}
	
	//self harm stage: affect own stat, Physical Health - 2 per turn
	public void InflictSelfHarm() {
		StopMoving ();
		MyPatient.MyStats.increaseStat ("PhysicalHealth", -2);
	}

	public void RestoreMovement() {
		agent.speed = 2f;
	}
	//slow movement
	public void SlowMovement() {
		agent.speed = 0.85f;
	}
	
	// No movement
	public void StopMoving() {
		agent.speed = 0f;
		AlterMoving (false);
	}
	public void AlterMoving(bool IsMovement) {
		Rigidbody MyRigid = gameObject.GetComponent<Rigidbody> ();
		if (MyRigid != null)
			MyRigid.isKinematic = IsMovement;
	}
}
