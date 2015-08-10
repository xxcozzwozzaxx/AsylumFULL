using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FSM : MonoBehaviour 
{
	private Patient MyPatient;
	//state machine for Patients, determine what patients will do in each state.

	public enum State
	{
		Passive,
		Aggressive,
		Hallucinating,
		Medicated,
		Tired,
		Sleeping,
		Hungry,
		Eating,
		InTherapy,
		Dying,
		Dead,
		MoveTo
	};
	
	private State currentState;
	private float lastStateChange = 0.0f;
	
	
	void Start()
	{
		SetCurrentState (State.Passive);
		MyPatient = gameObject.GetComponent<Patient> ();
	}
	
	void SetCurrentState(State _state)
	{
		currentState = _state;
		lastStateChange = Time.time;
	}
	
	float GetStateElapsed()
	{
		return Time.time - lastStateChange;
	}
	
	void Update()
	{
		Debug.Log (currentState);
		switch (currentState) 
		{
		case State.MoveTo:

			break;
		case State.Passive:
			
			
			
			break;
		case State.Aggressive:
			MyPatient.GoCrazy();

			break;
		case State.Hallucinating:
			
		
			
			break;
		case State.Medicated:
			
			
			
			break;
		case State.Tired:


			break;
		case State.Sleeping:
			
			
			
			break;
		case State.Hungry:


			break;
		case State.Eating:


			break;
		case State.InTherapy:
			
			
			
			break;
		case State.Dying:
			
			
			
			break;
		case State.Dead:



			break;
		}

	}


}
