/*
	Josscript allows for two different methods of adding data to a data block.
	
	1. The shorthand form:
	<dataset_name>[field_name[=value][;]...]

	Example:
	<book>name=Journey to the West;author=Wu Cheng'en;price=12.00;owner=Shell;
	
	2. Normal form:
	<dataset_name>
	[field_name][value]
	[...]
	
	Example:
	<book>
	[name]Journey to the West
	[author]	Wu Cheng'en
	[price] 12.00
	[owner]
	Shell
/*

/* ------------------------------------------------------------------------
	Josscript Function List
	-----------------------
	virtual public void Initialize() 
	virtual public JossData First
	virtual public JossData Last 
	virtual public bool LoadFile(string filename) 
	virtual public bool OpenFile(string FileName)
	virtual public bool ParseFile(string data) 
	virtual public bool Load(string PrefName)
	virtual public bool Save(string PrefName)
	virtual public bool SaveFile(string FileName)

	virtual public string ToString(bool include_id = true)
	
	virtual public void ImbedJossData() 
	virtual public bool Join(Josscript other)

	virtual public bool AddNode(string data_type, string add_data = "") 
	virtual public bool InsertNode(string data_type, string add_data = "") 
	virtual public bool InsertNode(int index, string data_type, string add_data="") 
	virtual public bool RemoveNode(int index) 
	virtual public bool RemoveCurrentNode() 

	virtual public bool PositionAtNode(int index) 
	virtual public bool PositionAtFirstNode() 
	virtual public bool PositionAtLastNode() 
	virtual public bool PositionAtNextNode()
	virtual public bool PositionAtPreviousNode() 
	virtual public bool PositionAtFirstNodeOfType(string data_type) 
	virtual public bool ContainsANodeOfType(string data_type) 

	virtual public JossData GetFirstNodeOfType(string data_type) 
	virtual public int GetFirstNodeOfTypei(string data_type) 
	virtual public JossData GetLastNodeOfType(string data_type) 
	virtual public int GetLastNodeOfTypei(string data_type) 

	virtual public List<JossData> DataWithField(string field, string val = "") 
	
	virtual public List<JossData> Children(int index = -1) 
	virtual public List<JossData> AllChildNodes(int index = -1) 
	virtual public List<int> Childreni(int index = -1) 
	virtual public List<int> AllChildNodesi(int index = -1) 
	virtual public List<JossData> AllDataOfType(string type_name, int starting_index = 0, string stop_at_data_type = "")
	virtual public List<int> AllDataOfTypei(string type_name, int starting_index = 0, string stop_at_data_type = "")

	Josscript Properties
	-----------------------
	public float JossVersion
	public JossData this [int index] 
	virtual public int Count 
	virtual public float DocumentJossVersion

/* ----------------------------------------------------------------------- */

/* ------------------------------------------------------------------------
	Josscript Function List
	-----------------------
	public void Clear()

	virtual public void Set(string name, string data) 
	virtual public int Int(string named) 
	virtual public bool Bool(string named)
	virtual public Rect Rect4(string named) 
	virtual public float Float(string named) 
	virtual public string String(string named) 
	virtual public Vector3 Vector(string named) 
	virtual public Quaternion Quat(string named) 
	virtual public void AddToData(string value) 
	virtual public void ProcessCombinedFields(string combined) 
	virtual public string ToString(bool include_id = true)
	virtual public bool Remove(string name = "value") 
	virtual public JossData Copy(bool include_id = true)

	Josscript Properties
	-----------------------
	public string this 					[string field_name]
	virtual public int 					ID
	public string						data_type
	public Dictionary<string, string>	defined
	public List<string>					data

/* ----------------------------------------------------------------------- */

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public enum JossCopyMode {new_id, old_id, no_id }

public class JossData : IDisposable {
	public string						data_type;
	public Dictionary<string, string>	defined;
	public List<string>					data;	

	bool disposed = false;
	
	static public implicit operator bool(JossData v) { return null != v; }

	public string this [string field_name] {
		get { return String( field_name ); }
		set { Set( field_name, value); }
	}

	public int ID {
		get {if (defined.ContainsKey("id")) return int.Parse( defined["id"] ); return 0; }
		set { Set("id", value.ToString() ); }
	}
		
	public JossData(int id = 0) {
		data	= new List<string>();
		defined = new Dictionary<string, string>();
		ID = id;
	}
	
	public void Clear() {
		defined.Clear();
		data.Clear();
	}
	
	virtual public bool Remove(string name = "value") {
		bool result = defined.ContainsKey(name);
		if (result)
			defined.Remove(name);
		
		return result;
	}
	
	virtual public string ToString(bool include_id = true) {
		string result = "<"+data_type+">";
		foreach(var d in defined)
			if (d.Key != "id" || include_id)
			result += "\n\t[" + d.Key + "]" + d.Value;

		foreach(string s in data)
			if (s != string.Empty)
				result += "\n\t" + s;
			
		return result;
	}
	
	virtual public void Set(string name, string data) {
		if (defined.ContainsKey(name) )
			defined[name] = data;
		else
			defined.Add(name, data);
	}
	
	virtual public int Int(string named = "value") {
		string result = string.Empty;
		if ( defined.TryGetValue(named, out result) ) {
			int value;
			if ( int.TryParse(result, out value) )
				return value;
			else
				return 0;
		}
			
		return 0;
	}
	
	virtual public float Float(string named = "value") {
		string result = string.Empty;
		if ( defined.TryGetValue(named, out result) ) {
			float value;
			if ( float.TryParse(result, out value) )
				return value;
			else
				return 0;
		}
			
		return 0;
	}
	
	virtual public string String(string named = "value") {
		string result = string.Empty;
		if ( defined.TryGetValue(named, out result) )
			return result;
			
		return string.Empty;
	}
	
	virtual public bool Bool(string named = "value") {
		string result = string.Empty;
		int i = 0;
		if ( defined.TryGetValue(named, out result) ) {
			result = result.Trim();
			if ( result.ToLower() == "true" )
				return true;
				
			if (int.TryParse(result, out i))
				return ( i > 0);
		}
		return false;
	}
	
	virtual public Vector3 Vector(string named = "value") {
		string result = string.Empty;
		if ( defined.TryGetValue(named, out result) ) {

			if (result.Length > 6) { // minimum possible length for Vector3 string
	      		string[] splitString = result.Substring(1, result.Length - 2).Split(',');
	      		if (splitString.Length == 3) {
		      		Vector3 outVector3 = new Vector3(0,0,0);
					outVector3.x = float.Parse(splitString[0]);
					outVector3.y = float.Parse(splitString[1]);
					outVector3.z = float.Parse(splitString[2]);
					return outVector3;
	      		}
			}
		}
			
		return Vector3.zero;
	}
	
	virtual public Quaternion Quat(string named = "value") {
		string result = string.Empty;
		if ( defined.TryGetValue(named, out result) ) {

			if (result.Length > 8) { // minimum possible length for Vector3 string
	      		string[] splitString = result.Substring(1, result.Length - 2).Split(',');
	      		if (splitString.Length == 4) {
		      		Quaternion outQ = new Quaternion(0,0,0,0);
					outQ.x = float.Parse(splitString[0]);
					outQ.y = float.Parse(splitString[1]);
					outQ.z = float.Parse(splitString[2]);
					outQ.w = float.Parse(splitString[3]);
					return outQ;
	      		}
			}
		}
			
		return Quaternion.identity;
	}
	
	virtual public Rect Rect4(string named = "value") {
		string result = string.Empty;
		if ( defined.TryGetValue(named, out result) ) {

			if (result.Length > 8) { // minimum possible length for Vector3 string
	      		string[] splitString = result.Substring(1, result.Length - 2).Split(',');
	      		if (splitString.Length == 4) {
		      		Rect outQ = new Rect(0,0,0,0);
					outQ.x = float.Parse(splitString[0]);
					outQ.y = float.Parse(splitString[1]);
					outQ.width = float.Parse(splitString[2]);
					outQ.height = float.Parse(splitString[3]);
					return outQ;
	      		}
			}
		}
			
		return new Rect(0,0,0,0);
	}

	virtual public void AddToData(string value) {
		data.Add(value);
	}
	
	virtual public void ProcessCombinedFields(string combined) {
		if (combined == string.Empty)
			return;
			
		string[] fields = combined.Split(';');
		foreach (string field in fields) {
			if (field.IndexOf('=') == -1) {
				AddToData(field.Trim());
			} else {
				string[] keyVal = field.Split('=');
				Set( keyVal[0].Trim(), keyVal[1].Trim() );
			}
		}
	}
	
	virtual public JossData Copy(JossCopyMode mode = JossCopyMode.no_id, string id_value="-1") {
		JossData result = new JossData();
		result.data_type = this.data_type;
		foreach (var data in this.defined) {
			if (data.Key != "id") {
				result.Set(data.Key, data.Value);
			} else {
				switch (mode) {
					//keep the original id....
				 	case JossCopyMode.old_id:
						result.Set("id", data.Value);
						break;
						
					case JossCopyMode.new_id : 
						result.Set("id", id_value);
						break;
						
					case JossCopyMode.no_id :
						result.Remove("id");
						break;
				}
			}
		}			
		foreach (string s in this.data)
			result.data.Add(s);
			
		return result;
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if(!this.disposed)
		{
			this.disposed = true;
			if(disposing)
			{
				Clear();
				defined = null;
				data = null;
			}
		}
	}

	~JossData() {
		Dispose(false);
	}

}

public class Josscript : IDisposable {
	const float __JossVersion = 2.0f;

	bool disposed = false;

	public List<JossData>			Elements;	
	public JossData					Node;
	
	static public float JossVersion { get { return __JossVersion; } }
	virtual public float DocumentJossVersion { get { return PositionAtFirstNodeOfType("Joss Header") ? Node.Float("Version") : 0f; }}	

	public JossData this [int index] {
		get { return Count > index ? Elements [index] : null; }
	}

	virtual public JossData Last {
		get { return (Elements.Count == 0) ? null : Elements[Elements.Count - 1]; }
	}
	
	virtual public JossData First {
		get { return (Elements.Count == 0) ? null : Elements[0]; }
	}
	
	virtual public int Count {
		get { return null == Elements ? 0 : Elements.Count; }
	}
	
	public Josscript(string filename = "") {
		if (string.Empty != filename) {
			if (!LoadFile(filename))
				Initialize();
		} else {
			Initialize();
		}
	}
	
	virtual public void Initialize() {
		Node = null;
		if (null != Elements)
			Elements.Clear();
		Elements	= new List<JossData>();
	}
	
	virtual public bool OpenFile(string FileName) {
		StreamReader f = new StreamReader(FileName);
		if (null == f)
			return false;
			
		string file = string.Empty;
		string line = string.Empty;
	    while ((line = f.ReadLine()) != null) 
			file += line + "\n";
		return ParseFile(file);
	}
	
	virtual public void ImbedJossData() {
		if (PositionAtFirstNodeOfType("Joss Header")) 
			Node.Set("Version", JossVersion.ToString());
		else
			AddNode("Joss Header","Version="+JossVersion);
	}
	
	virtual public bool Load(string PrefName) {
		string Joss = PlayerPrefs.GetString(PrefName, string.Empty);
		if (Joss == string.Empty)
			return false;
			
		ParseFile(Joss);
		return true;
	}
	
	virtual public bool LoadFile(string filename) {
		TextAsset FileResource = (TextAsset)Resources.Load(filename, typeof(TextAsset));
		if (null != FileResource) 
			return ParseFile( FileResource.text );
		
		return false;
	}
	
	virtual public bool SaveFile(string FileName, bool include_id = true) {
		StreamWriter f = new StreamWriter(File.Open(FileName, FileMode.Create));
		if (null == f)
			return false;
			
		ImbedJossData();
		foreach(JossData node in Elements) {
			if (node.data_type == "Joss Header")
				f.WriteLine(node.ToString(false) + "\n");
			else
				f.WriteLine(node.ToString(include_id) + "\n");
		}
		f.Close();
		return true;
	}
	
	virtual public bool Save(string PrefName, bool include_id = true) {
		
		PlayerPrefs.SetString(PrefName, ToString(include_id) );
		return true;
	}
	
	virtual public string ToString(bool include_id = true) {
		ImbedJossData();
		string pref = string.Empty;
		foreach(JossData node in Elements)
			if (node.data_type == "Joss Header")
				pref += node.ToString(false) + "\n";
			else
				pref += node.ToString(include_id) + "\n";

		return pref;
	}
	
	virtual public bool ParseFile(string data) {
		if (data == string.Empty)
			return false;
			
		Initialize();
		string[] lines = data.Split('\n');
		foreach (string line in lines) {
			//find and remove comments from the parser
			//take into account URL strings '://'
			string l = line;
			int commentIndex = l.IndexOf("//");
			if (commentIndex > -1) {
				int urlIndex = 0;
				if (commentIndex > 0) {
					while (l.IndexOf("://", urlIndex) == commentIndex - 1) {
						urlIndex = commentIndex + 1;
						commentIndex = l.IndexOf("//", urlIndex);
					}
				}

				if (commentIndex > -1)
					l = l.Substring(0, commentIndex);
			}
			l = l.Trim();

			//if the line is empty, do nothing with it...
			//ignore lines that close off data blocks...
			if (l == string.Empty //|| l.IndexOf("</") == 0
			) continue;

			//be default add data to the last datatype created
			int tagopen = l.IndexOf('[');
			int tagclose = l.IndexOf(']');
			if (tagclose > 1 && tagopen == 0) {
				if (null != Last) {
					string keyname = l.Substring(1, tagclose - 1).Trim();
					l = l.Substring(tagclose+1, l.Length - (tagclose + 1) ).Trim();
					if (null != Last) {
						Last.Set(keyname, l);
					}
				}
			} else {
			//check to see if this is the definition of a new datatype's constant fields
			//or whether this is new data for a particular datatype
				tagopen = l.IndexOf('<');
				tagclose = l.IndexOf('>');
				if (tagclose > 1 && tagopen == 0) {
					string key_name = l.Substring(1, tagclose - 1).Trim();
					string key_value = l.Substring(tagclose+1, l.Length - (tagclose + 1) );					
					AddNode(key_name, key_value);
				} else {
					//else add it as raw data...
					if (null != Last)
						Last.AddToData(l);
				}
			}
		}

		Node = (Count > 0) ? First : null;
		return true;
	}

	virtual public void Clear() {
		foreach (JossData d in Elements)
			d.Dispose();
		Elements.Clear();
	}
	
	virtual public bool Join(Josscript other, JossCopyMode copy_mode = JossCopyMode.no_id) {
		if (null == other || other.Count == 0)
			return false;
			
		foreach(JossData d in other) {
			Elements.Add( d.Copy(copy_mode, Elements.Count.ToString()) );
		}
			
		return true;
	}
	
	virtual public bool AddNode(string data_type, string add_data = "") {
		if (data_type == string.Empty)
			return false;
			
		Elements.Add( new JossData( Elements.Count ) );
		Last.data_type = data_type;
		Last.ProcessCombinedFields(add_data);
		return true;
	}

	virtual public bool CopyNode(JossData existing) {
		JossData copy = existing.Copy();
		Elements.Add( new JossData( Elements.Count ) );
		Last.data_type = copy.data_type;
		foreach (var key in copy.defined)
			Last.defined.Add(key.Key, key.Value);
		foreach (string s in copy.data)
			Last.data.Add(s);
		return true;
	}
	
	virtual public bool PositionAtFirstNode() {
		if (Count > 0)
			Node = Elements[0];
		else
			return false;
		return true;
	}
	
	virtual public bool PositionAtLastNode() {
		if (Count > 0)
			Node = Elements[Count-1];
		else
			return false;
		return true;
	}
	
	virtual public bool PositionAtNextNode() {
		if (null == Node)
			return false;
			
		int index = Node.ID + 1;
		return PositionAtNode(index);
	}
	
	virtual public bool PositionAtPreviousNode() {
		if (null == Node)
			return false;
			
		int index = Node.ID - 1;
		return PositionAtNode(index);
	}
	
	//Josscript files are supposed to have sequential nodes but since the
	//data author can override this, do a test to see if the specified
	//node is the one requested or else go look for it.
	//Since a single file can have multiple sections and each section
	//could start it's index from 1, you can also provide the section's
	//node index as an offset from which to start searching
	virtual public bool PositionAtNode(int index, int offset = 0) {
		if (index + offset < 0 || index + offset >= Count)
			return false;

		if (Elements [ index + offset ].ID == index) {
			Node = Elements[index + offset];
			return true;
		}
		
		int found_index = FindNodeByID(index, offset);
		if (found_index < 0)
			return false;
		Node = Elements[ found_index ];
		return true;
	}
	
	virtual public bool PositionAtID(int id) {
		if (id < 0)
			return false;
		
		int index = FindNodeByID(id);
		if (index == -1)
			return false;

		Node = Elements[index];
		return true;
	}
	
	virtual public int FindNodeByID(int id, int offset = 0) {
		for (int i = offset; i < Count; i++) 
			if (Elements[i].ID == id) {
				//if the node was found, point to it
				return i;
			}
		return -1;
	}

	virtual public bool RemoveCurrentNode() {
		return RemoveNode( Node.ID );
	}
	
	virtual public bool RemoveNode(int index) {
		if ( Count <= index)
			return false;

		//position Node on the next node
		if (null != Node)
		if (Node.ID == index)
			if (index < Count - 1)
				Node = Elements[index + 1];
			else
				//if there isn't a "next" node, first check if there is a "previous" node
				//and go there if there is...
				if (Node.ID == 0)
					Node = null;
				else
					Node = Elements[index - 1];
					
		Elements.RemoveAt(index);
		for (int i = index; i < Elements.Count; i++)
			Elements[i].ID = i;
			
		return true;
	}
	
	virtual public bool InsertNode(string data_type, string add_data = "") {
		if (null != Node)
			return InsertNode(Node.ID, data_type, add_data);
		else
			return AddNode(data_type, add_data);
	}
	
	virtual public bool InsertNode(int index, string data_type, string add_data="") {
		if (data_type == string.Empty)
			return false;
			
		if (Count == 0)
			return AddNode(data_type, add_data);
			
		Elements.Insert(index, new JossData( index ) );
		Node = Elements[index];
		Node.data_type = data_type;
		Node.ProcessCombinedFields(add_data);
		for (int i = index + 1; i < Count; i++)
			Elements[i].ID = i;
			
		return true;
	}
	
	virtual public bool ContainsANodeOfType(string data_type) {
		foreach (JossData node in Elements)
			if (node.data_type == data_type)
				return true;
		return false;
	}
	
	virtual public JossData GetFirstNodeOfType(string data_type) {
		if ( Count == 0 )
			return null;
			
		for ( int i = 0; i < Count; i++)
			if (Elements[i].data_type == data_type)
				return Elements[i];
		return null;
	}
	
	virtual public int GetFirstNodeOfTypei(string data_type) {
		if ( Count == 0 )
			return -1;
			
		for ( int i = 0; i < Count; i++)
			if (Elements[i].data_type == data_type)
				return i;
		return -1;
	}
	
	virtual public JossData GetLastNodeOfType(string data_type) {
		if ( Count == 0 )
			return null;
			
		for ( int i = Count -1; i >= 0; i--)
			if (Elements[i].data_type == data_type)
				return Elements[i];
		return null;
	}
	
	virtual public int GetLastNodeOfTypei(string data_type) {
		if ( Count == 0 )
			return -1;
			
		for ( int i = Count -1; i >= 0; i--)
			if (Elements[i].data_type == data_type)
				return i;
		return -1;
	}
	
	virtual public bool PositionAtFirstNodeOfType(string data_type) {
		JossData result = GetFirstNodeOfType(data_type);
		if (null != result)
			PositionAtNode(result.ID);
			
		return (null != result);
	}
	
	virtual public List<JossData> Children(int index = -1) {
		
		//if a node is not specified, try to use the currently active Node
		if ( index == -1 ) {
			if ( null == Node )
				return null;
			else
				index = Node.ID;
		}
		
		//if an invalid node is selected, return nothing
		//also return nothing if the very last node was selected
		if ( Count <= index - 1)
			return null;

		//see what the data type is of the first child node...		
		string parent_data_type = Elements[index++].data_type;
		string data_type		= Elements[index].data_type;

		//if the child node is of the same type as the parent node
		//then consider the parent childless		
		if (data_type == parent_data_type)
			return null;

		//loop through all remaining databclocks and return each that matches
		//the data type of the first child node, stopping at the first datablock
		//that matches the original data type
		List<JossData> Result = new List<JossData>();
		Result.Add( Elements[ index ] );
		for (++index; index < Count; index++) {
			if ( Elements[ index ].data_type == data_type)
				Result.Add( Elements[ index] );
			else
				if ( Elements[ index ].data_type == parent_data_type)
					break;
		}
		return Result;
	}
	
	virtual public List<JossData> AllChildNodes(int index = -1) {
		
		//if a node is not specified, try to use the currently active Node
		if ( index == -1 ) {
			if ( null == Node )
				return null;
			else
				index = Node.ID;
		}
		
		//if an invalid node is selected, return nothing
		//also return nothing if the very last node was selected
		if ( Count <= index - 1)
			return null;

		//see what the data type is of the parent node...		
		string parent_data_type = Elements[index].data_type;

		//loop through all remaining databclocks and return each that matches
		//the data type of the first child node, stopping at the first datablock
		//that matches the original data type
		List<JossData> Result = new List<JossData>();
		for (++index; index < Count; index++) {
			if ( Elements[ index ].data_type == parent_data_type)
				break;

			Result.Add( Elements[ index] );
		}
		if (Result.Count > 0)
			return Result;
			
		return null;
	}
	
	virtual public List<int> Childreni(int index = -1) {
		
		//if a node is not specified, try to use the currently active Node
		if ( index == -1 ) {
			if ( null == Node )
				return null;
			else
				index = Node.ID;
		}
		
		//if an invalid node is selected, return nothing
		//also return nothing if the very last node was selected
		if ( Count <= index - 1)
			return null;

		//see what the data type is of the first child node...		
		string parent_data_type = Elements[index++].data_type;
		string data_type		= Elements[index].data_type;

		//if the child node is of the same type as the parent node
		//then consider the parent childless		
		if (data_type == parent_data_type)
			return null;

		//loop through all remaining databclocks and return each that matches
		//the data type of the first child node, stopping at the first datablock
		//that matches the original data type
		List<int> Result = new List<int>();
		Result.Add( index  );
		for (++index; index < Count; index++) {
			if ( Elements[ index ].data_type == data_type)
				Result.Add( index );
			else
				if ( Elements[ index ].data_type == parent_data_type)
					break;
		}
		return Result;
	}
	
	virtual public List<int> AllChildNodesi(int index = -1) {
		
		//if a node is not specified, try to use the currently active Node
		if ( index == -1 ) {
			if ( null == Node )
				return null;
			else
				index = Node.ID;
		}
		
		//if an invalid node is selected, return nothing
		//also return nothing if the very last node was selected
		if ( Count <= index - 1)
			return null;

		//see what the data type is of the parent node...		
		string parent_data_type = Elements[index].data_type;

		//loop through all remaining databclocks and return each that matches
		//the data type of the first child node, stopping at the first datablock
		//that matches the original data type
		List<int> Result = new List<int>();
		for (++index; index < Count; index++) {
			if ( Elements[ index ].data_type == parent_data_type)
				break;

			Result.Add( index );
		}
		if (Result.Count > 0)
			return Result;
			
		return null;
	}

	virtual public List<JossData> DataWithField(string field, string val = "") {
		List<JossData> Result = new List<JossData>();
		for (int index = 0; index < Count; index++) {
			foreach (var k in Elements[index].defined) 
				if (k.Key == field) {
						if (val == string.Empty)
							Result.Add( Elements[ index] );
						else
							if (k.Value == val)
								Result.Add( Elements [ index] );
					break;
				}
		}
		if (Result.Count > 0)
			return Result;
			
		return null;
	}
	
	virtual public List<JossData> AllDataOfType(string type_name, int starting_index = 0, string stop_at_data_type = "") {
		List<JossData> result = new List<JossData>();
		if (starting_index >= Count)
			return result;
			
		for (int i = starting_index; i < Count; i++) {
			if (stop_at_data_type != string.Empty && Elements[i].data_type == stop_at_data_type)
				break;
				
			if (Elements[i].data_type == type_name)
				result.Add(Elements[i]);
		}				
		return result;
	}
	
	virtual public List<int> AllDataOfTypei(string type_name, int starting_index = 0, string stop_at_data_type = "") {
		List<int> result = new List<int>();
		if (starting_index >= Count)
			return result;
			
		for (int i = starting_index; i < Count; i++) {
			if (stop_at_data_type != string.Empty && Elements[i].data_type == stop_at_data_type)
				break;
				
			if (Elements[i].data_type == type_name)
				result.Add(i);
		}
						
		return result;
	}
	
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if(!this.disposed)
		{
			this.disposed = true;
			if(disposing)
			{
				Clear();
				Elements = null;
			}
		}		
	}

	~Josscript() {
		Dispose(false);
	}

	//impliment the ability to use "foreach" on Josscript data types
	public IEnumerator GetEnumerator() {
		return new JossI(Elements);
	}
}
class JossI : IEnumerator {
	List<JossData> Elements;
	int i = -1;
	
	public JossI(List<JossData> data) {
		Elements = data;
	}

	public bool MoveNext() {
		if (++i < Elements.Count)
			return true;
		return false;
	}

	public void Reset() {
		i = -1;
	}

	public object Current {	get { return Elements[i];} }

}
