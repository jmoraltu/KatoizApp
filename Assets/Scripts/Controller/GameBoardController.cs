using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames.BasicApi.Multiplayer;

public class GameBoardController : MonoBehaviour {

	public GameObject piecePrefab;
	public GameObject tilePrefab;
	public GameObject winnerParticleSystem;

	public const int BOARD_SIZE = 36;
	public const int BOARD_SIDE = 8;

	/*
	public const float ROTATION_X = 21.3f;
	public const float ROTATION_Y = 128;
	public const float ROTATION_Z = 157;

	public const float ROTATION_X = 25.0f;
	public const float ROTATION_Y = 128.0f;
	public const float ROTATION_Z = 154.0f;

	public const float ROTATION_X = 22f;
	public const float ROTATION_Y = 128.0f;
	public const float ROTATION_Z = 154.5f;*/

	public const float ROTATION_X = 23.23f;
	public const float ROTATION_Y = 129.0f;
	public const float ROTATION_Z = 154.5f;

	public const int MAX_COLORS = 5;
	public const int PILE_WINNER_SIZE = 4;
	public int numPlayers = 2;
	public const float MAX_TIME = 600f ;

	List<List<Vector3>> positionXY = null;

	List<Material> allPieces = null;
	string activeBoardColor;
	GameObject colorSelector = null;

	GameBoard gameBoard = null;
	Tile tile;
	Piece piece;
	string currentPlayer;
	//List<User> players = null;
	List<string> availablePicesColor = null;
	List<Material> remainingPieces = null;
	List<Material> remainingRepetedPieces = null;
	Stack<Piece> PathPieces = null;

	//GameObject currentPlayerPanel = null;
	Text currentPlayerName = null;
	Text remainingTimePlayer = null;
	GameObject playersPanel = null;
	CanvasGroup gameButtonsP1;
	CanvasGroup gameButtonsP2;
	RectTransform playerPanelP1;
	RectTransform playerPanelP2;
	float timer;
	bool backPress;
	string formatTime;
	bool isGameOver;
	int movesOverEmptyTiles;

	Piece selectedPiece = null;
	Tile initialPieceTile;
	Piece initialPiece;
	Material currentMaterial;

	//public GoogleAnalyticsV3 googleAnalytics;
	private GameBoardController gameBoardController;


	void Awake(){
		Application.targetFrameRate = 25;
		gameBoardController = this;
		GoogleMobileAdsController.Instance.ShowInterstitial();
		GoogleMobileAdsController.Instance.HideBanner();
		timer = MAX_TIME;
		SetInitialGameSettings();

		//MultiplayerController.Instance.SignInAndStartMPGame();
		MultiplayerController.Instance.TrySilentSignIn();
	}


	void Start(){

		//string myParticipantId = MultiplayerController.Instance.GetMyParticipantId();
		//Debug.Log("myParticipantId: " + myParticipantId);

		gameBoard = new GameBoard();
		//Tiles
		positionXY = GenerateTilesPosition(new Vector3(-1f,-4.27f,-6.24f));
		GenerateRandomPieces();
		GenerateBoard();
	}

	
	void DoMultiplayerUpdate(){
		MultiplayerController.Instance.SendMyUpdate("testing SendMyUpdate");
	}



	void OnEnable(){

		//Debug.Log("OnEnable() .......");
		GoogleMobileAdsController.Instance.ShowBanner();
	}
	
	void Update()
	{
		if(timer > 0 && !backPress){
			SetPlayerRemainingTime();
		}else{
			if(!isGameOver && !backPress){
				isGameOver = true;
				MessageHandler.ShowTimeIsUp(currentPlayer);
			}
				
		}

		//Shows frames per second rate on console
		Utils.CalculateFrameRate(); 


		if (Input.GetKey(KeyCode.Escape)) { 
			backPress = true;
			MainController.SwitchScene(MainController.previousScene);
		}

	}

	void SetPlayerRemainingTime(){
		
		if(currentPlayer == "Player1")
			timer = GameApplication.Instance.Players[0].RemainingTime = GameApplication.Instance.Players[0].RemainingTime - Time.deltaTime;
		else if(currentPlayer == "Player2"){
			timer = GameApplication.Instance.Players[1].RemainingTime = GameApplication.Instance.Players[1].RemainingTime - Time.deltaTime;
		}
		
		int minutes = Mathf.FloorToInt(timer / 60f);
		int seconds = Mathf.FloorToInt(timer - minutes * 60);
		formatTime = string.Format("{0:0}:{1:00}", minutes, seconds);
		remainingTimePlayer.text = formatTime;
	}

	void SetInitialGameSettings(){

		GenerateColorBoard();
		UserDAO userDAO = new UserDAO();

		movesOverEmptyTiles = 0;
		isGameOver = false;
		backPress = false;

		PathPieces = new Stack<Piece>();

		//(1)//currentPlayerPanel = GameObject.Find("CurrentPlayerPanel");
		//(2)//remainingTimePlayer = currentPlayerPanel.transform.Find("RemainingTime").GetComponent<Text>();

		//Use this call instead two GameObject.Find (1) and (2) as possible.
		remainingTimePlayer = GameObject.Find("CurrentPlayerPanel/RemainingTime").GetComponent<Text>();

		remainingTimePlayer.text = formatTime;

		MovesBoardController.Instance.FindViewByID();
		MovesBoardController.Instance.HideAllMessages();

		GameApplication.Instance.Players = new List<User>();
		User player;

		playersPanel = GameObject.Find("PlayersPanel");

		for(int i = 1; i < numPlayers + 1; i++){
			player = new User();
			if(i==1){
				player = userDAO.LoadUserFromLocalMemory();
				GameObject Player1 = GameObject.Find("Player1");

				RectTransform Player1Info = Player1.transform.Find("PlayerInfo").GetComponent<RectTransform>();
				Text player1Text = Player1Info.transform.Find("TextName").GetComponent<Text>();

				if((player.AvatarName == null ||  player.AvatarName.Length<1)){
					player1Text.text = "Player1";
				}
				else{
					player1Text.text = player.AvatarName;
				}
				player.RemainingTime = timer;
				GameApplication.Instance.Players.Add(player);

				playerPanelP1 = playersPanel.transform.Find("Player1").GetComponent<RectTransform>();
				gameButtonsP1 = playerPanelP1.transform.Find("GameButtons").GetComponent<CanvasGroup>();
				gameButtonsP1.alpha = 1;
			}
			else{
				GameObject playerN = GameObject.Find("Player" + i);
				RectTransform PlayerInfo = playerN.transform.Find("PlayerInfo").GetComponent<RectTransform>();
				Text playerNText = PlayerInfo.transform.Find("TextName").GetComponent<Text>();
				player.AvatarName = "Player" + i;
				playerNText.text = player.AvatarName;
				player.RemainingTime = timer;
				GameApplication.Instance.Players.Add(player);

				if(i == 2){
					playerPanelP2 = playersPanel.transform.Find("Player" + i).GetComponent<RectTransform>();
					gameButtonsP2 = playerPanelP2.transform.Find("GameButtons").GetComponent<CanvasGroup>();
					gameButtonsP2.alpha = 0;
				}
			}
		}

		colorSelector = GameObject.Find("ColorSelector");
		
		GameApplication.Instance.AvailableColors = new List<string>();

		for(int i=0; i< MAX_COLORS ; i++)
		{
			Image selector = colorSelector.transform.GetChild(i).GetComponent<Image>();
			if(i!=0){
				selector.enabled = false;
			}

		}

		//currentPlayerName = currentPlayerPanel.transform.Find("CurrentPlayerName").GetComponent<Text>();
		currentPlayerName = GameObject.Find("CurrentPlayerPanel/CurrentPlayerName").GetComponent<Text>();

		if(currentPlayer == null){
			currentPlayer = "Player1";
			currentPlayerName.text = (GameApplication.Instance.Players[0].AvatarName == null || GameApplication.Instance.Players[0].AvatarName.Length < 1) ? currentPlayer : GameApplication.Instance.Players[0].AvatarName;
		}
 
	}


	
	private List<List<Vector3>> GenerateTilesPosition(Vector3 initPos)
	{
		List<Vector3> tilesPosition = new List<Vector3>();
		List<List<Vector3>> localpositionXY = new List<List<Vector3>>();
		float deltaX;
		float deltaY;
		float deltaZ;
		Vector3 nextPos = new Vector3();
		Vector3 currentPos = new Vector3();
		Vector3 prevPos = new Vector3();

		for(int i = 0; i < BOARD_SIDE; i++){
			tilesPosition = new List<Vector3>();

			for(int j = 0; j < BOARD_SIDE; j++)
			{
				if(i==0 && j==0){
					//Set initial pos(0,0) 
					tilesPosition.Add(initPos);
				}
				else{
					if(j==0)
					{
						//desplazamiento de la posición en el eje Y
						//For tile scale 1.4
						deltaX = -1;
						deltaY = 0.57f;
						deltaZ = 0.82f;

						if(i > 0){
							prevPos = localpositionXY[i-1][0];
							tilesPosition.Add(new Vector3(prevPos.x + deltaX, prevPos.y + deltaY, prevPos.z + deltaZ)); //pos(0,0)
						}else{
							currentPos = tilesPosition[j];
							nextPos = new Vector3(currentPos.x + deltaX, currentPos.y + deltaY, currentPos.z + deltaZ);
							tilesPosition.Add(nextPos);
						}
					}
					else{
						//desplazamiento de la posición en el eje X
						//For tile scale 1.4
						deltaX = 1;
						deltaY = 0.57f; 
						deltaZ = 0.82f;
				
						currentPos = tilesPosition[j-1];

						nextPos = new Vector3(currentPos.x + deltaX, currentPos.y + deltaY, currentPos.z + deltaZ);
						tilesPosition.Add(nextPos);
					}
				}
			}
			localpositionXY.Add(tilesPosition);
		}
		return localpositionXY;
	}



	private Tile AddPiecesToHomeTile(Tile currentTile, int i, int j, bool isPlayerHome){

		//Vector3 initialPos = new Vector3(0.0f, 0.0f, -0.3f);
		Vector3 initialPos = new Vector3(0.0f, -0.3f, 0.0f);
		Quaternion rotation = new Quaternion();
		currentTile.pieces = new List<Piece>();
		piece = ((GameObject)Instantiate(piecePrefab, initialPos, rotation)).GetComponent<Piece>();
		piece.id = "Piece_" + i + "_" + j;
		piece.gameObject.name = "Piece_" + i + "_" + j;
		piece.tile = currentTile;
		MeshRenderer mr = piece.GetComponent<MeshRenderer>();
	

		int n = UnityEngine.Random.Range(0,remainingRepetedPieces.Count);
		mr.material = remainingRepetedPieces[n];
		remainingRepetedPieces.RemoveAt(n);


		string[] properties = mr.material.name.Split('_');
		piece.size = properties[0];
		piece.color = properties[1];
		piece.symbol = properties[2];

		piece.PieceTouch += OnPieceTouch;
		currentTile.pieces.Add(piece);

		if(isPlayerHome)
			piece.isHome = true;
		else
			piece.isHome = false;
		piece.transform.SetParent(currentTile.transform, false);

		return currentTile;
	}
	
	void GenerateBoard()
	{
		Quaternion rotation = new Quaternion();
		int n = 0;
		RectTransform containerRectTransform = GameObject.Find("GameBoard").GetComponent<RectTransform>();
		rotation.eulerAngles = new Vector3(ROTATION_X, ROTATION_Y, ROTATION_Z);
		gameBoard.tiles = new List<Tile>();

		remainingPieces = new List<Material>(allPieces.GetRange(0,30));
		remainingRepetedPieces = new List<Material>(allPieces.GetRange(31,9));
		int posOnBoard = 1;
		for(int i = 1; i < positionXY.Count-1; i++)
		{
			for(int j = 1; j < positionXY[i].Count-1; j++)
			{
				tile = ((GameObject)Instantiate(tilePrefab, positionXY[i][j], rotation)).GetComponent<Tile>();
				tile.id = "Tile_" + i + "_" + j;
				tile.gameObject.name = "Tile_" + i + "_" + j;
				tile.posX = i;
				tile.posY = j;
				tile.tileHeight = 1;
				tile.posOnBoard = posOnBoard;
				tile.TileTouch += OnTileTouch;
				posOnBoard++;
				//Add Pieces to tile
				//AddPiecesToTile(tile, i, j, false);

				///////////////////// implementado en el método AddPiecesToTile()/////////

				//Vector3 initialPos = new Vector3(0.0f, 0.0f, -0.3f); //Piece prefab
				Vector3 initialPos = new Vector3(0.0f, -0.3f, 0.0f);
				piece = ((GameObject)Instantiate(piecePrefab, initialPos, Quaternion.identity)).GetComponent<Piece>();
				piece.id = "Piece_" + i + "_" + j;
				piece.gameObject.name = "Piece_" + i + "_" + j;
				MeshRenderer mr = piece.GetComponent<MeshRenderer>();

				if(remainingPieces.Count > 0){
					n = UnityEngine.Random.Range(0, remainingPieces.Count);
					mr.material = remainingPieces[n];
					remainingPieces.RemoveAt(n);
				}else{
					n = UnityEngine.Random.Range(0, remainingRepetedPieces.Count-4);
					mr.material = remainingRepetedPieces[n]; 
					remainingRepetedPieces.RemoveAt(n);
				}


				string[] properties = mr.material.name.Split('_');
				piece.size = properties[0];
				piece.color = properties[1];
				piece.symbol = properties[2];
				piece.tile = tile;
				piece.PieceTouch += OnPieceTouch;

				//Debug.Log("Piece color: "+ piece.color + " symbol: " + piece.symbol + " size: " + piece.size + "pos(x,y) " + "(" + piece.tile.posX + "," + piece.tile.posY + ")");

				tile.pieces.Add(piece);
				piece.transform.SetParent(tile.transform, false);
				////////////////////////////////////////////////////////////////////////

				tile.transform.SetParent(containerRectTransform.transform, false);
				gameBoard.tiles.Add(tile);

			}
		}

		//Make a for operator until player count
		GeneratePlayerHomeTile(1, containerRectTransform.transform, rotation);
		GeneratePlayerHomeTile(2, containerRectTransform.transform, rotation);
	}

	Tile getTileByPositionXY(int x, int y){
		foreach(Tile tile in gameBoard.tiles){
			if(tile.posX == x && tile.posY == y){
				return tile;
			}
		}
		return null;
	}

	void GenerateColorBoard(){

		availablePicesColor = new List<string>();
		//Color[] colors = {Color.red, Color.blue, Color.white, Color.green, Color.yellow};
		//Color[] colors = {new Color32(187,41,36,255), new Color32(13,84,143,255), new Color(220,220,220,255), new Color32(14,148,37,255), new Color32(233,215,72,255)};
		Color[] colors = {new Color32(221,34,34,255), new Color32(0,102,170,255), new Color(255,255,255,255), new Color32(102,170,51,255), new Color32(238,187,0,255)};
		List<Color> remainingColors = new List<Color>(colors);
		GameObject colorBoard = GameObject.Find("ColorBoard");

		GameApplication.Instance.AvailableColors = new List<string>();

		System.Random randNumber = new System.Random();
		for(int i=0; i< colors.Length ; i++)
		{
			Image currentColor = colorBoard.transform.GetChild(i).GetComponent<Image>();
			int n = randNumber.Next(0, remainingColors.Count);
			currentColor.color = remainingColors[n];
			string activeColor = remainingColors[n].ToHexStringRGB();
			availablePicesColor.Add(activeColor);
			remainingColors.RemoveAt(n);
		}

		activeBoardColor = availablePicesColor[0];

	}

	void GeneratePlayerHomeTile(int player, Transform parent, Quaternion rotation)
	{
		Tile tilePlayer = null;
		int posX = 0; 
		int posY = 0;
		int posOnBoard = 0;
		rotation.eulerAngles = new Vector3(ROTATION_X, ROTATION_Y, ROTATION_Z);

		if(player == 1){
			posX = 1; posY = 0; posOnBoard = 37;
		}

		if(player == 2){
			posX = 6; posY = 7; posOnBoard = 38;
		}

		if(player == 3){
			posX = 7; posY = 1; posOnBoard = 39;
		}

		if(player == 4){
			posX = 0; posY = 6; posOnBoard = 40;
		}

		tilePlayer = ((GameObject)Instantiate(tilePrefab, positionXY[posX][posY], rotation)).GetComponent<Tile>();
		tilePlayer.id = "Tile_" + posX + "_" + posY;
		tilePlayer.gameObject.name = "Tile_" + posX + "_" + posY;
		AddPiecesToHomeTile(tilePlayer, posX, posY, true);
		tilePlayer.posX = posX;
		tilePlayer.posY = posY;
		tilePlayer.tileHeight = 1;
		tilePlayer.posOnBoard = posOnBoard;
		tilePlayer.TileTouch += OnTileTouch;
		gameBoard.tiles.Add(tilePlayer);
		tilePlayer.transform.SetParent(parent, false);
	}


	private void OnTileTouch(Tile currentTile, string id){
		if(selectedPiece != null){
			Debug.Log("currentTile: " + currentTile.name);
			Debug.Log("selectedPiece: " + selectedPiece.name);
			if(IsValidMove(currentTile)){
				/***if level = hard **/
				//MakeMovement(selectedPiece, currentTile);

				/*** if level = easy **/
				MakeMovementPath(currentTile);

				movesOverEmptyTiles = movesOverEmptyTiles + 1;
				MeshRenderer mr = selectedPiece.GetComponent<MeshRenderer>();
				mr.material = currentMaterial;
			}
		}
	}

	private void OnPieceTouch(Piece currentPiece, string id)
	{
		//Debug.Log("PositionOnBoard: " + currentPiece.tile.posOnBoard);

		MeshRenderer mrCurrent = currentPiece.GetComponent<MeshRenderer>();
		string selectedPieceColor = mrCurrent.material.color.ToHexStringRGB();

		Debug.Log("currentPiece-> color: " + currentPiece.color + " size: " + currentPiece.size + " symbol: " + currentPiece.symbol);
		if(selectedPiece != null)
		{
			if(currentPiece.MeetsPreconditions(selectedPiece)){
				//Debug.Log("MeetsPreconditions...");
				if(IsValidMove(currentPiece)){
					/***if level = hard ****
					MakeMovement(selectedPiece, currentPiece.tile);
					MeshRenderer mr = selectedPiece.GetComponent<MeshRenderer>();
					mr.material = currentMaterial;
					*/

					//if level = easy
					MakeMovementPath(currentPiece.tile);


				}
			}
		}
		else
		{
			if(selectedPieceColor.Equals(activeBoardColor)){

				if(currentPiece.MeetsPreconditions(selectedPiece))
				{
					selectedPiece = currentPiece;
					initialPieceTile = currentPiece.tile;
					initialPiece = currentPiece;

					/* Set piece transparency*/
					selectedPiece.SetSelectionTransparency();
					MeshRenderer mr = selectedPiece.GetComponent<MeshRenderer>();
					currentMaterial = mr.material;

					/* Set selected color to piece*/
					/*MeshRenderer mr = selectedPiece.GetComponent<MeshRenderer>();
					currentMaterial = mr.material;
					mr.material = Resources.Load("Material/OriginalTheme/selected") as Material;*/
				}
			}
		}
	}


	private bool IsValidMove(Tile nextTile){

		MovesBoardController.Instance.HideAllMessages();

		if(movesOverEmptyTiles != 0){
			return false;
		}

		if(selectedPiece.tile.IsSurroundingPoint(nextTile)){

			if(nextTile.pieces.Count==0)
				MovesBoardController.Instance.ShowEmptyTileOK();
			else
				MovesBoardController.Instance.ShowEmptyTileError();


			if(nextTile.pieces.Count==0 && (!nextTile.IsStartPosition(initialPieceTile)))
				return true;
			else
				return false;
		}
		else
			return false;
	}


	private bool IsValidMove(Piece currentPiece){
	
		MovesBoardController.Instance.HideAllMessages();

		if(currentPiece.tile.posX == initialPieceTile.posX && currentPiece.tile.posY == initialPieceTile.posY)
			return false;

		if(movesOverEmptyTiles != 0){
			return false;
		}

		if(selectedPiece.tile.IsCrossPositionPoint(currentPiece.tile))
		{
			if(currentPiece.IsSameColor(selectedPiece) || currentPiece.IsSameSymbol(selectedPiece) || currentPiece.IsSameSize(selectedPiece))
			{
				if(currentPiece.IsSameColor(selectedPiece))
					MovesBoardController.Instance.ShowSameColorOK();
				if(currentPiece.IsSameSymbol(selectedPiece))
					MovesBoardController.Instance.ShowSameSymbolOK();
				if(currentPiece.IsSameSize(selectedPiece))
					MovesBoardController.Instance.ShowSameSizeOK();

				return true;
			}
			else{
				MovesBoardController.Instance.ShowSameSizeError();
				MovesBoardController.Instance.ShowSameColorError();
				MovesBoardController.Instance.ShowSameSymbolError();
				//Debug.Log("Invalid cross move: Pieces are NEITHER same color NOR same symbol NOR same size");
				return false;
			}
		}
		else if(selectedPiece.tile.IsCornerPositionPoint(currentPiece.tile))
		{
			if(currentPiece.IsSameColor(selectedPiece) && currentPiece.IsSameSymbol(selectedPiece))
			{
				MovesBoardController.Instance.ShowSameColorOK();
				MovesBoardController.Instance.ShowSameSymbolOK();
				Debug.Log("valid corner move: Pieces are same color and symbol");
				return true;
			}
			else if(currentPiece.IsSameColor(selectedPiece) && currentPiece.IsSameSize(selectedPiece))
			{ 
				MovesBoardController.Instance.ShowSameColorOK();
				MovesBoardController.Instance.ShowSameSizeOK();
				Debug.Log("valid corner move: Pieces are same color and size");
				return true;
			}
			else if(currentPiece.IsSameSymbol(selectedPiece) && currentPiece.IsSameSize(selectedPiece))
			{ 
				MovesBoardController.Instance.ShowSameSymbolOK();
				MovesBoardController.Instance.ShowSameSizeOK();
				Debug.Log("valid corner move: Pieces are same symbol and size");
				return true;
			}
			else{

				if(currentPiece.IsSameColor(selectedPiece))
					MovesBoardController.Instance.ShowSameColorOK();
				else
					MovesBoardController.Instance.ShowSameColorError();

				if(currentPiece.IsSameSize(selectedPiece))
					MovesBoardController.Instance.ShowSameSizeOK();
				else
					MovesBoardController.Instance.ShowSameSizeError();

				if(currentPiece.IsSameSymbol(selectedPiece))
					MovesBoardController.Instance.ShowSameSymbolOK();
				else
					MovesBoardController.Instance.ShowSameSymbolError();

				return false;
			}
		}
		else{
			MovesBoardController.Instance.ShowInvalidMovementError();
			Debug.Log("Invalid move: Neither corner NOR cross move");
			return false;
		}
	}


	public void ResetGameMovement(){
		if(initialPieceTile != null)
		{
			if(selectedPiece != null)
			{
				MeshRenderer mr = selectedPiece.GetComponent<MeshRenderer>();
				mr.material = currentMaterial;

				/**** level = easy ********/
				Debug.Log("PathPieces.Count: " + PathPieces.Count);
				if(PathPieces.Count > 0){
					RollBackMovePath(true);
				}
				else{
					selectedPiece = null;
				}
				initialPiece.UnSetSelectionTransparency();

				/**** level = hard ********/
				//MakeMovement(selectedPiece, initialPieceTile);
				//selectedPiece.UnSetSelectionTransparency();
				//selectedPiece = null;
				movesOverEmptyTiles = 0;

			}
			MovesBoardController.Instance.HideAllMessages();
		}
	}


		
	//Need to pass parameter selectedPiece, initialPiece, lastMovePosition, gameBoard, numPlayer if created on class GameBoard
	private bool IsTileHeightDifferenceOK(){

		//Both pieces(origin and target) are inside player zone
		if(selectedPiece.IsPlayerZone(numPlayers) && initialPiece.IsPlayerZone(numPlayers))
		{
			return (gameBoard.CheckTileHeightDiffInsidePlayerZone(initialPiece, selectedPiece, numPlayers));
		}
		//The target piece is inside player zone, but origin is outside
		else if(selectedPiece.IsPlayerZone(numPlayers)){
			return (gameBoard.CheckTileHeightDiffFromOutsidePlayerZone(selectedPiece, numPlayers));
		}
		//The origin piece is inside player zone, but target is out side
		else if(initialPiece.IsPlayerZone(numPlayers)){
			return (gameBoard.CheckTileHeightDiffLeavingPlayerZone(initialPiece, numPlayers));
		}
		else{
			return true;
		}
	}


	public void ConfirmGameMovement(){
	
		if(selectedPiece != null   && PathPieces.Count > 0){

			initialPiece.tile.tileHeight--;
			selectedPiece.tile.tileHeight++;


			if(selectedPiece.IsPlayerZone(numPlayers) || initialPiece.IsPlayerZone(numPlayers)){

				if(IsTileHeightDifferenceOK()){
					Debug.Log("TileHeighIsCorrect");

					selectedPiece = null;
					MovesBoardController.Instance.HideAllMessages();
					if(!IsWinnerMove()){
						if(PathPieces.Count > 0){
							movesOverEmptyTiles = 0;
							UpdateActiveColor();
							ChangeTurn();
							RollBackMovePath(false);
						}
					}
					else{
						//Create FireWorks Effect
						RollBackMovePath(false);
						Instantiate(winnerParticleSystem);
						MessageHandler.ShowGameOver(currentPlayer);
						Debug.Log ("You have won the game!!!");
					}

					initialPiece.UnSetSelectionTransparency();
				}
				else{
					Debug.Log("TileHeigh Is NOT Correct");
					initialPiece.tile.tileHeight++;
					selectedPiece.tile.tileHeight--;
					MovesBoardController.Instance.HideAllMessages();
					MovesBoardController.Instance.ShowTileHeighError();
				}
			}
			else{
				selectedPiece = null;
				MovesBoardController.Instance.HideAllMessages();
				if(!IsWinnerMove()){
					if(PathPieces.Count > 0){
						movesOverEmptyTiles = 0;
						UpdateActiveColor();
						ChangeTurn();
						RollBackMovePath(false);
					}
				}
				else{
					//Create FireWorks Effect
					Instantiate(winnerParticleSystem);
					MessageHandler.ShowGameOver(currentPlayer);
					Debug.Log ("You have won the game!!!");
				}
				initialPiece.UnSetSelectionTransparency();
			}
			
		}
	}
	
	private void RollBackMovePath(bool reset){

		Tile lastTile = PathPieces.Peek().tile;

		while (PathPieces.Count != 0)
		{
			PathPieces.Peek().tile.PopTopPiece();
			string pathPieceName = PathPieces.Pop().name;
			Destroy(GameObject.Find(pathPieceName));
		}
		if(!reset){
			MakeMovement(initialPiece, lastTile);
		}
	}

	private void ChangeTurn(){

		//Rotate camera
		//Transform mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();

		if(currentPlayer == "Player1")
		{
			currentPlayerName.text = "Player2";
			currentPlayer = "Player2";
			gameButtonsP2.alpha = 1;
			gameButtonsP1.alpha = 0;
		}
		else{
			currentPlayer = "Player1";
			currentPlayerName.text = (GameApplication.Instance.Players[0].AvatarName == null || GameApplication.Instance.Players[0].AvatarName.Length < 1) ? currentPlayer : GameApplication.Instance.Players[0].AvatarName;
			gameButtonsP1.alpha = 1;
			gameButtonsP2.alpha = 0;
		}
	}

	private void UpdateActiveColor()
	{
		int index = availablePicesColor.IndexOf(activeBoardColor);
	
		for(int i=0; i< MAX_COLORS ; i++)
		{
			Image selector = colorSelector.transform.GetChild(i).GetComponent<Image>();
			if(index < 4){
				activeBoardColor = availablePicesColor[index + 1];
				if(i != index + 1){
					selector.enabled = false;
				}
				else{
					selector.enabled = true;
				}
			}
			else{
				activeBoardColor = availablePicesColor[0];
				if(i != 0)
					selector.enabled = false;
				else
					selector.enabled = true;
			}
		}
	}
	
	private bool IsWinnerMove(){
		if(currentPlayer == "Player1")
		{
			Tile p1 = Tile.GetPlayerTile(gameBoard.tiles, 1);
			if(p1.pieces.Count == PILE_WINNER_SIZE){
				return true;
			}else{
				return false;
			}
		}
		else if(currentPlayer == "Player2"){
			Tile p2 = Tile.GetPlayerTile(gameBoard.tiles, 2);
			if(p2.pieces.Count == PILE_WINNER_SIZE)
				return true;
			else{
				return false;
			}
		}
		else{
			return false;
		}
	}

	private void MakeMovement(Piece movingPiece, Tile nextTile){
		if(movingPiece != null){

			Tile currentTile = movingPiece.tile;
			nextTile.pieces.Add(movingPiece);
			currentTile.pieces.Remove(movingPiece);

			//Vector3 initialPos = new Vector3(0.0f, 0.0f, SetDeltaZ(nextTile)); //Tile prefab
			Vector3 initialPos = new Vector3(0.0f, SetDeltaZ(nextTile), 0.0f); //TileUV

			movingPiece.transform.parent = null;
			//movingPiece.transform.localScale = new Vector3(1, 1, 0.3f); //Tile prefab
			movingPiece.transform.localScale = new Vector3(1.0f, 0.5f, 1.0f); //TileUV
			movingPiece.transform.position = initialPos;
			movingPiece.transform.rotation = Quaternion.identity;
			movingPiece.tile = nextTile;
			
			if(nextTile.IsHomePosition()){
				movingPiece.isHome = true;
			}else{
				movingPiece.isHome = false;
			}
			movingPiece.transform.SetParent(nextTile.transform, false);
		}
	}  


	private void MakeMovementPath(Tile nextTile){

		nextTile.pieces.Add(selectedPiece);

		//Vector3 initialPos = new Vector3(0.0f, 0.0f, SetDeltaZ(nextTile));
		Vector3 initialPos = new Vector3(0.0f, SetDeltaZ(nextTile), 0.0f);

		Piece piecePath = ((GameObject)Instantiate(piecePrefab, initialPos, Quaternion.identity)).GetComponent<Piece>();
		piecePath.id = selectedPiece.id;
		piecePath.name = initialPiece.name + "_" + PathPieces.Count;
		piecePath.color = selectedPiece.color;
		piecePath.symbol = selectedPiece.symbol;
		piecePath.size = selectedPiece.size;
		piecePath.transform.parent = null;

		//piecePath.transform.localScale = new Vector3(1, 1, 0.3f);  //Tile prefab
		piecePath.transform.localScale = new Vector3(1.0f, 0.5f, 1.0f); //TileUV

		piecePath.tile = nextTile;
		piecePath.transform.SetParent(nextTile.transform, false);

		MeshRenderer mr = piecePath.GetComponent<MeshRenderer>();
		mr.material = currentMaterial;

		PathPieces.Push(piecePath);
		selectedPiece = piecePath;
	}

	private float SetDeltaZ(Tile nextTile){
		float deltaZ = 0.0f;
		int pileSize = nextTile.pieces.Count;
		if(pileSize > 0){
			//deltaZ = pileSize * -0.5f;
			deltaZ = pileSize * -0.3f;
		}
		return deltaZ;
	}


	List<Material> GenerateRandomPieces(){
		allPieces = new List<Material>();

		allPieces.Add(Resources.Load("Material/OriginalTheme/little_red_square") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/little_red_triangle") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/little_red_circle") as Material);

		allPieces.Add(Resources.Load("Material/OriginalTheme/little_yellow_square") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/little_yellow_triangle") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/little_yellow_circle") as Material);

		allPieces.Add(Resources.Load("Material/OriginalTheme/little_blue_square") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/little_blue_triangle") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/little_blue_circle") as Material);

		allPieces.Add(Resources.Load("Material/OriginalTheme/little_green_square") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/little_green_triangle") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/little_green_circle") as Material);

		allPieces.Add(Resources.Load("Material/OriginalTheme/little_white_square") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/little_white_triangle") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/little_white_circle") as Material);

		//15 pieces

		allPieces.Add(Resources.Load("Material/OriginalTheme/big_red_square") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/big_red_triangle") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/big_red_circle") as Material);
		
		allPieces.Add(Resources.Load("Material/OriginalTheme/big_yellow_square") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/big_yellow_triangle") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/big_yellow_circle") as Material);
		
		allPieces.Add(Resources.Load("Material/OriginalTheme/big_blue_square") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/big_blue_triangle") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/big_blue_circle") as Material);
		
		allPieces.Add(Resources.Load("Material/OriginalTheme/big_green_square") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/big_green_triangle") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/big_green_circle") as Material);
		
		allPieces.Add(Resources.Load("Material/OriginalTheme/big_white_square") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/big_white_triangle") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/big_white_circle") as Material);

		//30 pieces

		//Repeted Pieces
		allPieces.Add(Resources.Load("Material/OriginalTheme/little_red_circle") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/little_yellow_triangle") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/little_blue_circle") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/little_green_square") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/little_white_square") as Material);

		allPieces.Add(Resources.Load("Material/OriginalTheme/big_red_square") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/big_yellow_circle") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/big_blue_triangle") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/big_green_circle") as Material);
		allPieces.Add(Resources.Load("Material/OriginalTheme/big_white_triangle") as Material);

		//40 pieces

		return allPieces;
	}


	public void OpenCornerMenu(){
		Animator animMenu = GameObject.Find("CircleMenu").GetComponent<Animator>();
		animMenu.Play("cornerMenu");
	}

	void OnDestroy(){
		piece.PieceTouch -= OnPieceTouch;
		if(gameBoardController != null)
		{
			gameBoardController = null;
		}
	}
}