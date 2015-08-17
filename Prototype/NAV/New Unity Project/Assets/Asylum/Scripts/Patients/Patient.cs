using UnityEngine;
using UnityEngine.UI;
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
	public Sprite OtherSprite;
	public Image image;
	public BehaviourState currentState;
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

	public void changeStateImage(BehaviourState _state){
		currentState = _state;
		switch (currentState) 
		{
		case BehaviourState.MoveTo:
			
			//animation
			
			break;
		case BehaviourState.Passive:
			image.sprite = OtherSprite;
			//wandering
			
			break;
		case BehaviourState.MediumAggressive:
			//Create a radius around patient, for other patients within the radius: Aggression +1 per turn
			
			break;
		case BehaviourState.HighAggression:
			//Patient attack the closest target, target's physical health -2 per turn
			
			break;
		case BehaviourState.WarningHallucinating:
			//warning stage: fast movement, create icon above head
			
			
			break;
		case BehaviourState.SelfHarmHallusinating:
			//self harm stage: affect own stat, Physical Health - 2 per turn
			
			
			break;
		case BehaviourState.Tired:
			//slow movement
			
			break;
		case BehaviourState.Sleeping:
			
			//sleeping animation
			
			break;
		case BehaviourState.Hungry:
			//slow movement
			//icon
			
			break;
		case BehaviourState.Eating:
			
			//icon
			//animation
			break;
		case BehaviourState.InTherapy:
			
			//icon
			
			break;
		case BehaviourState.Dying:
			
			//slow movement, icon
			
			break;
		case BehaviourState.Dead:
			
			//dead
			break;
			
		}
	}
};