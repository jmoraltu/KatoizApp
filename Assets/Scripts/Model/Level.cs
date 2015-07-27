using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Level {

	public Level(){}

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
	
	[SerializeField]
	private int _minMatchLevelId;
	public int MinMatchLevelId
	{
		get
		{
			return _minMatchLevelId;
		}
		set
		{
			_minMatchLevelId = value;
		}
	}


	[SerializeField]
	private string _minMatchLevelName;
	public string MinMatchLevelName
	{
		get
		{
			return _minMatchLevelName;
		}
		set
		{
			_minMatchLevelName = value;
		}
	}

}
