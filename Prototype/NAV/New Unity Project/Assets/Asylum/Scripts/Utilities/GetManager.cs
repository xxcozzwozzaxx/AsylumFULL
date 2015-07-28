using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GetManager : MonoBehaviour {	
	
	public static TurnManager GetTurnManager() {
		TurnManager[] TurnManagers = FindObjectsOfType(typeof(TurnManager)) as TurnManager[];
		if (TurnManagers.Length > 0)
			return TurnManagers[0];
		else
		return new TurnManager();
	}
	public static PatientManager GetPatientManager() {
		PatientManager[] PatientManagers = FindObjectsOfType(typeof(PatientManager)) as PatientManager[];
		if (PatientManagers.Length > 0)
			return PatientManagers[0];
		else
		return new PatientManager();
	}
	public static GaurdManager GetGuardManager() {
		GaurdManager[] GaurdManagers = FindObjectsOfType(typeof(GaurdManager)) as GaurdManager[];
		if (GaurdManagers.Length > 0)
			return GaurdManagers[0];
		else
			return new GaurdManager();
	}
	
	public static EventsManager GetEventsManager() {
		EventsManager[] EventManagers = FindObjectsOfType(typeof(EventsManager)) as EventsManager[];
		if (EventManagers.Length > 0)
			return EventManagers[0];
		else
			return new EventsManager();
	}
};