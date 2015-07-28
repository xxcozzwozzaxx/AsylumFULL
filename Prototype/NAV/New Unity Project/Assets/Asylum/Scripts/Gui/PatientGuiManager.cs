using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// managers the 2d patient gui that pops up and displays the stats
public class PatientGuiManager : MonoBehaviour {
	public Patient MyPatient;
	// list of bars in the future
	public BarGui MyBar1;
	public BarGui MyBar2;
	public BarGui MyBar3;
	public BarGui MyBar4;
	public BarGui MyBar5;
	public GameObject MyNameLabel;
	public GameObject MyPatientDescription;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//	public int Aggression, Hallucinations, PhysicalHealth, Hunger, Fatigue;
		UpdatePatientStats(MyPatient.gameObject);
	}

	public void ToggleProfile() {
		if (transform.GetChild (0).gameObject.activeSelf == false)
		transform.GetChild (0).gameObject.SetActive (!transform.GetChild (0).gameObject.activeSelf);
	}
	public void UpdatePatient(GameObject NewPatient) {
		MyPatient = NewPatient.GetComponent<Patient> ();
	}
	public void UpdatePatientStats(GameObject NewPatient) {
		if (MyPatient) {
			MyBar1.Stat = Mathf.FloorToInt(MyPatient.MyStats.getStat("PhysicalHealth").Amount/2.0f);
			MyBar2.Stat = Mathf.FloorToInt(MyPatient.MyStats.getStat("Hunger").Amount/2.0f);
			MyBar3.Stat = Mathf.FloorToInt(MyPatient.MyStats.getStat("Fatigue").Amount/2.0f);
			MyBar4.Stat = Mathf.FloorToInt(MyPatient.MyStats.getStat("Hallusinations").Amount/2.0f);
			MyBar5.Stat = Mathf.FloorToInt(MyPatient.MyStats.getStat("Aggression").Amount/2.0f);
			MyNameLabel.GetComponent<Text>().text = MyPatient.name;
			MyPatientDescription.GetComponent<Text>().text = MyPatient.MyStats.Description;
		}
	}
}
