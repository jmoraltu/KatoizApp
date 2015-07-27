using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class GameBoard {

	public GameBoard(){}

	private int diff1;
	private int diff2;
	private int diff3;
	private int diff4;
	private const int MAX_TILE_DIFF = 2;

	[SerializeField]
	private List<Tile> _tiles;
	public List<Tile> tiles
	{
		get
		{
			return _tiles;
		}
		set
		{
			_tiles = value;
		}
	}


	public bool CheckTileHeightDiffInsidePlayerZone(Piece initialPiece, Piece selectedPiece,int numPlayers){

		Debug.Log("initialPiece height: " + initialPiece.tile.tileHeight);
		Debug.Log("selectedPiece height: " + selectedPiece.tile.tileHeight);
		
		
		if(numPlayers <=2 ){
			if(selectedPiece.tile.posOnBoard == 7){
				if(initialPiece.tile.posOnBoard == 1) //--> this.tiles[0]
				{
					diff1 = Math.Abs(selectedPiece.tile.tileHeight - (initialPiece.tile.tileHeight));
					diff2 = Math.Abs((initialPiece.tile.tileHeight) - this.tiles[1].tileHeight);
					diff3 = Math.Abs((initialPiece.tile.tileHeight) - this.tiles[36].tileHeight);

					return (diff1 < 3 && diff2 < 3 && diff3 < 3);

				}else if(initialPiece.tile.posOnBoard == 2){
					
					diff1 = Math.Abs(selectedPiece.tile.tileHeight - this.tiles[0].tileHeight);
					diff2 = Math.Abs((initialPiece.tile.tileHeight) - this.tiles[0].tileHeight);
					return (diff1 < 3 && diff2 < 3);
				}
			}
			
			else if(selectedPiece.tile.posOnBoard == 2){
				if(initialPiece.tile.posOnBoard == 1) //--> this.tiles[0]
				{
					diff1 = Math.Abs(selectedPiece.tile.tileHeight - (initialPiece.tile.tileHeight));
					diff2 = Math.Abs((initialPiece.tile.tileHeight) - this.tiles[6].tileHeight);
					diff3 = Math.Abs((initialPiece.tile.tileHeight) - this.tiles[36].tileHeight);

					return (diff1 < 3 && diff2 < 3 && diff3 < 3);

				}else if(initialPiece.tile.posOnBoard == 7){
					diff1 = Math.Abs(selectedPiece.tile.tileHeight - this.tiles[0].tileHeight);
					diff2 = Math.Abs((initialPiece.tile.tileHeight) - this.tiles[0].tileHeight);
					return (diff1 < 3 && diff2 < 3);
				}
			}

			else if(selectedPiece.tile.posOnBoard == 1){
				if(initialPiece.tile.posOnBoard == 7){
					diff1 = Math.Abs(selectedPiece.tile.tileHeight - (initialPiece.tile.tileHeight));
					diff2 = Math.Abs((selectedPiece.tile.tileHeight) - this.tiles[1].tileHeight);
					diff3 = Math.Abs((selectedPiece.tile.tileHeight) - this.tiles[36].tileHeight);
					
					return (diff1 < 3 && diff2 < 3 && diff3 < 3);
				}
				else if(initialPiece.tile.posOnBoard == 2){
					diff1 = Math.Abs(selectedPiece.tile.tileHeight - (initialPiece.tile.tileHeight));
					diff2 = Math.Abs((selectedPiece.tile.tileHeight) - this.tiles[6].tileHeight);
					diff3 = Math.Abs((selectedPiece.tile.tileHeight) - this.tiles[36].tileHeight);
					
					return (diff1 < 3 && diff2 < 3 && diff3 < 3);
				}
			}

			else if(selectedPiece.tile.posOnBoard == 37){
				if(initialPiece.tile.posOnBoard == 1){
					
					diff1 = Math.Abs(selectedPiece.tile.tileHeight - (initialPiece.tile.tileHeight));
					diff2 = Math.Abs((initialPiece.tile.tileHeight) - this.tiles[1].tileHeight);
					diff3 = Math.Abs((initialPiece.tile.tileHeight) - this.tiles[6].tileHeight);

					return (diff1 < 3 && diff2 < 3 && diff3 < 3);
				}
				else if(initialPiece.tile.posOnBoard == 2){

					diff1 = Math.Abs(selectedPiece.tile.tileHeight - this.tiles[0].tileHeight);
					diff2 = Math.Abs((initialPiece.tile.tileHeight) - this.tiles[0].tileHeight);

					return (diff1 < 3 && diff2 < 3);
				}

				else if(initialPiece.tile.posOnBoard == 7){
					diff1 = Math.Abs(selectedPiece.tile.tileHeight - this.tiles[0].tileHeight);
					diff2 = Math.Abs((initialPiece.tile.tileHeight) - this.tiles[0].tileHeight);
					return (diff1 < 3 && diff2 < 3);
				}
			}
			

			else{
				return true;
			}
		}
		return true;
	}
	
	public bool CheckTileHeightDiffFromOutsidePlayerZone(Piece selectedPiece,int numPlayers){
		
		if(numPlayers <=2 ){
			if(selectedPiece.tile.posOnBoard == 2 || selectedPiece.tile.posOnBoard == 7)
			{
				diff1 = Math.Abs(selectedPiece.tile.tileHeight - (this.tiles[0].tileHeight));	
				return (diff1 < 3);
			}
			
			else if(selectedPiece.tile.posOnBoard == 1)
			{
				diff1 = Math.Abs(selectedPiece.tile.tileHeight - (this.tiles[1].tileHeight));
				diff2 = Math.Abs(selectedPiece.tile.tileHeight - this.tiles[6].tileHeight);
				diff3 = Math.Abs(selectedPiece.tile.tileHeight - this.tiles[36].tileHeight);
				return (diff1 < 3 && diff2 < 3 && diff3 < 3);
			}
			else if(selectedPiece.tile.posOnBoard == 37){
				diff1 = Math.Abs(selectedPiece.tile.tileHeight - (this.tiles[0].tileHeight));
				return (diff1 < 3);
			}
			else{
				return true;
			}
			
		}
		return true;
	}
	
	public bool CheckTileHeightDiffLeavingPlayerZone(Piece initialPiece,int numPlayers){
				
		if(numPlayers <=2 ){
			if(initialPiece.tile.posOnBoard == 2){
				diff1 = Math.Abs((initialPiece.tile.tileHeight) - this.tiles[0].tileHeight);
				return (diff1 < 3);
			}
			
			else if(initialPiece.tile.posOnBoard == 7){
				diff1 = Math.Abs((initialPiece.tile.tileHeight) - this.tiles[0].tileHeight);
				return (diff1 < 3);
			}
			
			else if(initialPiece.tile.posOnBoard == 1){
				diff1 = Math.Abs((initialPiece.tile.tileHeight) - this.tiles[1].tileHeight);
				diff2 = Math.Abs((initialPiece.tile.tileHeight) - this.tiles[6].tileHeight);
				diff3 = Math.Abs((initialPiece.tile.tileHeight) - this.tiles[36].tileHeight);
				return (diff1 < 3 && diff2 < 3 && diff3 < 3);
			}
			
		}
		return true;
	}


	/*
	public bool CheckTileHeightDiffInsidePlayerZone(Piece initialPiece, Piece selectedPiece, int prevMovePosition,int numPlayers){
		//Debug.Log("Inside CheckTileHeightDiffInsidePlayerZone");
		//Debug.Log("Tile 0 pieces: " + this.tiles[0].pieces.Count);
		//Debug.Log("prevMovePosition: " + prevMovePosition);
		Debug.Log("initialPiece height: " + initialPiece.tile.tileHeight);
		Debug.Log("selectedPiece height: " + selectedPiece.tile.tileHeight);


		if(numPlayers <=2 ){
			if(selectedPiece.tile.posOnBoard == 7){
				if(initialPiece.tile.posOnBoard == 1) //--> this.tiles[0]
				{
					diff1 = Math.Abs(selectedPiece.tile.pieces.Count - (initialPiece.tile.pieces.Count -1));
					diff2 = Math.Abs((initialPiece.tile.pieces.Count -1) - this.tiles[1].pieces.Count);
					diff3 = Math.Abs((initialPiece.tile.pieces.Count -1) - this.tiles[36].pieces.Count);

					if(initialPiece.tile.pieces.Count > selectedPiece.tile.pieces.Count){
						return (diff1<1 && diff2 < 3 && diff3 < 3);
					}else{
						return (diff1<3 && diff2 < 3 && diff3 < 3);
					}
				}else if(initialPiece.tile.posOnBoard == 2){

					diff1 = Math.Abs(selectedPiece.tile.pieces.Count - this.tiles[0].pieces.Count);
					diff2 = Math.Abs((initialPiece.tile.pieces.Count -1) - this.tiles[0].pieces.Count);
					return (diff1 < 3 && diff2 < 3);
				}
			}

			else if(selectedPiece.tile.posOnBoard == 2){
				if(initialPiece.tile.posOnBoard == 1) //--> this.tiles[0]
				{
					diff1 = Math.Abs(selectedPiece.tile.pieces.Count - (initialPiece.tile.pieces.Count -1));
					diff2 = Math.Abs((initialPiece.tile.pieces.Count -1) - this.tiles[6].pieces.Count);
					diff3 = Math.Abs((initialPiece.tile.pieces.Count -1) - this.tiles[36].pieces.Count);
					
					if(initialPiece.tile.pieces.Count > selectedPiece.tile.pieces.Count){
						return (diff1 < 1 && diff2 < 3 && diff3 < 3);
					}else{
						return (diff1 < 3 && diff2 < 3 && diff3 < 3);
					}
				}else if(initialPiece.tile.posOnBoard == 7){
					diff1 = Math.Abs(selectedPiece.tile.pieces.Count - this.tiles[0].pieces.Count);
					diff2 = Math.Abs((initialPiece.tile.pieces.Count -1) - this.tiles[0].pieces.Count);
					return (diff1 < 3 && diff2 < 3);
				}
			}

			else if(selectedPiece.tile.posOnBoard == 37){
				if(initialPiece.tile.posOnBoard == 1){

					diff1 = Math.Abs(selectedPiece.tile.pieces.Count - (initialPiece.tile.pieces.Count -1));
					diff2 = Math.Abs((initialPiece.tile.pieces.Count-1) - this.tiles[1].pieces.Count);
					diff3 = Math.Abs((initialPiece.tile.pieces.Count-1) - this.tiles[6].pieces.Count);



					if(initialPiece.tile.pieces.Count > selectedPiece.tile.pieces.Count){
						return (diff1 < 1 && diff2 < 3 && diff3 < 3);
					}else{
						return (diff1 < 3 && diff2 < 3 && diff3 < 3);
					}
				}
				else if(initialPiece.tile.posOnBoard == 7){
					diff1 = Math.Abs(selectedPiece.tile.pieces.Count - this.tiles[0].pieces.Count);
					diff2 = Math.Abs((initialPiece.tile.pieces.Count - 1) - this.tiles[0].pieces.Count);
					return (diff1 < 3 && diff2 < 3);
				}
			}

			else if(selectedPiece.tile.posOnBoard == 1){
				if(initialPiece.tile.posOnBoard == 7){
					diff1 = Math.Abs(selectedPiece.tile.pieces.Count - (initialPiece.tile.pieces.Count -1));
					diff2 = Math.Abs((initialPiece.tile.pieces.Count -1) - this.tiles[1].pieces.Count);
					diff3 = Math.Abs((initialPiece.tile.pieces.Count -1) - this.tiles[36].pieces.Count);
					
					if(initialPiece.tile.pieces.Count > selectedPiece.tile.pieces.Count){
						return (diff1 < 1 && diff2 < 3 && diff3 < 3);
					}else{
						return (diff1 < 3 && diff2 < 3 && diff3 < 3);
					}
				}
				else if(initialPiece.tile.posOnBoard == 2){
					diff1 = Math.Abs(selectedPiece.tile.pieces.Count - (initialPiece.tile.pieces.Count -1));
					diff2 = Math.Abs((initialPiece.tile.pieces.Count -1) - this.tiles[6].pieces.Count);
					diff3 = Math.Abs((initialPiece.tile.pieces.Count -1) - this.tiles[36].pieces.Count);

					if(initialPiece.tile.pieces.Count > selectedPiece.tile.pieces.Count){
						return (diff1 < 1 && diff2 < 3 && diff3 < 3);
					}else{
						return (diff1 < 3 && diff2 < 3 && diff3 < 3);
					}
				}
			}
			else{
				return true;
			}
		}
		return true;
	}

	public bool CheckTileHeightDiffFromOutsidePlayerZone(Piece selectedPiece, int prevMovePosition,int numPlayers){
		//Debug.Log("Inside CheckTileHeightDiffFromOutsidePlayerZone");
		//Debug.Log("Tile 0 pieces: " + this.tiles[0].pieces.Count);
		//Debug.Log("prevMovePosition: " + prevMovePosition);
		Debug.Log("selectedPiece height: " + selectedPiece.tile.tileHeight);

		if(numPlayers <=2 ){

			if(selectedPiece.tile.posOnBoard == 2 || selectedPiece.tile.posOnBoard == 7)
			{
				if(prevMovePosition == 1){
					diff1 = Math.Abs(selectedPiece.tile.pieces.Count - (this.tiles[0].pieces.Count -1));

					if(selectedPiece.tile.pieces.Count > (this.tiles[0].pieces.Count - 1))
						return (diff1 < 3);
					else
						return (diff1 < 2);
				}
				else{
					diff1 = Math.Abs(selectedPiece.tile.pieces.Count - this.tiles[0].pieces.Count);
					return (diff1 < 3);
				}
			}
		
			else if(selectedPiece.tile.posOnBoard == 1){
				if(prevMovePosition == 2){
					diff1 = Math.Abs(selectedPiece.tile.pieces.Count - (this.tiles[1].pieces.Count -1));
					diff2 = Math.Abs(selectedPiece.tile.pieces.Count - this.tiles[6].pieces.Count);
					diff3 = Math.Abs(selectedPiece.tile.pieces.Count - this.tiles[36].pieces.Count);
					
					if(selectedPiece.tile.pieces.Count > (this.tiles[1].pieces.Count - 1))
						return (diff1 < 3 && diff2 < 3 && diff3 < 3);
					else
						return (diff1 < 2 && diff2 < 3 && diff3 < 3);

				}else if(prevMovePosition == 7){
					diff1 = Math.Abs(selectedPiece.tile.pieces.Count - (this.tiles[6].pieces.Count -1));
					diff2 = Math.Abs(selectedPiece.tile.pieces.Count - this.tiles[1].pieces.Count);
					diff3 = Math.Abs(selectedPiece.tile.pieces.Count - this.tiles[36].pieces.Count);

					if(selectedPiece.tile.pieces.Count > (this.tiles[6].pieces.Count - 1))
						return (diff1 < 3 && diff2 < 3 && diff3 < 3);
					else
						return (diff1 < 2 && diff2 < 3 && diff3 < 3);
				}
				else{//Moves from outer 
					diff1 = Math.Abs(selectedPiece.tile.pieces.Count - this.tiles[1].pieces.Count);
					diff2 = Math.Abs(selectedPiece.tile.pieces.Count - this.tiles[6].pieces.Count);
					diff3 = Math.Abs(selectedPiece.tile.pieces.Count - this.tiles[36].pieces.Count);
					return (diff1 < 3 && diff2 < 3 && diff3 < 3);
				}
			}
			else{
				return true;
			}
	
		}
		return true;
	}

	public bool CheckTileHeightDiffLeavingPlayerZone(Piece initialPiece, int prevMovePosition,int numPlayers){
		//Debug.Log("Inside CheckTileHeightDiffLeavingPlayerZone");
		//Debug.Log("Tile 0 pieces: " + this.tiles[0].pieces.Count);
		//Debug.Log("prevMovePosition: " + prevMovePosition);
		Debug.Log("initialPiece height: " + initialPiece.tile.tileHeight);

		if(numPlayers <=2 ){
			if(initialPiece.tile.posOnBoard == 2){

				if(prevMovePosition == 1){

				}

				else{
					diff1 = Math.Abs((initialPiece.tile.pieces.Count -1) - this.tiles[0].pieces.Count);
					return (diff1 < 3);
				}
			}

			else if(initialPiece.tile.posOnBoard == 7){

				if(prevMovePosition == 1){
					
				}
				else{
					diff1 = Math.Abs((initialPiece.tile.pieces.Count -1) - this.tiles[1].pieces.Count);
					return (diff1 < 3);
				}
			}

			else if(initialPiece.tile.posOnBoard == 1){
				
			}
			else{
				diff1 = Math.Abs((initialPiece.tile.pieces.Count -1) - this.tiles[1].pieces.Count);
				return (diff1 < 3);
			}

		}
		return true;
	}
*/

}
