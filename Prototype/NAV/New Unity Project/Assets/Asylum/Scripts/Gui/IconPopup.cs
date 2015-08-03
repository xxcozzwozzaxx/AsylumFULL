using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IconPopup : MonoBehaviour {

	public float endTimer;
	void Start()
	{
		endTimer = 6f;
	}
	void Update()
	{
		float timeLeft = endTimer - Time.time;
		//BehaviourState State = GetComponent<PatientStats>.getState ();
		//Debug.Log (State);

		//OtherScript otherScript = GetComponent<PatientStats>();
		//transform.GetChild (3).gameObject.transform.LookAt (Camera.main.transform.position);
		Debug.Log (timeLeft);
		if (transform.GetChild (3).gameObject.activeSelf == false && timeLeft != 0f) 
		{
			transform.GetChild (3).gameObject.SetActive (true);
			//transform.GetChild (3).gameObject.SetActive (!transform.GetChild (3).gameObject.activeSelf);
		}

		if (transform.GetChild (3).gameObject.activeSelf == true && timeLeft <= 0f) 
		{
			transform.GetChild (3).gameObject.SetActive (false);
		}
	}	

	/*void statePic()
	{
	}*/
}
