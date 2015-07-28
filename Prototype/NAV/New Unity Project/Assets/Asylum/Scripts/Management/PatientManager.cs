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
	
	public void SpawnPatient() {	
		SpawnPatient (transform.position);
	}

	public void SpawnPatient(Vector3 NewPatientLocation) {
		Debug.Log ("Spawning patient at: " + Time.time);
		if (PatientPrefab != null) {
			GameObject NewPatient = (GameObject) Instantiate(PatientPrefab, NewPatientLocation, Quaternion.identity);
			MyPatients.Add (NewPatient.GetComponent<Patient>());
			MyPatientGenerator.UpdatePatientName(NewPatient);
			//MyPatientGenerator.UpdatePatientWithStats(NewPatient);
			MyPatientGenerator.UpdatePatientWithDescription(NewPatient);
		}
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

	
	// grabs all the 'patients' in the scene and sets their movement state to true
	/*public void UnSelectPatients() {
		// now start up all the patients movement again! incase any where disabled
		Patient[] MyPatients = GameObject.FindObjectsOfType(typeof(Patient)) as Patient[];
		for (int i = 0; i < MyPatients.Length; i++) {
			MyPatients[i].GetComponent<CharacterSelector>().IsSelected = false;
		}
	}*/
}
