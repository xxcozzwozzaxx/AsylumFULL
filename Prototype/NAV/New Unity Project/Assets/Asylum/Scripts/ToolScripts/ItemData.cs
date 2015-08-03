using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("ItemData")]
public class ItemData {
	

	//public string pName, pDesc, pIcon, pMesh;
	//public int osStat, aStat, hallStat, phStat, HunStat, fStat;


	[XmlAttribute("PNAME")]


	public string PNAME { get; set; }
	public string PDESCRIPTION { get; set; }
	
	public int OSSTAT { get; set; }
	public int ASTAT { get; set; }
	public int HALLSTAT { get; set; }
	public int PHSTAT { get; set; }
	public int HUNSTAT { get; set; }
	public int FSTAT { get; set; }
	
	public string PICON { get; set; }
	public string PMESH{ get; set; }



	
}