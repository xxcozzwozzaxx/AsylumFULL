using UnityEngine;
using System.Collections;

using gui = UnityEngine.GUILayout;

// Camera Movement mainly

public class PlayerMovement : MonoBehaviour {
	Vector3 CameraPosition = new Vector3();
	public float MovementSpeedMultiple = 100.0f;	// used for WASD movement
	public float ZoomSpeedMultiple = 100.0f;		// used for zooming in and out with mouse scroll

	public Vector3 LimiterMinimum = new Vector3(-16,1,-16);
	public Vector3 LimiterMaximum = new Vector3(16,15,16);
	public float MovementSpeed = 2f;
	public float ButtonSize = 100f;

	public bool IsInput = true;
	public bool IsPerspective;
	public float BorderLength = 15f;
	public bool IsDebug = false;

	// used for panning and zooming
	Vector2 TouchDelta = new Vector2();
	Vector2 OriginalTouch = new Vector2();
	Vector2 CurrentTouch = new Vector2();
	// used for pinching
	Vector2 OriginalTouch2 = new Vector2();
	Vector2 TouchDelta2 = new Vector2();
	Vector2 CurrentTouch2 = new Vector2();

	Vector3 OriginalMovement = new Vector3();
	bool IsPinching = false;

	// Use this for initialization
	void Start () {
		CameraPosition.y = gameObject.transform.position.y;
	}

	// Update is called once per frame
	void Update () {
		if (IsInput) {

			if (Input.touchCount > 0) {
				if (Input.GetTouch(0).phase == TouchPhase.Began) {
					OriginalTouch = Input.touches[0].position;
					if (Input.touches.Length > 1) {
						OriginalTouch2 = Input.touches[1].position;
						IsPinching = true;
					} else {
						IsPinching = false;
					}
					OriginalMovement = CameraPosition;
				} else if (Input.GetTouch(0).phase == TouchPhase.Moved) {// Get movement of the finger since last frame
					if (!IsPinching && Input.touches.Length > 1 && Input.GetTouch(1).phase == TouchPhase.Began) {
						OriginalTouch = Input.touches[0].position;
						OriginalTouch2 = Input.touches[1].position;
						OriginalMovement = CameraPosition;
						IsPinching = true;
					}
					CurrentTouch = Input.GetTouch(0).position;
					TouchDelta = OriginalTouch-CurrentTouch;
					
					// Move object across XY plane
					if (!IsPinching) {
						CameraPosition = OriginalMovement + new Vector3(MovementSpeed*(TouchDelta.x/100f), 0, MovementSpeed*(TouchDelta.y/100f));
					} else {
						CurrentTouch2 = Input.GetTouch(1).position;
						TouchDelta2 = OriginalTouch2-CurrentTouch2;
						float OriginalDistance = Vector3.Distance (OriginalTouch2,OriginalTouch);
						CurrentDistance = Vector3.Distance (CurrentTouch2,CurrentTouch);
						ZoomDistance = MovementSpeed*((OriginalDistance-CurrentDistance)/250f);
						CameraPosition.y = OriginalMovement.y + ZoomDistance;
					}
				}
				if (Input.GetTouch(0).phase == TouchPhase.Ended && !IsPinching) {
					TouchDelta = new Vector2();
				}
				if (IsPinching && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(1).phase == TouchPhase.Ended)) {
					IsPinching = false;
					OriginalMovement = CameraPosition;
					if (Input.GetTouch(0).phase == TouchPhase.Ended) {
						OriginalTouch = Input.touches[1].position;
					} else {
						OriginalTouch = Input.touches[0].position;
					}
				}
			}
			Move ();	// moves the camera
		}
	}
	float ZoomDistance;
	float CurrentDistance;
	public void OnGUI() {
		if (IsDebug) {
			//if (gui.Button("+", GUILayout.Width(ButtonSize), GUILayout.Height(ButtonSize)))
			//	ZoomIn();
			//if (gui.Button("-", GUILayout.Width(ButtonSize), GUILayout.Height(ButtonSize)))
			//	ZoomOut();

			GUI.skin.label.fontSize = 40;
			gui.Label ("Is Touch Enabled: " + Input.touchSupported.ToString (), GUILayout.Width(Screen.width));
			//gui.Label ("TouchDelta: " + TouchDelta.ToString(), GUILayout.Width(Screen.width));
			//gui.Label ("Original Touch Position: " + OriginalTouch.ToString(), GUILayout.Width(Screen.width));
			//gui.Label ("Current Touch Position: " + CurrentTouch.ToString(), GUILayout.Width(Screen.width));
			//gui.Label ("Original Camera Position: " + OriginalMovement.ToString(), GUILayout.Width(Screen.width));
			//gui.Label ("Current Camera Position: " + CameraPosition.ToString(), GUILayout.Width(Screen.width));
			gui.Label ("Is Pinching? " + IsPinching.ToString(), GUILayout.Width(Screen.width));
			gui.Label ("ZoomDistance? " + ZoomDistance.ToString(), GUILayout.Width(Screen.width));
			gui.Label ("CurrentDistance? " + CurrentDistance.ToString(), GUILayout.Width(Screen.width));

		}
	}

	public void EnableMovement() {
		IsInput = true;
	}
	public void DisableMovement() {
		IsInput = false;
	}

	
	public void TogglePerspective() {
		IsPerspective = !IsPerspective;
		//if (IsPerspective) 
		{
			gameObject.GetComponent<Camera>().orthographic = !IsPerspective;
		}
	}	

	public Vector3 GetForwardDirection () {
		Vector3 ForwardDirection = transform.forward;
		ForwardDirection.y = 0;
		return ForwardDirection;
	}
	public Vector3 GetRightDirection () {
		Vector3 ForwardDirection = transform.right;
		ForwardDirection.y = 0;
		return ForwardDirection;
	}
	public bool IsMouseInRightBorder() {
		Vector3 MousePosition = Input.mousePosition;
		Vector2 ScreenSize = new Vector2(Screen.width, Screen.height);
		if (MousePosition.x >= ScreenSize.x - BorderLength && MousePosition.x <= ScreenSize.x)
			return true;
		return false;
	}
	public bool IsMouseInLeftBorder() {
		Vector3 MousePosition = Input.mousePosition;
		Vector2 ScreenSize = new Vector2(Screen.width, Screen.height);
		if (MousePosition.x <= BorderLength && MousePosition.x >= 0)
			return true;
		return false;
	}
	public bool IsMouseInTopBorder() {
		Vector3 MousePosition = Input.mousePosition;
		Vector2 ScreenSize = new Vector2(Screen.width, Screen.height);
		if (MousePosition.y >= ScreenSize.y - BorderLength && MousePosition.y <= ScreenSize.y)
			return true; 

		return false;
	}
	public bool IsMouseInBottomBorder() {
		Vector3 MousePosition = Input.mousePosition;
		Vector2 ScreenSize = new Vector2(Screen.width, Screen.height);
		if (MousePosition.y <= BorderLength && MousePosition.y >= 0)
			return true;
		return false;
	}
	public void Move() {
		float MovementSpeed = MovementSpeedMultiple * Time.deltaTime;
		float ZoomSpeed = ZoomSpeedMultiple * Time.deltaTime;
		if (Input.GetKey (KeyCode.LeftShift)) {
			MovementSpeed *= 3.0f;
			ZoomSpeed *= 3.0f;
		}
		if (Input.GetKey (KeyCode.W)) {
			CameraPosition += new Vector3(0,0,MovementSpeed);
		}
		else if (Input.GetKey (KeyCode.S)) {
			CameraPosition -= new Vector3(0,0,MovementSpeed);
		}
		if (Input.GetKey (KeyCode.A)) {
			CameraPosition -= new Vector3(MovementSpeed,0,0);
		}
		else if (Input.GetKey (KeyCode.D)) {
			CameraPosition += new Vector3(MovementSpeed,0,0);
		}
		if (!TurnManager.IsRayHitGui ()) {
			if (IsMouseInTopBorder()) {
				CameraPosition += new Vector3(0,0,MovementSpeed);
			}
			else if (IsMouseInBottomBorder()) {
				CameraPosition -= new Vector3(0,0,MovementSpeed);
			}
			if (IsMouseInLeftBorder()) {
				CameraPosition -= new Vector3(MovementSpeed,0,0);
			}
			else if (IsMouseInRightBorder()) {
				CameraPosition += new Vector3(MovementSpeed,0,0);
			}
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			ZoomOut(ZoomSpeed);
		}
		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			ZoomIn(ZoomSpeed);
		}

		CameraPosition.x = Mathf.Clamp (CameraPosition.x, LimiterMinimum.x, LimiterMaximum.x);
		CameraPosition.y = Mathf.Clamp (CameraPosition.y, LimiterMinimum.y, LimiterMaximum.y);
		CameraPosition.z = Mathf.Clamp (CameraPosition.z, LimiterMinimum.z, LimiterMaximum.z);
		float KeepY = gameObject.transform.position.y;
		gameObject.transform.position = CameraPosition.x*GetRightDirection() +  CameraPosition.z*GetForwardDirection();
		gameObject.transform.position = new Vector3(gameObject.transform.position.x, CameraPosition.y, gameObject.transform.position.z);

	}

	public void ZoomIn() {
		ZoomIn (0.5f);
	}
	public void ZoomOut() {
		ZoomOut (0.5f);
	}

	public void ZoomOut(float ZoomSpeed) {
		if (gameObject.GetComponent<Camera> ().orthographic) {
			gameObject.GetComponent<Camera> ().orthographicSize += ZoomSpeed;
		} else {
			CameraPosition.y += ZoomSpeed;
		}
	}
	public void ZoomIn(float ZoomSpeed) {
		if (gameObject.GetComponent<Camera> ().orthographic) {
			gameObject.GetComponent<Camera> ().orthographicSize -= ZoomSpeed;
		} else {
			CameraPosition.y -= ZoomSpeed;
		}
	}

}
