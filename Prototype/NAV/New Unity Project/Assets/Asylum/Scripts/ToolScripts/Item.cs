using UnityEngine;
using System.Collections;

public class Item {


	public string Name, Desc;
	public int 	overallSanity,
				Aggression,
				Hallucinations,
				PhysHealth,
				Hunger,
				Fatigue;
	public Texture2D Icon;
	public GameObject Mesh;


	void Init()
	{

		Name = "Unknown";
		Desc = "???";
		Icon = null;
		Mesh = null;
		overallSanity = 0;
		Aggression = 0;
		Hallucinations = 0;
		PhysHealth = 0;
		Hunger = 0;
		Fatigue = 0;

	}
	

	void Init(string pName, int osStat, int aStat,int hallStat, int phStat, int hunStat, int fStat,
	          string pDesc, Texture2D pIcon, GameObject pMesh)
	{
		Name = pName;
		Desc = pDesc;
		Icon = pIcon;
		Mesh = pMesh;
		overallSanity = osStat;
		Aggression = aStat;
		Hallucinations = hallStat;
		PhysHealth = phStat;
		Hunger = hunStat;
		Fatigue = fStat;
		
	}

}

	
