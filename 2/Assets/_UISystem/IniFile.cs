using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class IniFile  
{
	// the full name of the file
	string fileFullName;

	// pair of name and value
	// eg. name = value ; comment
	class Pair
	{
		public string name;
		public string value;
		public string comment;

		public Pair(string n, string v, string c)
		{
			name = n;
			value = v;
			comment = c;
		}
	}

	// section
	class Section
	{
		public string name;
		public string comment;
		public Dictionary<string, Pair> pairs;

		public Section(string n, string c)
		{
			name = n;
			comment = c;
			pairs = new Dictionary<string, Pair>();
		}

	}
	Section current_section;

	// data
	Dictionary<string, Section> data;


	public IniFile()
	{
		current_section = null;
		data = new Dictionary<string, Section>();
	}

	// load a exist file, otherwise, return false
	public bool Load_File(string fileName)
	{
		// file NOT exist
		if (!System.IO.File.Exists(fileName))
			return false;

		fileFullName = fileName;

		StreamReader sr = new StreamReader(fileFullName);

		while (!sr.EndOfStream)
		{
			string line = sr.ReadLine();
			line = line.Trim();

			if (line.Length == 0)
				continue;

			if (Is_Load_Comment(line))
				continue;


			// if the first letter is "[", it is section 
			// eg. [MySection]
			if (Is_Load_A_Section(line))
			{
				Load_A_Section(line);
			}
			else
			{
				Load_A_Pair(line);
			}

		}

		// end load
		sr.Close();
		current_section = null;

		return true;
	}

	bool Is_Load_Comment( string rawLine)
	{
		if (rawLine[0] == ';')
		{
			return true;
		}
		return false;
	}

	bool Is_Load_A_Section(string rawLine)
	{
		if (rawLine[0] == '[')
		{
			return true;
		}
		return false;
	}

	void Load_A_Section(string rawLine)
	{
		// read the section name and comment
		int startSection = rawLine.IndexOf('[');
		int endSection = rawLine.LastIndexOf(']');
		string name = rawLine.Substring((startSection + 1), (endSection - startSection - 1));
		string comment = "";
		if (rawLine.Split(';').Length >= 2)
			comment = rawLine.Split(';')[1];

		name = name.Trim();
		comment = comment.Trim();

		current_section = new Section(name, comment);
		data.Add(name,current_section);
	}

	void Load_A_Pair(string rawLine)
	{
		// read the pair name, value and comment
		string name = rawLine.Split('=')[0];
		string value = rawLine.Split('=')[1].Split(';')[0];
		string comment = "";
		if (rawLine.Split('=')[1].Split(';').Length >= 2)
			comment = rawLine.Split('=')[1].Split(';')[1];

		name = name.Trim();
		value = value.Trim();
		comment = comment.Trim();

		Pair p = new Pair(name, value, comment);
		current_section.pairs.Add(name, p);
	}


	// is section exist
	public bool Is_Section(string section)
	{
		return data.ContainsKey(section);
	}

	// if section not exist, return false
	public bool Goto_Section(string section)
	{
		if (Is_Section(section))
		{
			current_section = data[section];
			return true;
		}

		return false;
	}

	// create section, if exist, update the comment
	public bool Create_Section(string section, string comment)
	{
		if (Is_Section(section))
		{
			current_section = data[section];
			current_section.comment = comment;
			return false;
		}

		current_section = new Section(section, comment);
		data.Add(section, current_section);
		
		return false;
	}

	public bool Is_Name(string name)
	{
		if (current_section != null)
		{
			return current_section.pairs.ContainsKey(name);
		}

		return false;
	}

	public string Get_Section_Comment()
	{
		if (current_section != null)
		{
			return current_section.comment;
		}
		
		return "";
	}
	
	// get comment
	public string Get_Comment(string name, string defaultValue = "")
	{
		if (current_section != null)
		{
			if (Is_Name(name))
			{
				return current_section.pairs[name].comment;
			}
		}
		
		return defaultValue;
	}

	// get string
	public string Get_String(string name, string defaultValue = "")
	{
		if (current_section != null)
		{
			if (Is_Name(name))
			{
				return current_section.pairs[name].value;
			}
		}

		return defaultValue;
	}

	// get int
	public int Get_Int(string name, int defaultValue = 0)
	{
		return int.Parse(Get_String(name, defaultValue + ""));
	}

	// get float
	public float Get_Float(string name, float defaultValue = 0f)
	{
		return float.Parse(Get_String(name, defaultValue + ""));
	}


	// set string or update
	public void Set_String(string name, string value, string comment = "")
	{
		if (current_section != null)
		{
			if (Is_Name(name))
			{
				current_section.pairs[name].value = value;
				if (comment != "")
					current_section.pairs[name].comment = comment;
			}
			else
			{
				current_section.pairs.Add(name, new Pair(name, value, comment));
			}
		}
	}

	// set int or update
	public void Set_Int(string name, int value, string comment = "")
	{
		Set_String(name, value + "", comment);
	}

	// set float or update
	public void Set_Float(string name, float value, string comment = "")
	{
		Set_String(name, value + "", comment);
	}

	public void Save()
	{
		SaveTo(fileFullName);
	}

	public void SaveTo(string fileName)
	{
		StreamWriter sw = new StreamWriter(fileName,false);

		foreach(string key in data.Keys)
		{
			Section s = data[key];
			sw.WriteLine("[" + s.name + "]" + ((s.comment == "") ? "" : (" ; " + s.comment)));
			foreach(string name in s.pairs.Keys)
			{
				Pair p = s.pairs[name];
				sw.WriteLine( p.name + " = " + p.value + ((p.comment == "") ? "" : (" ; " + p.comment)));
			}
		}
		sw.Close();
	}

	public string[] Get_All_Section()
	{
		List<string> ret = new List<string>();
		foreach(string key in data.Keys)
		{
			ret.Add(key);
		}
		return ret.ToArray();
	}

}
















