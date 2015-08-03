using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ItemBuilder : EditorWindow 
{
		
	public string patientName = "Patient Name";
	public string patientDesc = "Patient Description here";
	public string patientIcon = "NONE";
	public string patientMesh = "NONE";
	public int 	overallSanity = -1,
				Aggression= -1,
				Hallucinations= -1,
				PhysHealth= -1,
				Hunger= -1,
				Fatigue= -1;

	public bool Done = false;
	

	[MenuItem("Patients/Profile Builder")]
	
	public static void ShowWindow()
	{
		EditorWindow.GetWindow (typeof(ItemBuilder));
	}

	void AddPatient()
	{

		List<ItemData> patients = new List<ItemData>();
				ItemContainer handler = ItemContainer.Load (Path.Combine (Application.dataPath, "items.xml"));
				foreach (ItemData obj in handler.Items) {
						Debug.Log (obj.PNAME);
						Debug.Log (obj.PDESCRIPTION);
						Debug.Log (obj.PICON);
						Debug.Log(obj.PMESH);
						Debug.Log (obj.OSSTAT);
						Debug.Log (obj.ASTAT);
						Debug.Log (obj.HALLSTAT);
						Debug.Log (obj.PHSTAT);
						Debug.Log (obj.HUNSTAT);
						Debug.Log (obj.FSTAT);
			
						ItemData tempItem = new ItemData ();
						
						tempItem.PNAME = obj.PNAME;
						tempItem.PDESCRIPTION = obj.PDESCRIPTION;
						tempItem.OSSTAT = obj.OSSTAT;
						tempItem.ASTAT = obj.ASTAT;
						tempItem.HALLSTAT = obj.HALLSTAT;
						tempItem.PHSTAT = obj.PHSTAT;
						tempItem.HUNSTAT = obj.HUNSTAT;
						tempItem.FSTAT = obj.FSTAT;
						tempItem.PICON = obj.PICON;
						tempItem.PMESH = obj.PMESH;


						patients.Add (tempItem);

				}

				ItemData newItem = new ItemData ();
	
				if (patientName != "") 
				{
						newItem.PNAME = patientName;	
				}
				if (patientDesc != "") 
				{
					newItem.PDESCRIPTION = patientDesc;
				}
				if(patientIcon != "")
				{
					newItem.PICON = patientIcon;
				}
				if(patientMesh != "")
				{
					newItem.PMESH = patientMesh;
				}
				if(overallSanity != -1)
				{
					newItem.OSSTAT = overallSanity;
				}
				if(Aggression != -1)
				{
					newItem.ASTAT = Aggression;
				}
				if(Hallucinations != -1)
				{
					newItem.HALLSTAT = Hallucinations;
				}
				if(PhysHealth != -1)
				{
					newItem.PHSTAT = PhysHealth;
				}
				if(Hunger != -1)
				{
					newItem.HUNSTAT = Hunger;
				}
				if(Fatigue != -1)
				{
					newItem.FSTAT = Fatigue;
				}

		patients.Add (newItem);
		handler.Items = patients.ToArray();
		handler.Save (Path.Combine (Application.dataPath, "items.xml"));
		}
	
	void OnGUI()
	{
		GUILayout.Label ("Item Builder", EditorStyles.boldLabel);
		
		patientName = EditorGUILayout.TextField ("Patient Name", patientName);
		patientDesc = EditorGUILayout.TextArea (patientDesc,GUILayout.Height(50));
		patientIcon = EditorGUILayout.TextField ("Patient Icon", patientIcon);
		patientMesh = EditorGUILayout.TextField ("Patient Mesh", patientMesh);
		
		overallSanity = EditorGUILayout.IntField ("Overall Sanity", overallSanity);
		Aggression = EditorGUILayout.IntField ("Aggression", Aggression);
		Hallucinations = EditorGUILayout.IntField ("Hallucinations", Hallucinations);
		PhysHealth = EditorGUILayout.IntField ("Physical Health", PhysHealth);
		Hunger = EditorGUILayout.IntField ("Hunger", Hunger);
		Fatigue = EditorGUILayout.IntField ("Fatigue", Fatigue);

		if (GUILayout.Button ("Create", EditorStyles.miniButton)) 
		{
			AddPatient();
		}


		
	}
}