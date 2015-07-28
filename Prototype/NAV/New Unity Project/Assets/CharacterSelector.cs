using UnityEngine;
using System.Collections;

public class CharacterSelector : MonoBehaviour {
	public bool IsHighlighted = false;
	public bool IsSelected = false;
	public bool HasBeenOrdered = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		UpdateSelectionEffects ();
		// each frame we can assume its not highlighted, until the ray cast from turn manager checks
		IsHighlighted = false;
	}

	public void UpdateSelectionEffects() {
		if (gameObject.transform.FindChild("HighlightMesh") && gameObject.transform.FindChild("SelectionMesh") && gameObject.transform.FindChild("DisabledMesh")) 
		{
			// update selected mesh
			if (HasBeenOrdered) {
				IsSelected = false;
				gameObject.transform.FindChild("DisabledMesh").gameObject.SetActive (HasBeenOrdered);
				gameObject.transform.FindChild("SelectionMesh").gameObject.SetActive(false);
				gameObject.transform.FindChild("HighlightMesh").gameObject.SetActive(false);
			} else if (IsSelected) {
				gameObject.transform.FindChild("SelectionMesh").gameObject.SetActive (IsSelected);
				gameObject.transform.FindChild("DisabledMesh").gameObject.SetActive(false);
				gameObject.transform.FindChild("HighlightMesh").gameObject.SetActive(false);
			} else {
				gameObject.transform.FindChild("HighlightMesh").gameObject.SetActive (IsHighlighted);
				gameObject.transform.FindChild("DisabledMesh").gameObject.SetActive(false);
				gameObject.transform.FindChild("SelectionMesh").gameObject.SetActive(false);
			}
		}
	}
}
