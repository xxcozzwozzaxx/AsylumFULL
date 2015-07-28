using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
// To do
//		add a button listener to the no event
// 

public class GameEvent {
	public int EventIndex = 0;	// 0 for new patient, 1 for new guard etc
	public string EventText;
	public float CashBonus;

	public GameEvent () 
	{

	}
};

public class EventsManager : MonoBehaviour {
	public List<GameEvent> MyEvents = new List<GameEvent>();
	public List<GameObject> MyEventGuis = new List<GameObject>();
	public GameObject EventGuiPrefab;
	bool HasInitialized = false;
	public int EventsToSpawn = 15;
	public float MarginY = 5f;
	public float SizeY = 80f;
	// references
	PatientManager MyPatientManager;
	GaurdManager MyGuardManager;

	// Use this for initialization
	void Start () {
		MyPatientManager = GetManager.GetPatientManager ();
		MyGuardManager = GetManager.GetGuardManager ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!HasInitialized) {
			for (int i = 0; i < EventsToSpawn; i++) 
			{
				NewEvent(0);
			}
			UpdateGui ();
			HasInitialized = true;
		}
		if (Input.GetKeyDown (KeyCode.X)) {
			AddRandomEvent();
			UpdateGui ();
		}
	}
	public void AddRandomEvent() {
		NewEvent (Random.Range (0, 2));
		UpdateGui ();
	}

	// Initial cost for employees is one weeks pay
	public void NewEvent(int EventIndex) {
		GameEvent NewEvent = new GameEvent ();
		NewEvent.EventIndex = EventIndex;
		switch (EventIndex) {
			case (0):	// patients give money
				NewEvent.EventText = "Application: Patient" + "\n" +
									"Name: Steve" + "\n" +
									"Income: $300" + "\n" +
									"Condition: Alchoholism";
				NewEvent.CashBonus = 300;
				break;
			case (1):	// guards cost money
				NewEvent.EventText = "Application: Guard" + "\n" +
									"Name: Dezlo" + "\n" +
									"Cost: $1050" + "\n" +
									"Salary: $150 Per Day";
				NewEvent.CashBonus = -1050;
				break;
			case (2):	// doctors cost money
				NewEvent.EventText = "Application: Doctor" + "\n" +
									"Name: Leviathan" + "\n" +
									"Cost: $2100" + "\n" +
									"Salary: $300 Per Day";
				NewEvent.CashBonus = -2100;
				break;
		}
		MyEvents.Add (NewEvent);
	}
	
	// Removes all the EventGuis from the scene
	public void ClearGuis() {
		for (int i = 0; i < MyEventGuis.Count; i++) {
			Destroy (MyEventGuis[i]);
		}
		MyEventGuis.Clear ();
	}

	// Updates the Gui Rect Transform, the size, and updates the positions of all the events in it
	public void UpdateGui() {
		ClearGuis ();

		float NewHeight = MyEvents.Count * (SizeY + MarginY);
		float MinHeight = -1;

		MinHeight = gameObject.transform.parent.parent.GetComponent<RectTransform> ().sizeDelta.y;

		if (NewHeight < MinHeight)
			NewHeight = MinHeight;

		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector3(gameObject.GetComponent<RectTransform>().sizeDelta.x,NewHeight,0);

		// Now create teh Event Guis
		for (int i = 0; i < MyEvents.Count; i++) 
		{
			GameObject NewEvent = (GameObject) Instantiate(EventGuiPrefab, new Vector3(), Quaternion.identity);
			NewEvent.name = "Event " + i;
			NewEvent.transform.SetParent(gameObject.transform, false);
			NewEvent.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,-MarginY-SizeY/2f-i*(SizeY+MarginY),0);
			MyEventGuis.Add (NewEvent);
		}
		ResetButtonListeners ();
	}

	// Adds the functions to the buttons, so the buttons close when yes or no is pressed, and if yes is pressed they do extra things
	public void ResetButtonListeners() {
		for (int i = 0; i < MyEventGuis.Count; i++) 
		{	
			// as it passes by references, it needs its own copy
			MyEventGuis[i].name = "Event " +  i.ToString();
			int EventIndex = i;
			UnityEngine.Events.UnityAction NoAction = () =>  
			{
				RemoveEvent(EventIndex);
			};
			MyEventGuis[i].transform.FindChild ("NoButton").GetComponent<Button>().onClick.AddListener(NoAction);
			
			int NoEventIndex = i;
			UnityEngine.Events.UnityAction YesAction = () =>  
			{
				//Debug.LogError ("Yes Event: " + EventIndex);
				YesEvent(NoEventIndex);
			};
			MyEventGuis[i].transform.FindChild ("YesButton").GetComponent<Button>().onClick.AddListener(YesAction);

			MyEventGuis[i].transform.FindChild ("EventLabel").GetComponent<Text>().text = MyEvents[i].EventText;
		}
	}
	// This is what happens when an event is activated
	public void YesEvent(int EventIndex) {
		//Debug.LogError ("Activate Event: " + EventIndex);
		if (EventIndex >= 0 && EventIndex < MyEvents.Count) {
			GameEvent MyEvent = MyEvents [EventIndex];
			GetManager.GetTurnManager().PlayerMoney += MyEvent.CashBonus;
			switch (MyEvent.EventIndex) {
					case (0):
							// Spawn Patient
							MyPatientManager.SpawnPatient ();
							break;
					case (1):
							// Spawn Guard
							MyGuardManager.SpawnGuard ();
							break;
					case (2):
							// Spawn Doctor
							//MyDoctorManager.SpawnDoctor ();
							break;
			}
		}
		RemoveEvent (EventIndex);
	}
	// this just removes the event - activated on both buttons
	public void RemoveEvent(int EventIndex) {
		//Debug.LogError ("Removing: " + EventIndex);
		for (int i = 0; i < MyEventGuis.Count; i++) {
			if (MyEventGuis[i].name == "Event " + EventIndex.ToString()) {
				if (MyEventGuis[i])
					Destroy (MyEventGuis[i]);
				MyEventGuis.RemoveAt (i);
				if (i >= 0 && i < MyEvents.Count)
					MyEvents.RemoveAt (i);
				i = MyEventGuis.Count;		// break the loop
			}
		}
		UpdateGui ();
	}
}
