using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour {
	public float FadeTime = 3f;
	public float TimeDelay = 3f;
	float TimeStarted = 0;
	Color32 MyBackgroundColor;
	Color32 MyFontColor;
	bool IsForward = true;


	// Use this for initialization
	void Start () {
		MyBackgroundColor = gameObject.GetComponent<RawImage> ().color;
		MyFontColor = gameObject.transform.GetChild(0).gameObject.GetComponent<Text> ().color;
		ResetFade(1); 
		IsForward = true;// starts on 255 aplha
	}
	public void ResetFade(int DayNumber) { 
		IsForward = false;
		TimeStarted = Time.time;
		gameObject.transform.GetChild(0).gameObject.GetComponent<Text> ().text = "Day " + DayNumber.ToString();
		gameObject.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
		float TimePassed = (Time.time - TimeStarted) / FadeTime;
		int FadeValue;
		int StartValue = 255;
		int EndValue = 0;
		if (!IsForward) {
			StartValue = 0;
			EndValue = 255;
		}
		FadeValue = Mathf.RoundToInt (Mathf.Lerp (StartValue, EndValue, TimePassed));

		// now set the background image, and font color as the fade value
		Color32 NewBackgroundColor = new Color32(MyBackgroundColor.r,MyBackgroundColor.g,MyBackgroundColor.b,
		                                         (byte)(FadeValue));
		gameObject.GetComponent<RawImage> ().color = NewBackgroundColor;
		Color32 NewFontColor = new Color32(MyFontColor.r,MyFontColor.g,MyFontColor.b,
		                                   (byte)(FadeValue));

		gameObject.transform.GetChild(0).gameObject.GetComponent<Text> ().color = NewFontColor;

		if (FadeValue == EndValue) {
			if (IsForward) {	// if faded to nothingness
				gameObject.SetActive (false);
			} else {
				TimeStarted = Time.time+TimeDelay;
				IsForward = true;
				//if ()
				GetManager.GetTurnManager().EndDay();
			}
		}
	}
}
