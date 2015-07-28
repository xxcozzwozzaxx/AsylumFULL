using UnityEngine;
using System.Collections;

public class WallFader : MonoBehaviour {
	public float FadeTime = 3f;
	public float TimeDelay = 3f;
	float TimeStarted = 0;
	Color32 MyBackgroundColor;
	Color32 MyFontColor;
	bool IsForward = true;
	
	
	// Use this for initialization
	void Start () {
		MyBackgroundColor = gameObject.GetComponent<Renderer> ().material.color;
		ResetFade(); 
		IsForward = true;// starts on 255 aplha
	}
	public void ResetFade() { 
		IsForward = false;
		TimeStarted = Time.time+TimeDelay;
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
		Color32 NewBackgroundColor = new Color32(MyBackgroundColor.r,MyBackgroundColor.g,MyBackgroundColor.b,
		                                         (byte)(FadeValue));
		gameObject.GetComponent<Renderer> ().material.color = NewBackgroundColor;
		//Time.timeScale = 0;
		if (FadeValue == EndValue) {
			if (IsForward) {
				//Time.timeScale = 1;
				gameObject.SetActive (false);
			} else {
				TimeStarted = Time.time+TimeDelay;
				IsForward = true;
			}
		}
	}
}
