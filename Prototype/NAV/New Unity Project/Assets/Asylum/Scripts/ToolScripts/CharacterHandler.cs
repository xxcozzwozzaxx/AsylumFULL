using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CharacterHandler : MonoBehaviour {


	public List<Item> Inventory = new List<Item>();
	

	// Use this for initialization
	void Start () {

		ItemContainer handler = ItemContainer.Load(Path.Combine(Application.dataPath, "items.xml"));
		foreach(ItemData obj in handler.Items)
		{
			Debug.Log(obj.PNAME);
			Debug.Log(obj.PDESCRIPTION);
			Debug.Log(obj.PICON);
			Debug.Log(obj.PMESH);
			Debug.Log(obj.OSSTAT);
			Debug.Log(obj.ASTAT);
			Debug.Log(obj.HALLSTAT);
			Debug.Log(obj.PHSTAT);
			Debug.Log(obj.HUNSTAT);
			Debug.Log(obj.FSTAT);

			Item tempItem = new Item();
			tempItem.Name = obj.PNAME;
			tempItem.Desc = obj.PDESCRIPTION;
			tempItem.overallSanity = obj.OSSTAT;
			tempItem.Aggression = obj.ASTAT;
			tempItem.Hallucinations = obj.HALLSTAT;
			tempItem.PhysHealth = obj.PHSTAT;
			tempItem.Hunger = obj.HUNSTAT;
			tempItem.Fatigue = obj.FSTAT;



		
			if (obj.PICON != "NONE") 
			{
				tempItem.Icon = Resources.Load("Icons/"+obj.PICON)as Texture2D;
			}
			if (obj.PMESH != "NONE") 
			{
				tempItem.Mesh = Resources.Load(obj.PMESH)as GameObject;
			}
			else
			{
				tempItem.Mesh = null;
			}

			Inventory.Add(tempItem);
			

		}

	}
	
}
