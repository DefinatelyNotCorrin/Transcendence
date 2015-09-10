using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

	public class Card {

	[XmlAttribute("name")]
	public string name;
		
	[XmlAttribute("ID")]
	public int id;

	[XmlAttribute("image")]
	public string image;

	[XmlAttribute("alliance")]
	public string alliance;

	[XmlAttribute("type")]
	public string type;

	[XmlAttribute("cost")]
	public int cost;

	[XmlAttribute("attack")]
	public string attack;

	[XmlAttribute("health")]
	public string health;

	[XmlAttribute("defense")]
	public string defense;

	[XmlAttribute("ability")]
	public string ability;

	[XmlAttribute("range")]
	public string range;

	[XmlAttribute("target")]
	public string target;

	}


