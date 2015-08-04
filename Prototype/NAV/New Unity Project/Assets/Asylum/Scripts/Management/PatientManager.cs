using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Things to do:
// make this control the selected patient


public class PatientManager : MonoBehaviour {
	public List<Patient> MyPatients = new List<Patient>();
	public GameObject PatientPrefab;
	public int PatientSelected = 0;
	private PatientGenerator MyPatientGenerator;
	float TimeSinceSpawned = 0;
	public float SpawnerCoolDown = 0f;

	// Use this for initialization
	void Start () {
		TimeSinceSpawned = Time.time;
		MyPatientGenerator = GetComponent<PatientGenerator> ();
		GetAllPatientsInScene ();
	}

	public void GetAllPatientsInScene() {
		MyPatients.Clear ();
		// get all Rooms in level and Add to MyRoomsList
		Patient[] MyPatientsInScene = FindObjectsOfType(typeof(Patient)) as Patient[];
		for (int i = 0; i < MyPatientsInScene.Length; i++) {
			MyPatients.Add(MyPatientsInScene[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		SpawnPatientOnTimer ();
	}

	void SpawnPatientOnTimer() {
		if (SpawnerCoolDown != 0)
		if (Time.time - TimeSinceSpawned > SpawnerCoolDown) {
			SpawnPatient(gameObject.transform.position);
			TimeSinceSpawned = Time.time;
		}
	}

	public void RemovePatient(int PatientIndex) {
		Destroy (MyPatients [PatientIndex].gameObject);
		MyPatients.RemoveAt (PatientIndex);
	}
	
	public GameObject SpawnPatient() {	
		return SpawnPatient (transform.position);
	}

	public GameObject SpawnPatient(Vector3 NewPatientLocation) {
		if (MyPatientGenerator == null) {
			MyPatientGenerator = GetComponent<PatientGenerator> ();
		}
		//Debug.Log ("Spawning patient at: " + Time.time);
		if (PatientPrefab != null) 
		{
			Debug.LogError ("spawnin le patient!");
			GameObject NewPatient = (GameObject) Instantiate(PatientPrefab, NewPatientLocation, Quaternion.identity);
			MyPatients.Add (NewPatient.GetComponent<Patient>());
			if (MyPatientGenerator){
				//MyPatientGenerator.UpdatePatientName(NewPatient);
				MyPatientGenerator.UpdatePatientWithStats(NewPatient);
				//MyPatientGenerator.UpdatePatientWithDescription(NewPatient);
			}
			return NewPatient;
		}
		Debug.LogError ("NO patient prefab... check patient manager!");
		return null;
	}

	public void SelectPatient(int PatientIndex) {
		DeselectAllPatients ();
		MyPatients[PatientIndex].GetComponent<CharacterSelector>().IsSelected = true;
		PatientSelected = PatientIndex;
	}

	public void DeselectAllPatients() {
		for (int i = 0; i < MyPatients.Count; i++) {
			//MyPatients[i].Select(false);
			MyPatients[i].GetComponent<CharacterSelector>().IsSelected = false;
		}
	}
	public void AddPatient(string Name, string Description, int Stat1, int Stat2, int Stat3, int Stat4, int Stat5, int Stat6) {
		//GameObject NewPatient = SpawnPatient ();
		Debug.LogError ("spawnin le : " + Name);
		PatientStats BlargPatient = new PatientStats ();
		BlargPatient = MyPatientGenerator.DefaultStats;
		BlargPatient.Name = Name;
		BlargPatient.Description = Description;
		BlargPatient.setStat("Overall Insanity", Stat1);
		BlargPatient.setStat("Aggression", Stat2);
		BlargPatient.setStat("Hallucinations", Stat3);
		BlargPatient.setStat("Physical Health", Stat4);
		BlargPatient.setStat("Hunger", Stat5);
		BlargPatient.setStat("Fatigue", Stat6);
		MyPatientGenerator.MyStatsPresets.Add (BlargPatient);
	}
	
	// grabs all the 'patients' in the scene and sets their movement state to true
	/*public void UnSelectPatients() {
		// now start up all the patients movement again! incase any where disabled
		Patient[] MyPatients = GameObject.FindObjectsOfType(typeof(Patient)) as Patient[];
		for (int i = 0; i < MyPatients.Length; i++) {
			MyPatients[i].GetComponent<CharacterSelector>().IsSelected = false;
		}
	}*/
}
