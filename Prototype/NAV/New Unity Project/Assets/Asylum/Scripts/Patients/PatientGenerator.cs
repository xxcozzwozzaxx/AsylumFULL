using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// generate stats
// description of their history
// generate names
public class PatientGenerator : MonoBehaviour {
	public List<string> PossibleNames = new List<string>();
	public List<string> Descriptions = new List<string>();
	public List<PatientStats> MyStatsPresets = new List<PatientStats>();
	public PatientStats DefaultStats;
	// Use this for initialization
	void Start () {
		DefaultNames ();
		CreateRandomStats ();
	}
	
	// Update is called once per frame
	void Update () {
		//CreateRandomStats ();
	}

	public void DefaultNames() {
		PossibleNames.Add ("Marz");
		PossibleNames.Add ("James");
		PossibleNames.Add ("Coreey");
		PossibleNames.Add ("Angel");
		PossibleNames.Add ("Amy");
		PossibleNames.Add ("Sally");
	}
	public void UpdatePatientName(GameObject MyPatient) {
		int NameIndex = Random.Range (0, PossibleNames.Count);
		MyPatient.name = PossibleNames [NameIndex];
	}
	public void UpdatePatientWithStats(GameObject MyPatient) {
		int StatIndex = Random.Range (0, MyStatsPresets.Count);
		MyPatient.GetComponent<Patient> ().MyStats = MyStatsPresets [StatIndex];
	}
	public void UpdatePatientWithStats(GameObject MyPatient, int StatIndex) {
		StatIndex = Mathf.Clamp (StatIndex, 0, MyStatsPresets.Count);
		MyPatient.GetComponent<Patient> ().MyStats = MyStatsPresets [StatIndex];
	}
	public void UpdatePatientWithDescription(GameObject MyPatient) {
		int NameIndex = Random.Range (0, Descriptions.Count);
		MyPatient.GetComponent<Patient>().MyStats.Description = Descriptions [NameIndex];
	}
	public void CreateRandomStats() {
		for (int i = 0; i < 5; i++) {
			PatientStats NewStats = new PatientStats();
			NewStats.Randomize(5,2);
			MyStatsPresets.Add (NewStats);
		}
	}
}
