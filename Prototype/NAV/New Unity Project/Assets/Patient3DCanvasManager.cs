using UnityEngine;
using System.Collections;

public enum  Patient3DCanvasScreenType {
	None,
	Profile,
	Treat,
	Follow
}

public class Patient3DCanvasManager : MonoBehaviour {
	public GameObject MyPatient;
	public Patient3DCanvasScreenType ScreenType;	// 0 for Profile, 1 for Treat with medicene

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (MyPatient)
			transform.position = MyPatient.gameObject.transform.position + new Vector3 (0, 3, 0);
	}
	public void UpdateScreenType(Patient3DCanvasScreenType NewScreenType) {
		if (ScreenType != NewScreenType) 
		{
			ScreenType = NewScreenType;
			DisableAllScreens();
			switch (ScreenType) {
				case (Patient3DCanvasScreenType.Profile):
				gameObject.transform.FindChild ("ProfileGui").gameObject.SetActive (true);
				break;
				case (Patient3DCanvasScreenType.Treat):
					gameObject.transform.FindChild ("GuardToPatientGui").gameObject.SetActive (true);
					break;
			}
		}
	}
	public void DisableAllScreens() {
		gameObject.transform.FindChild ("ProfileGui").gameObject.SetActive (false);
		gameObject.transform.FindChild ("GuardToPatientGui").gameObject.SetActive (false);
	}
}
