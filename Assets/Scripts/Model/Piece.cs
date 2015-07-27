using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Piece: MonoBehaviour{


	public delegate void OnPieceTouchDelegate(Piece piece, string name);
	public event OnPieceTouchDelegate PieceTouch;

	public Piece(){}

	public Piece(Piece piece){
		this.id = piece.id;
		this.name = piece.name;
		this.color = piece.color;
		this.size = piece.size;
		this.symbol = piece.symbol;
		this.tile = piece.tile;
		this.isHome = piece.isHome;
	}

	void OnMouseDown() {
		//MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
		//mr.material = Resources.Load("Material/selected") as Material;
		//Debug.Log("Piece.Tile.id " + this.tile.id);

		if (PieceTouch != null) 
			PieceTouch(this, id);
	}


	[SerializeField]
	private string _id;
	public string id
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
	private Vector3 _position;
	public Vector3 position
	{
		get
		{
			return _position;
		}
		set
		{
			_position = value;
		}
	}
	
	[SerializeField]
	private Quaternion _rotation;
	public Quaternion rotation
	{
		get
		{
			return _rotation;
		}
		set
		{
			_rotation = value;
		}
	}

	[SerializeField]
	private string _color;
	public string color
	{
		get
		{
			return _color;
		}
		set
		{
			_color = value;
		}
	}

	[SerializeField]
	private string _symbol;
	public string symbol
	{
		get
		{
			return _symbol;
		}
		set
		{
			_symbol = value;
		}
	}

	[SerializeField]
	private string _size;
	public string size
	{
		get
		{
			return _size;
		}
		set
		{
			_size = value;
		}
	}
	
	[SerializeField]
	private Tile _tile;
	public Tile tile
	{
		get
		{
			return _tile;
		}
		set
		{
			_tile = value;
		}
	}

	[SerializeField]
	private bool _isHome;
	public bool isHome
	{
		get
		{
			return _isHome;
		}
		set
		{
			_isHome = value;
		}
	}

	public bool IsOnTop(){
		Piece topPiece = this.tile.pieces[this.tile.pieces.Count - 1];
		return this.id.Equals(topPiece.id);
	}

	public bool IsSameColor(Piece piece){
		return this.color.Equals(piece.color);
	}

	public bool IsSameSymbol(Piece piece){
		 return this.symbol.Equals(piece.symbol);
	}

	public bool IsSameSize(Piece piece){
		return this.size.Equals(piece.size);
	}

	//preconditions
	//the current piece is on top of pile or is not at a home player position or is not an initial position
	public bool MeetsPreconditions(Piece piece)
	{
		if(piece != null)
		{
			//return (this.tile.id != piece.tile.id && this.IsOnTop());
			return (this.IsOnTop());
		}
		else
		{
			return (this.IsOnTop() && !this.isHome);
		}
	}

	public void SetSelectionTransparency(){
		MeshRenderer mr = this.GetComponent<MeshRenderer>();
		Color currentColor = mr.material.color;
		currentColor.a -= 0.3f;
		mr.material.color = currentColor;
	}

	public void UnSetSelectionTransparency(){
		MeshRenderer mr = this.GetComponent<MeshRenderer>();
		Color currentColor = mr.material.color;
		currentColor.a += 0.3f;
		mr.material.color = currentColor;
	}

	public bool IsPlayerZone(int numPlayers){
		
		//Player 1
		if(this.tile.posOnBoard == 1 || this.tile.posOnBoard == 2 || this.tile.posOnBoard == 7 || this.tile.posOnBoard == 37){
			return true;
		}
		
		else if(numPlayers == 2){
			//Player 2
			if(this.tile.posOnBoard == 30 || this.tile.posOnBoard == 35 || this.tile.posOnBoard == 36 || this.tile.posOnBoard == 38)
				return true;
			else
				return false;
		}
		
		else if(numPlayers == 3){
			//Player 2
			if(this.tile.posOnBoard == 30 || this.tile.posOnBoard == 35 || this.tile.posOnBoard == 36)
				return true;
			//Player 3
			else if(this.tile.posOnBoard == 5 || this.tile.posOnBoard == 6 || this.tile.posOnBoard == 12)
				return true;
			else
				return false;
		}
		
		else if(numPlayers == 4){
			//Player 2
			if(this.tile.posOnBoard == 30 || this.tile.posOnBoard == 35 || this.tile.posOnBoard == 36)
				return true;
			
			//Player 3
			if(this.tile.posOnBoard == 5 || this.tile.posOnBoard == 6 || this.tile.posOnBoard == 12)
				return true;
			
			//Player 4
			if(this.tile.posOnBoard == 25 || this.tile.posOnBoard == 30 || this.tile.posOnBoard == 31)
				return true;
			else
				return false;
		}
		else {
			return false;
		}
	}
}
