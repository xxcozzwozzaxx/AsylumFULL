using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

// A different state will be stored per guard
// at the end of each turn (2mins) the functions related will activate
public enum MoveType {
	MoveToPosition,
	FollowPatient,
	MedicatePatient,	//will open inventory to select a medicine type then will stop patient from moving
	RestrainPatient,	//will open inventory to select straitjacket type then will trigger animation
	SendToTreatment,	//send to treatment room, trigger animation, enter treatment state
	SendToCafeteria		// send to cafeteria ... rest is unknown
};

public class PlayerMove {
	public MoveType MyMoveType;
	public GameObject Gaurd;
	public GameObject Doctor;
	public GameObject Patient;
	public Vector3 MoveToPosition;

	public PlayerMove(GameObject GaurdObject, Vector3 NewPosition_) {
		Gaurd = GaurdObject;
		MoveToPosition = NewPosition_;
		MyMoveType = MoveType.MoveToPosition;
	}
	public PlayerMove(MoveType NewMoveType, GameObject GaurdObject, GameObject PatientToMedicate_) {
		Gaurd = GaurdObject;
		Patient = PatientToMedicate_;
		MyMoveType = NewMoveType;
	}
};


public class TurnManager : MonoBehaviour {
	public Text MoneyText;
	public float PlayerMoney = 0;
	public PatientManager MyPatientManager;
	public List<PlayerMove> PlayerMoves = new List<PlayerMove>();
	public List<Room> MyRooms = new List<Room>();

	public bool IsPatientSelected = false;
	public GameObject SelectedPatient;
	public GameObject MyPatientGui;
	public PatientGuiManager MyPatientGuiManager;

	public bool IsGuardSelected = false;
	public GameObject SelectedGuard;
	public GaurdManager MyGuardManager;
	RaycastHit hitInfo;
	//public GameObject LastSelectPatient;
	//public bool HasBeenOrdered = false;
	public int TurnsPassed = 1;
	public GameObject MyEndOfDayGui;


	public void Awake() 
	{
		UnSelectPatient ();
		GetAllRoomsInScene ();
		MyGuardManager = GetManager.GetGuardManager ();
	}
	public void GetAllRoomsInScene() {
		// get all Rooms in level and Add to MyRoomsList
		Room[] MyRoomsScene = FindObjectsOfType(typeof(Room)) as Room[];
		for (int i = 0; i < MyRoomsScene.Length; i++)
			MyRooms.Add(MyRoomsScene[i]);
	}

	void Update () 
	{
		CheckMouseHit ();
		MoneyText.text = "$ " + PlayerMoney.ToString ();
	}

	// This should be added as a turn
	public void MoveSelectedPatientToRoom(int RoomIndex) {
		MoveToRoom (SelectedPatient, RoomIndex);
	}
	public void MoveToRoom(GameObject Bot, int RoomIndex) {
		//Debug.LogError (Bot.name + "'s Moving PatientToRoom: " + RoomIndex);
		for (int i = 0; i < MyRooms.Count; i++) {
			if (MyRooms [i].GetComponent<Room>().RoomIndex == RoomIndex) {
				Bot.GetComponent<Behaviour>().MoveToNewPositionCommand (MyRooms [i].gameObject.transform.position);
				Bot.GetComponent<Patient>().MyStats.MyTreatmentState = MyRooms [i].MyTreatmentState;
				i = MyRooms.Count;
			}
		}
		SelectedGuard.GetComponent<CharacterSelector> ().HasBeenOrdered = true;
		SelectedPatient.GetComponent<CharacterSelector> ().HasBeenOrdered = true;
		UnSelectGuard ();
		UnSelectPatient ();
	}
	public static bool IsRayHitGui() 
	{
		var pointer = new PointerEventData (EventSystem.current);
		// convert to a 2D position
		pointer.position = (Input.mousePosition);
		
		List<RaycastResult> raycastResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointer, raycastResults);
		
		if (raycastResults.Count > 0) {
			return true;
		}
		return false;
	}

	public void CheckMouseHit() 
	{
		hitInfo = new RaycastHit();
		bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
		if (hit && !IsRayHitGui()) 
		{
			
			// Select patient code
			if (hitInfo.transform.gameObject.tag == "Patient")
			{
				if (Input.GetMouseButtonDown(0)) 
				{
					Debug.Log ("Patient Selected!");
					SelectPatient(hitInfo.transform.gameObject);
				} else {
					HighlightCharacter(hitInfo.transform.gameObject);
				}
			} 

			else if (hitInfo.transform.gameObject.tag == "Guard") 
			{
				if (Input.GetMouseButtonDown (0)) {
					Debug.Log ("Guard selected");
					SelectGuard(hitInfo.transform.gameObject);
					UnSelectPatient ();
				} else {
					HighlightCharacter(hitInfo.transform.gameObject);
				}
			}

			else if(hitInfo.transform.gameObject.tag == "Ground") 
			{
				
				if (Input.GetMouseButtonDown (0)) {
					UnSelectPatient();
				}
				Debug.Log ("Ground Selected");
				if (Input.GetMouseButtonDown(1) &&  IsGuardSelected && (SelectedGuard != null)) 
				{
					AddMoveToTurn(SelectedGuard.gameObject, hitInfo.point);
					SelectedGuard.GetComponent<CharacterSelector>().HasBeenOrdered = true;
					UnSelectGuard();
				}
			}
			
		}
		else 
		{
			Debug.Log("No hit");
			
		}
	}

	// Select and Unselect functions
	
	public void HighlightCharacter(GameObject NewSelectedGuard) {
		NewSelectedGuard.GetComponent<CharacterSelector>().IsHighlighted = true;
	}
	public void SelectGuard(GameObject NewSelectedGuard) {
		UnSelectGuard();
		
		if (!NewSelectedGuard.GetComponent<CharacterSelector>().HasBeenOrdered) {
			SelectedGuard = NewSelectedGuard;
			SelectedGuard.GetComponent<CharacterSelector>().IsSelected = true;
			IsGuardSelected = true;
		}
	}
	public void UnSelectGuard () 
	{
		IsGuardSelected = false;
		if (SelectedGuard != null) {
			SelectedGuard.GetComponent<CharacterSelector>().IsSelected = false;
			SelectedGuard = null;	// set targetted patient to nothing
		}
	}
	public UnityAction OnSelectPatientFunction;

	public void SelectPatient (GameObject NewSelectedPatient) 
	{
		if (NewSelectedPatient && MyPatientGui) 
		if (!NewSelectedPatient.GetComponent<CharacterSelector>().HasBeenOrdered) {
			NewSelectedPatient.GetComponent<CharacterSelector>().IsSelected = true;
			SelectedPatient = NewSelectedPatient;
			Patient MyPatient = SelectedPatient.GetComponent<Patient>();
			MyPatientGui.SetActive (true);
			MyPatientGui.GetComponent<Patient3DCanvasManager>().MyPatient = SelectedPatient;
			if (IsGuardSelected) {
				MyPatientGui.GetComponent<Patient3DCanvasManager>().UpdateScreenType(Patient3DCanvasScreenType.Treat);
			} else {
				MyPatientGui.GetComponent<Patient3DCanvasManager>().UpdateScreenType(Patient3DCanvasScreenType.Profile);
			}
			MyPatientGuiManager.GetComponent<PatientGuiManager> ().UpdatePatient (SelectedPatient);
			IsPatientSelected = true;
		}
		OnSelectPatientFunction.Invoke ();
		//UnSelectGuard();
	}
	public void UnSelectPatient () 
	{
		if (SelectedPatient)
			SelectedPatient.GetComponent<CharacterSelector>().IsSelected = false;
		IsPatientSelected = false;
		SelectedPatient = null;	// set targetted patient to nothing
		MyPatientGui.SetActive (false);
	}


	public void EndTurn() 
	{
		UnSelectGuard ();
		UnSelectPatient ();
		//MyPatientManager.DeselectAllPatients ();
		//MyGuardManager.UnSelectGuards ();		// need to implement
		for (int i = PlayerMoves.Count-1; i >= 0; i--) 
		{
			//PlayerMoves[i].Gaurd.GetComponent<Guard>().HasBeenOrdered = false;
			switch (PlayerMoves[i].MyMoveType) {
				case (MoveType.MoveToPosition):
					PlayerMoves[i].Gaurd.GetComponent<Guard>().MoveToLocation(PlayerMoves[i].MoveToPosition);
					break;
				case (MoveType.FollowPatient):
					PlayerMoves[i].Gaurd.GetComponent<Guard>().FollowPatient(PlayerMoves[i].Patient);
						break;
			}
			PlayerMoves.RemoveAt (i);
		}
		DoThing ();	// does the things
		for (int i = 0; i < MyPatientManager.MyPatients.Count; i++) {
			MyPatientManager.MyPatients[i].GetComponent<CharacterSelector>().HasBeenOrdered = false;
		}
		for (int i = 0; i < MyGuardManager.MyGuards.Count; i++) {
			MyGuardManager.MyGuards[i].GetComponent<CharacterSelector>().HasBeenOrdered = false;
		}
		GetManager.GetEventsManager ().AddRandomEvent ();
		TurnsPassed++;
	}
	// when close the end of day screen
	public void StartDay() {
		//Debug.LogError ("Starting day");
		Time.timeScale = 1;
		MyEndOfDayGui.GetComponent<GuiScaleAnimator>().ResetAnimationClose ();
	}
	// maybe put in game manager
	// Called when Day X gui fade-in completes
	public void EndDay() {
		//Debug.LogError ("Ended day");
		MyEndOfDayGui.SetActive (true);
		MyEndOfDayGui.GetComponent<GuiScaleAnimator>().ResetAnimation ();
	}
	public void PauseTime() {
		Time.timeScale = 0;
	}

	public void DoThing() {
		for (int i = 0; i < MyPatientManager.MyPatients.Count; i++) {
			MyPatientManager.MyPatients[i].MyStats.IncreaseTurn(TurnsPassed);
		}
	}

	public void AddPlayerMove(PlayerMove NewMove) {

		bool IsGuardInList = false;
		/// check if gaurd is already in the list
		for (int i = 0; i<PlayerMoves.Count; i++) 
		{
			if(PlayerMoves[i].Gaurd == NewMove.Gaurd) {
				IsGuardInList = true;
				i = PlayerMoves.Count;
			}
		}
		if (!IsGuardInList) 
		{
			PlayerMoves.Add (NewMove);
		}

	}
	public void AddMoveToTurn(GameObject GaurdToMove, Vector3 MoveToPosition) 
	{
		PlayerMove NewMove = new PlayerMove(GaurdToMove, MoveToPosition);
		AddPlayerMove (NewMove);
	}

	public void AddTurnMedicate(GameObject DoctorObject,GameObject PatientObject) 
	{
		PlayerMove NewMove = new PlayerMove(MoveType.MedicatePatient, DoctorObject, PatientObject);
		AddPlayerMove (NewMove);
	}

	public void AddTurnMoveToPatient(GameObject GaurdObject, GameObject PatientObject)
	{
		PlayerMove NewMove = new PlayerMove (MoveType.FollowPatient, GaurdObject, PatientObject);
		AddPlayerMove (NewMove);
	}
}
