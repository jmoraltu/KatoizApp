using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class GameApplication : Singleton<GameApplication> {


	private GameApplication(){}

	[SerializeField]
	private User _player1;
	public User Player1
	{
		get
		{
			return _player1;
		}
		set
		{
			_player1 = value;
		}
	}

	[SerializeField]
	private User _player2;
	public User Player2
	{
		get
		{
			return _player2;
		}
		set
		{
			_player2 = value;
		}
	}


	[SerializeField]
	private List<User> _players;
	public List<User> Players
	{
		get
		{
			return _players;
		}
		set
		{
			_players = value;
		}
	}

	[SerializeField]
	private bool _saveAvatar;
	public bool SaveAvatar
	{
		get
		{
			return _saveAvatar;
		}
		set
		{
			_saveAvatar = value;
		}
	}

	[SerializeField]
	private List<string> _availableColors;
	public List<string> AvailableColors
	{
		get
		{
			return _availableColors;
		}
		set
		{
			_availableColors = value;
		}
	}


	[SerializeField]
	private List<Tile> _boardTiles;
	public List<Tile> BoardTiles
	{
		get
		{
			return _boardTiles;
		}
		set
		{
			_boardTiles = value;
		}
	}


}
