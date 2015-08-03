using UnityEngine;
using System.Collections;

public class Arrows : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DrawArrowOne()
	{
		GameObject.Find("Arrow1").GetComponent<GUITexture> ().enabled = true;
	}
	public void DrawArrowTwo()
	{
		GameObject.Find("Arrow2").GetComponent<GUITexture> ().enabled = true;
	}
	public void DrawArrowThree()
	{
		GameObject.Find("Arrow3").GetComponent<GUITexture> ().enabled = true;
	}
	public void DrawArrowFour()
	{
		GameObject.Find("Arrow4").GetComponent<GUITexture> ().enabled = true;
	}
	public void DrawArrowFive()
	{
		GameObject.Find("Arrow5").GetComponent<GUITexture> ().enabled = true;
	}
	public void DrawArrowSix()
	{
		GameObject.Find("Arrow6").GetComponent<GUITexture> ().enabled = true;
	}
	public void DrawArrowSeven()
	{
		GameObject.Find("Arrow7").GetComponent<GUITexture> ().enabled = true;
	}
	public void DrawArrowEight()
	{
		GameObject.Find("Arrow8").GetComponent<GUITexture> ().enabled = true;
	}
}
