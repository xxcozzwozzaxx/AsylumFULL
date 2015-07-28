using UnityEngine;
using System.Collections;

public class GuiScaleAnimator : MonoBehaviour {
	public Vector2 BeginScale = new Vector2(0,0);
	public Vector2 EndScale = new Vector2(1,1);
	public float AnimationSpeed = 1;
	public bool AnimationDirection;
	float BeginTime;
	RectTransform MyRectTransform;
	public bool IsEndOfDayScreen = false;

	// Use this for initialization
	void Start () {
		MyRectTransform = gameObject.GetComponent<RectTransform> ();
		ResetAnimation ();
	}

	public void ResetAnimation() {
		MyRectTransform = gameObject.GetComponent<RectTransform> ();
		BeginTime = Time.time;
		MyRectTransform.localScale = BeginScale;
		AnimationDirection = true;
	}

	public void ResetAnimationClose() {
		BeginTime = Time.time;
		MyRectTransform.localScale = EndScale;
		AnimationDirection = false;
	}

	public void OnEnable() {
		ResetAnimation ();
	}

	// Update is called once per frame
	void Update () {
		if (Time.timeScale != 0) 
		{
			float TimePassed = Time.time - BeginTime;
			TimePassed *= AnimationSpeed;
			Vector2 NewScale;
			if (AnimationDirection)
			{
				NewScale = new Vector2 (Mathf.Lerp (BeginScale.x, EndScale.x, TimePassed), 
			                               Mathf.Lerp (BeginScale.y, EndScale.y, TimePassed));
			}
			else
			{
				NewScale = new Vector2 (Mathf.Lerp (EndScale.x, BeginScale.x, TimePassed), 
				                        Mathf.Lerp (EndScale.y, BeginScale.y, TimePassed));
			}

			MyRectTransform.localScale = NewScale;

			// if has shrunk to nothingness
			if (NewScale == new Vector2(0,0) && !AnimationDirection) {
				gameObject.SetActive(false);
			} else if (AnimationDirection && IsEndOfDayScreen && NewScale == new Vector2(1,1)) {
				GetManager.GetTurnManager().PauseTime();
			}
		}
	}
}
