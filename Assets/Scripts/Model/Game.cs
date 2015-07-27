using UnityEngine;
using System;
using System.Collections;


[Serializable]
public class Game  {

	public Game(){}

	[SerializeField]
	private int _played;
	public int Played
	{
		get
		{
			return _played;
		}
		set
		{
			_played = value;
		}
	}
	
	[SerializeField]
	private int _wins;
	public int Wins
	{
		get
		{
			return _wins;
		}
		set
		{
			_wins = value;
		}
	}
	
	[SerializeField]
	private int _ties;
	public int Ties
	{
		get
		{
			return _ties;
		}
		set
		{
			_ties = value;
		}
	}

}
