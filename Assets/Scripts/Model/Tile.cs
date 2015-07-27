using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Tile : MonoBehaviour {


	//Contructor
	public Tile(){

	}

	public delegate void OnTileTouchDelegate(Tile tile, string name);
	public event OnTileTouchDelegate TileTouch;

	void OnMouseDown() {
		if (TileTouch != null) 
			TileTouch(this, id);
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
	private List<Piece> _pieces;
	public List<Piece> pieces
	{
		get
		{
			return _pieces;
		}
		set
		{
			_pieces = value;
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
	private Vector3 _rotation;
	public Vector3 rotation
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
	private int _posX;
	public int posX
	{
		get
		{
			return _posX;
		}
		set
		{
			_posX = value;
		}
	}

	[SerializeField]
	private int _posY;
	public int posY
	{
		get
		{
			return _posY;
		}
		set
		{
			_posY = value;
		}
	}

	[SerializeField]
	private int _posOnBoard;
	public int posOnBoard
	{
		get
		{
			return _posOnBoard;
		}
		set
		{
			_posOnBoard = value;
		}
	}


	[SerializeField]
	private int _tileHeight;
	public int tileHeight
	{
		get
		{
			return _tileHeight;
		}
		set
		{
			_tileHeight = value;
		}
	}
	
	public Piece Top(){
		Piece topPiece = null;
		int pileCount = this.pieces.Count;
		if(pileCount > 0){
			topPiece = this.pieces[pileCount - 1];
			return topPiece;
		}
		else{
			return topPiece;
		}
	}

	public void PopTopPiece(){
		this.pieces.RemoveAt(this.pieces.Count-1);
	}

	public bool IsHomePosition(){
		if(this.posX == 1 && this.posY == 0)
			return true;
			
		else if(this.posX == 6 && this.posY == 7)
			return true;
			
		else if(this.posX == 7 && this.posY == 1)
			return true;
			
		else if(this.posX == 0 && this.posY == 6)
			return true;
			
		else
			return false;
	}

	//public bool IsStartPosition
	public bool IsStartPosition(Tile initialPieceTile){
		if(this.posX == initialPieceTile.posX && this.posY == initialPieceTile.posY)
			return true;
		else
			return false;
		
		
	}

	public static Tile GetPlayerTile(List<Tile> tileList, int player){

		foreach(Tile mytile in tileList){
			if(player == 1){
				if(mytile.posX == 1 && mytile.posY == 0){
					//Debug.Log("mytile.name" + mytile.name);
					return mytile;
				}
			}else if(player == 2){
				if(mytile._posX == 6 && mytile._posY==7){
					//Debug.Log("mytile.name" + mytile.name);
					return mytile;
				}
			}
		}
		Debug.Log("mytile.name: " + tileList[0]);
		return null;
	}

	//Calculates if two tiles are surrounded each other
	public bool IsSurroundingPoint(Tile nextTile){
		
		//Move up, down, left, right
		if((this.posX + 1 == nextTile.posX || this.posX - 1 == nextTile.posX) && this.posY == nextTile.posY){
			//Debug.Log("condition 1 ok");
			return true;
		}
		else if((this.posY + 1 == nextTile.posY || this.posY - 1 == nextTile.posY) && this.posX == nextTile.posX){
			//Debug.Log("condition 2 ok");
			return true;
		}
		
		//Move to corners
		else if( (this.posX + 1 == nextTile.posX || this.posX - 1 == nextTile.posX) && this.posY + 1 == nextTile.posY){
			return true;
		}
		
		else if( (this.posX + 1 == nextTile.posX || this.posX - 1 == nextTile.posX) && this.posY - 1 == nextTile.posY){
			return true;
		}
		
		else{
			return false; 
		}
	}

	//Calculates if two tiles are in a cross position point to each other
	public bool IsCrossPositionPoint(Tile nextTile){
		
		//Move up, down, left, right
		if((this.posX + 1 == nextTile.posX || this.posX - 1 == nextTile.posX) && this.posY == nextTile.posY){
			//Debug.Log("condition 1 ok");
			return true;
		}
		else if((this.posY + 1 == nextTile.posY || this.posY - 1 == nextTile.posY) && this.posX == nextTile.posX){
			//Debug.Log("condition 2 ok");
			return true;
		}
		else{
			return false;
		}
	}

	//Calculates if two tiles are in a corner position point to each other
	public bool IsCornerPositionPoint(Tile nextTile){
		//Move to corners
		if( (this.posX + 1 == nextTile.posX || this.posX - 1 == nextTile.posX) && this.posY + 1 == nextTile.posY){
			return true;
		}
		
		else if( (this.posX + 1 == nextTile.posX || this.posX - 1 == nextTile.posX) && this.posY - 1 == nextTile.posY){
			return true;
		}
		else{
			return false; 
		}
	}



	public bool IsPlayerZone(int numPlayers){
		
		//Player 1
		if(this.posOnBoard == 1 || this.posOnBoard == 2 || this.posOnBoard == 7){
			return true;
		}
		
		else if(numPlayers == 2){
			//Player 2
			if(this.posOnBoard == 30 || this.posOnBoard == 35 || this.posOnBoard == 36)
				return true;
			else
				return false;
		}
		
		else if(numPlayers == 3){
			//Player 2
			if(this.posOnBoard == 30 || this.posOnBoard == 35 || this.posOnBoard == 36)
				return true;
			//Player 3
			else if(this.posOnBoard == 5 || this.posOnBoard == 6 || this.posOnBoard == 12)
				return true;
			else
				return false;
		}
		
		else if(numPlayers == 4){
			//Player 2
			if(this.posOnBoard == 30 || this.posOnBoard == 35 || this.posOnBoard == 36)
				return true;
			
			//Player 3
			if(this.posOnBoard == 5 || this.posOnBoard == 6 || this.posOnBoard == 12)
				return true;
			
			//Player 4
			if(this.posOnBoard == 25 || this.posOnBoard == 30 || this.posOnBoard == 31)
				return true;
			else
				return false;
		}
		else {
			return false;
		}
	}
}
