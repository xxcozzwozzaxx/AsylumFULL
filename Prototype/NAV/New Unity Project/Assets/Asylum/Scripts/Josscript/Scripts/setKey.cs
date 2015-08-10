using UnityEngine;
using System.Collections;

public class setKey : MonoBehaviour {

	public int tutVal = 1;
	public bool useOnce, used, cache;
	public int secondaryIndex;
	
	public GUITexture ArrowTex;

	// Use this for initialization
	void Start () 
	{
		if(cache)
			name = gameObject.GetInstanceID().ToString();
	}

	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown() 
	{
		if ((!used && useOnce) || !useOnce) 
		{
			DoTheThing ();
			if (useOnce) 
			{
				used = true;
			}
		}

		//enabled = false;
	}
	public void DoTheThing()
	{
		if ((!used && useOnce) || !useOnce) 
		{
			if (cache) 
			{
				DlgManager.GameKeys.Seti ("Interaction", gameObject.GetInstanceID ());
			}
			//DlgManager.GameKeys.Seti ("tutKey", tutVal);
			Camera.main.gameObject.BroadcastMessage ("StartDialogue", tutVal);
			if (ArrowTex != null) 
			{
					ArrowTex.enabled = false;
			}
			if (useOnce) 
			{
					used = true;
			}
		}
	}

	public void secondIndex()
	{
		tutVal = secondaryIndex;
		if (used) 
		{
			used = false;
		}
	}

}
