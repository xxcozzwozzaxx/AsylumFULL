using UnityEngine;
using System.Collections;

public class setKey : MonoBehaviour {

	public int tutVal = 1;
	
	public GUITexture ArrowTex;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown() 
	{
		DoTheThing ();
		enabled = false;
	}
	public void DoTheThing()
	{
		//DlgManager.GameKeys.Seti ("tutKey", tutVal);
		Camera.main.gameObject.BroadcastMessage("StartDialogue", tutVal);
		
		if(ArrowTex != null)
			ArrowTex.enabled = false;
	}
}
