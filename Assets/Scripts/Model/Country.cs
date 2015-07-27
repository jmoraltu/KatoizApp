using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class Country {


	public Country(){}

	[SerializeField]
	private int _id;
	public int Id
	{
		get
		{
			return _id;
		}
		set
		{
			_id = value;
		}
	}
	
	[SerializeField]
	private string _name;
	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
		}
	}
}
