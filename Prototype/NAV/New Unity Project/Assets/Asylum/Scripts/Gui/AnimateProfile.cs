using UnityEngine;
using System.Collections;

public class AnimateProfile : MonoBehaviour {
	private Vector3 BeginPosition;
	private Vector3 EndPosition;
	private Vector3 DifferencePosition;
	
	public Vector3 MoveToPosition;
	public bool IsForward = true;
	public bool IsAnimating;
	private float FramesPerSecond;
	public float Speed = 3f;
	float Threshold = 1.0f;
	public Vector3 MyPosition;
	public bool IsLeftAlligned;
	public bool IsRightAlligned;
	public float PixelWidth;
	
	// Use this for initialization
	void Start () {
		ResetPositions ();
	}

	public void ResetPositions() {
		//BeginPosition = transform.position;
		if (IsLeftAlligned) {
			BeginPosition.x = PixelWidth;
			MoveToPosition = new Vector3(-PixelWidth*2f,0,0);
		} else if (IsRightAlligned) {
			RectTransform MyCanvas = transform.parent.gameObject.GetComponent<RectTransform>();
			BeginPosition.x = MyCanvas.sizeDelta.x-PixelWidth;
			MoveToPosition = new Vector3(PixelWidth*2f,0,0);
		}
		//Debug.LogError ("Camera.main.pixelWidth: " + Camera.main.pixelWidth);
		BeginPosition.y = 0;
		EndPosition = BeginPosition + MoveToPosition;
		DifferencePosition = BeginPosition - EndPosition;
		DifferencePosition.x = Mathf.Abs(DifferencePosition.x);
		
	}
	public void AnimateBackwards() {
		if (IsAnimating) {
			IsForward = !IsForward;
		} else
			IsAnimating = true;
	}
	// Update is called once per frame
	void Update () {
		if (IsAnimating) {
			AnimateGui();
		}
	}
	public void AnimateGui() {
		ResetPositions ();
		RectTransform MyRectTransform = gameObject.GetComponent<RectTransform>();
		MyPosition = MyRectTransform.anchoredPosition;
		
		Vector3 TargetPosition;
		Vector3 PreviousPosition;
		if (IsForward) {
			TargetPosition = EndPosition;
			PreviousPosition = BeginPosition;
		} else {
			TargetPosition = BeginPosition;
			PreviousPosition = EndPosition;
		}
		//if (IsLerp)
		MyPosition = Vector3.Lerp(MyPosition,TargetPosition,Time.deltaTime*Speed);
		/*else 
			{
				float Direction = 1;
				if (IsForward)
					Direction = -1;
				MyPosition.x += Direction*Speed*Time.deltaTime*(DifferencePosition.x);
				//MyPosition = Vector3.Slerp(MyPosition,TargetPosition,Time.deltaTime*AnimationSpeed);
				if (!IsForward)
					MyPosition.x = Mathf.Clamp (MyPosition.x, PreviousPosition.x, TargetPosition.x);
				else
					MyPosition.x = Mathf.Clamp (MyPosition.x,  TargetPosition.x, PreviousPosition.x);
			}*/
		if (MyPosition.x > TargetPosition.x-Threshold && MyPosition.x < TargetPosition.x+Threshold)
			MyPosition.x = TargetPosition.x;
		MyRectTransform.anchoredPosition = MyPosition;
		if (MyPosition == TargetPosition) {
			IsForward = !IsForward;
			IsAnimating = false;
		}
	}
}
