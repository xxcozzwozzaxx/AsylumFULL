using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GaurdManager : MonoBehaviour {
	public List<Guard> MyGuards = new List<Guard>();
	public GameObject GuardPrefab;

	// Use this for initialization
	void Start () {
		GetAllGuardsInScene ();
	}
	public void GetAllGuardsInScene() {
		MyGuards.Clear ();
		// get all Rooms in level and Add to MyRoomsList
		Guard[] MyGuardsInScene = FindObjectsOfType(typeof(Guard)) as Guard[];
		for (int i = 0; i < MyGuardsInScene.Length; i++) {
			MyGuards.Add(MyGuardsInScene[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void SpawnGuard() {
		SpawnGuard (transform.position);
	}
	
	public void SpawnGuard(Vector3 NewSpawnPosition) {
		Debug.Log ("Spawning Guard at: " + Time.time);
		if (GuardPrefab != null) {
			GameObject NewGuard = (GameObject) Instantiate(GuardPrefab, NewSpawnPosition, Quaternion.identity);
			MyGuards.Add (NewGuard.GetComponent<Guard>());
			//MyPatientGenerator.UpdatePatientName(NewPatient);
			//MyPatientGenerator.UpdatePatientWithStats(NewPatient);
		}
	}
}
