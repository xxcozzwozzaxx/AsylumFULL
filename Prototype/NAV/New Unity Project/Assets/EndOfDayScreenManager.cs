using UnityEngine;
using System.Collections;

public class EndOfDayScreenManager : MonoBehaviour {
	public GameObject Screen1;
	public GameObject Screen2;
	public GameObject Screen3;

	// Use this for initialization
	void Start () {
		ChangeTab (0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void ChangeTab(int NewScreenType) {
		Screen1.SetActive (false);
		Screen2.SetActive (false);
		Screen3.SetActive (false);
		switch (NewScreenType) {
		case (0):
			Screen1.SetActive (true);
			break;
		case (1):
			Screen2.SetActive (true);
			break;
		case (2):
			Screen3.SetActive (true);
			break;
		}
	}
}
