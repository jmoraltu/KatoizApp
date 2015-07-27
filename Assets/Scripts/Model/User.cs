using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


[Serializable]
public class User : IComparable<User>{
	
	/*
	 *Class Fields
	private string _name;
	private string _surname;
	private string _avatarName;
	private string _gender;
	private string _mobilePhone;
	private string _country;
	private string _level;
	private string _minimunMatchLevel;
	private bool _allowMatchInvites;
	private bool _allowChat;
	private bool _hasPublicProfile;
	private bool _allowPromotionalOffers;
	private byte[] avatarImage;
	public Country country;
	public Level level;
	public Game game;
	*/

	public User(){}

	
	public User(string name, string surname, string avatarName, string mobilePhone, bool allowMatchInvites, bool allowChat, bool hasPublicProfile, int countryIndex, int levelIndex, int genderIndex, byte[] avatarImage, bool enableMusic)
	{
		this._name = name;
		this._surname = surname;
		this._avatarName = avatarName;
		this._mobilePhone = mobilePhone;
		this._allowMatchInvitations = allowMatchInvites;
		this._allowChat = allowChat;
		this._hasPublicProfile = hasPublicProfile;
		this._country.Id = countryIndex;
		this._level.MinMatchLevelId = levelIndex;
		this._gender = genderIndex;
		this._avatarImage = avatarImage;
		this._enableMusic = enableMusic;
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
	private string _surname;
	public string Surname
	{
		get
		{
			return _surname;
		}
		set
		{
			_surname = value;
		}
	}


	[SerializeField]
	private string _avatarName;
	public string AvatarName
	{
		get
		{
			return _avatarName;
		}
		set
		{
			_avatarName = value;
		}
	}

	[SerializeField]
	private byte[] _avatarImage;
	public byte[] AvatarImage
	{
		get
		{
			return _avatarImage;
		}
		set
		{
			_avatarImage = value;
		}
	}


	[SerializeField]
	private Level _level;
	public Level level
	{
		get
		{
			return _level;
		}
		set
		{
			_level = value;
		}
	}
	
	[SerializeField]
	private Country _country;
	public Country country
	{
		get
		{
			return _country;
		}
		set
		{
			_country = value;
		}
	}
	
	
	[SerializeField]
	private Game _game;
	public Game game
	{
		get
		{
			return _game;
		}
		set
		{
			_game = value;
		}
	}

	[SerializeField]
	private List<User> _frieds;
	public List<User> Friends
	{
		get
		{
			return _frieds;
		}
		set
		{
			_frieds = value;
		}
	}

	[SerializeField]
	private int _gender;
	public int Gender
	{
		get
		{
			return _gender;
		}
		set
		{
			_gender = value;
		}
	}
	
	[SerializeField]
	private string _mobilePhone;
	public string MobilePhone
	{
		get
		{
			return _mobilePhone;
		}
		set
		{
			_mobilePhone = value;
		}
	}
	


	[SerializeField]
	private bool _allowMatchInvitations;
	public bool AllowMatchInvitations
	{
		get
		{
			return _allowMatchInvitations;
		}
		set
		{
			_allowMatchInvitations = value;
		}
	}
	
	[SerializeField]
	private bool _allowChat;
	public bool AllowChat
	{
		get
		{
			return _allowChat;
		}
		set
		{
			_allowChat = value;
		}
	}
	
	[SerializeField]
	private bool _hasPublicProfile;
	public bool HasPublicProfile
	{
		get
		{
			return _hasPublicProfile;
		}
		set
		{
			_hasPublicProfile = value;
		}
	}

	[SerializeField]
	private bool _enableMusic;
	public bool EnableMusic
	{
		get
		{
			return _enableMusic;
		}
		set
		{
			_enableMusic = value;
		}
	}

	[SerializeField]
	private bool _allowPromotions;
	public bool AllowPromotions
	{
		get
		{
			return _allowPromotions;
		}
		set
		{
			_allowPromotions = value;
		}
	}

	[SerializeField]
	private bool _isOnLine;
	public bool IsOnLine
	{
		get
		{
			return _isOnLine;
		}
		set
		{
			_isOnLine = value;
		}
	}
	
	[SerializeField]
	private bool _allowPromotionalOffers;
	public bool AllowPromotionalOffers
	{
		get
		{
			return _allowPromotionalOffers;
		}
		set
		{
			_allowPromotionalOffers = value;
		}
	}


	[SerializeField]
	private float _remainingTime;
	public float RemainingTime
	{
		get
		{
			return _remainingTime;
		}
		set
		{
			_remainingTime = value;
		}
	}


	public int CompareTo(User user)
	{
		return this.AvatarName.CompareTo(user.AvatarName);
	}

	public static int CompareUserName(User u1, User u2)
	{
		return u1.Name.CompareTo(u2.Name);
	}

	public static int CompareUserByGamesPlayed(User u1, User u2)
	{
		return u2.game.Played.CompareTo(u1.game.Played);
	}

	public static int CompareUserByGamesWon(User u1, User u2)
	{
		return u2.game.Wins.CompareTo(u1.game.Wins);
	}
}
