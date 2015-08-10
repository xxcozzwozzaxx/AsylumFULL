using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


[System.Serializable]
public enum TreatmentState {
	None,
	SingleTherapy,
	GroupTherapy,
	Lobotomy,
	Trephination,
	InsulinComaTherapy,
	Bloodletting,
	ShockTherapy,
	IceBaths,
	Restraints,
	Solitary,
	FeverTherapy,
	SpinningTherapy
};

[System.Serializable]
public enum BehaviourState
{
	Passive,
	MediumAggressive,
	HighAggression,
	WarningHallucinating,
	SelfHarmHallusinating,
	Tired,
	Sleeping,
	Hungry,
	Eating,
	InTherapy,
	Dying,
	Dead,
	MoveTo
};

[System.Serializable]
public class Stat {
	public string name;
	private int MaxStat = 20;
	public float Amount = 10;
	public Stat ()
	{

	}

	public Stat(string statName, float statAmount)
	{
		name = statName;
		Amount = statAmount;
	}
	
	public void SetStat(float newStat)
	{
		Amount = newStat;
		CheckLimit ();
	}

	public void increaseStat(float newStat)
	{
		Amount += newStat;
		CheckLimit ();
	}

	public void CheckLimit()
	{
		Amount = Mathf.Clamp (Amount, 0, MaxStat);
	}
	
};

[System.Serializable]
public class PatientStats {
	public Patient MyPatient;
	public string Name;
	public string Description;							// used to describe the patient in its profile
	public TreatmentState MyTreatmentState = TreatmentState.None;
	public BehaviourState currentState = BehaviourState.Passive;
	public List<Stat> theStats = new List<Stat>();		// a list of its stats, designed for dynamic stats as opposed to static
	public Sprite OtherSprite;
	public Image PATIENTIMAGE;
	public PatientGenerator gen;
	public GameObject thePatient;

	public PatientStats()
	{
		SetCurrentState (BehaviourState.Passive);
	}
	
	public Stat increaseStat(string name, float Amount) {
		for (int i = 0; i < theStats.Count; i++) 
		{
			if (name == theStats[i].name)
			{
				theStats[i].increaseStat(Amount);
			}
		}
		return new Stat ();
	}

	// returns the stat of a given name from the list
	public Stat getStat(string name) {
		for (int i = 0; i < theStats.Count; i++) 
		{
			if (name == theStats[i].name)
			{
				return theStats[i];
			}
		}
		return new Stat ();
	}

	// sets the stat with a given name to an ammount
	public void setStat(string name, float Amount)
	{
		for (int i = 0; i < theStats.Count; i++) 
		{
			if (name == theStats[i].name)
			{
				theStats[i].SetStat(Amount);
				i = theStats.Count;
			}
		}
	}
	
	// increases the stat with a given name by an ammount
	void SetCurrentState(BehaviourState _state)
	{
		currentState = _state;
		//lastStateChange = Time.time;
		ChangeStates ();
	}

	public void Randomize() {
		for (int i = 0; i < theStats.Count; i++) {
			theStats[i].SetStat(Random.Range (1,20));
		}
	}

	// Randomizes the stats around a range
	public void Randomize(int Offset, int Range) {
		for (int i = 0; i < theStats.Count; i++) {
			theStats[i].SetStat(Random.Range (Offset-Range, Offset+Range));
		}
	}

	public void IncreaseTurn(int TurnsPassed) {
		Debug.Log ("Increasing turn in patient!" + MyPatient.name);
		TreatmentsTurn ();
		StatsTurn (TurnsPassed);
		//setStat ("Hunger", Mathf.Clamp (getStat("Hunger").Amount, 1, 20));
	}

	public void TreatmentsTurn() {
		switch (MyTreatmentState) 
		{
		case TreatmentState.Bloodletting:

			increaseStat("Aggression", -0.5f);
			increaseStat("Hallucinations", 0.5f);
			increaseStat("PhysicalHealth", -0.5f);
			
			break;
		case TreatmentState.FeverTherapy:

			increaseStat("Aggression", -0.5f);
			increaseStat("Hallucinations", 0.5f);
			increaseStat("PhysicalHealth", -1f);
			increaseStat("Fatigue", 1f);
			
			break;
		case TreatmentState.GroupTherapy:

			increaseStat("Aggression", 1f);
			increaseStat("Hallucinations", -1f);
			increaseStat("Hunger", 0.5f);
			increaseStat("Fatigue", 0.5f);
			
			break;
		case TreatmentState.IceBaths:

			increaseStat("Aggression", -0.5f);
			increaseStat("Hallucinations", 0.5f);
			increaseStat("Hunger", 1f);
			increaseStat("Fatigue", 0.5f);
			
			break;
		case TreatmentState.InsulinComaTherapy:

			increaseStat("Aggression", -1f);
			increaseStat("Hallucinations", 1f);
			increaseStat("PhysicalHealth", -0.5f);
			increaseStat("Fatigue", -0.5f);
			
			break;
		case TreatmentState.Lobotomy:

			increaseStat("Aggression", 1f);
			increaseStat("Hallucinations", -1f);
			increaseStat("PhysicalHealth", -1f);
			
			break;
		case TreatmentState.Restraints:

			increaseStat("Aggression", -0.5f);
			increaseStat("Hallucinations", -0.5f);
			increaseStat("Hunger", 0.5f);
			increaseStat("Fatigue", -0.5f);
			
			break;
		case TreatmentState.ShockTherapy:

			increaseStat("Aggression", 1f);
			increaseStat("Hallucinations", -0.5f);
			increaseStat("PhysicalHealth", -1f);
			
			break;
		case TreatmentState.SingleTherapy:

			increaseStat("Aggression", -0.5f);
			increaseStat("Hallucinations", -0.5f);
			increaseStat("Hunger", 0.5f);
			increaseStat("Fatigue", 0.5f);
			
			break;
		case TreatmentState.Solitary:

			increaseStat("Aggression", -1f);
			increaseStat("Hallucinations", 1f);
			increaseStat("Fatigue", -0.5f);
			
			break;
		case TreatmentState.SpinningTherapy:

			increaseStat("Aggression", -1f);
			increaseStat("Hallucinations", -0.5f);
			increaseStat("Hunger", 1f);
			increaseStat("Fatigue", 1f);
			
			break;
		case TreatmentState.Trephination:

			increaseStat("Aggression", -0.5f);
			increaseStat("Hallucinations", 1f);
			increaseStat("PhysicalHealth", -0.5f);
			
			break;
			
		}
		MyTreatmentState = TreatmentState.None;
	}

	// stats affect stats
	public void StatsTurn(int TurnsPassed) {
		// if every second turn
		if (TurnsPassed % 5 == 0) {
			increaseStat("Hunger", 0.5f);
		}
		if (getStat ("Aggression").Amount >= 9) {

			increaseStat("PhysicalHealth", -0.5f);
			increaseStat("Hunger", 0.5f);
			increaseStat("Fatigue", 0.5f);

		}
		if (getStat ("Hallucinations").Amount >= 9) {

			increaseStat("Aggression", 0.5f);
			increaseStat("Fatigue", 0.5f);

		}
		if (getStat ("PhysicalHealth").Amount <= 3) {

			increaseStat("Hunger", 0.5f);
			increaseStat("Fatigue", 1f);

		}
		if (getStat ("Hunger").Amount >= 9) {

			increaseStat("Aggression", 1f);
			increaseStat("Hallucinations", 0.5f);
			increaseStat("PhysicalHealth", -0.5f);
			increaseStat("Fatigue", 0.5f);

		}
		if (getStat ("Fatigue").Amount >= 9) {

			increaseStat("Hallucinations", 1f);
			increaseStat("PhysicalHealth", -1f);

		}
				
		//setting states
		if (getStat ("Aggression").Amount  > 5) {
			
			SetCurrentState (BehaviourState.MediumAggressive);

		}
		if (getStat ("Aggression").Amount  > 8) {
			
			SetCurrentState (BehaviourState.HighAggression);
			
		} 
		if(getStat ("Hallucinations").Amount  > 5)
		{
			
//			MyPatient.MyMovementState = MovementState.Crazy;
//			MyPatient.PlayCrazySong();
			SetCurrentState (BehaviourState.WarningHallucinating);
		}
		if(getStat ("Hallucinations").Amount  > 8)
		{
			
//			MyPatient.MyMovementState = MovementState.Crazy;
//			MyPatient.PlayCrazySong();
			SetCurrentState (BehaviourState.SelfHarmHallusinating);
		}
		if (getStat ("PhysicalHealth").Amount  < 4)
		{
			SetCurrentState (BehaviourState.Dying);
		}
		if(getStat ("Hunger").Amount  > 5)
		{
			SetCurrentState (BehaviourState.Hungry);
		}
		if(getStat ("Fatigue").Amount  > 5)
		{
			SetCurrentState (BehaviourState.Tired);
		}
		if(getStat ("PhysicalHealth").Amount  <= 0)
		{
			SetCurrentState (BehaviourState.Dead);
		}
	}


	public void ChangeStates()
	{
		if (MyPatient.gameObject.name != "PatientManager") 
		{
			Debug.Log (currentState);
			switch (currentState) 
			{
				case BehaviourState.MoveTo:

				//animation

				break;
				case BehaviourState.Passive:
//
//				Sprite tempImg = new Sprite ();
//				foreach (Sprite sp in gen.SpriteList.ToArray())
//				{
//					if (sp.name == "h1")
//					PATIENTIMAGE.overrideSprite = sp;
//				}
			//wandering
			
				break;
						case BehaviourState.MediumAggressive:
								//MyPatient.GoCrazy ();
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
	}
};