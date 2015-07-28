using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateTime : MonoBehaviour {
	public TurnManager MyTurnManager;
	public FadeScreen MyFadeScreen;
	// Turn stuff
	public int TurnNumber = 1;
	public int TurnsPerDay = 5;
	public int MinutesPerTurn = 1;
	public int SecondsPerTurn = 10;
	// Actual time
	public int DayNumber = 1;
	private float TimeStarted = 0;
	// Use this for initialization
	void Start () {
		TimeStarted = Time.time;
		DayNumber = 1;
		MyFadeScreen.ResetFade(DayNumber);
	}
	
	// Update is called once per frame
	void Update () {
		float SecondsPassed = (Time.time - TimeStarted);
		int MinutesPassed = Mathf.FloorToInt(SecondsPassed / 60.0f);
		SecondsPassed = SecondsPassed - MinutesPassed * 60f;
		gameObject.GetComponent<Text> ().text = "Day: " + DayNumber.ToString() + ", Turn: " + TurnNumber.ToString() + "\n" + 
												"Time: " + MinutesPassed + "m" + (Mathf.RoundToInt(SecondsPassed)) + "s";
		
		if (MinutesPassed*60f+SecondsPassed >= MinutesPerTurn*60f+SecondsPerTurn) {
			EndTurn();
		}
		if (TurnNumber > TurnsPerDay) {
			EndDay();
		}
	}
	public void EndTurn() {
		MyTurnManager.EndTurn ();
		TimeStarted = Time.time;
		TurnNumber++;
	}
	public void EndDay() {
		TimeStarted = Time.time;	// reset the time
		TurnNumber = 1;				// reset the turns
		DayNumber++;				// increase the days
		MyFadeScreen.ResetFade(DayNumber);
	}
}
