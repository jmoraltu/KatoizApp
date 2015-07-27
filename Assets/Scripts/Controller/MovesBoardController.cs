using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;


public class MovesBoardController : Singleton<MovesBoardController> {

	//public static MovesBoardController instance = null;
	private CanvasGroup emptyCanvas;
	private CanvasGroup sizeCanvas;
	private CanvasGroup colorCanvas;
	private CanvasGroup symbolCanvas;
	private Image iconEmpty;
	private Image iconSize;
	private Image iconColor;
	private Image iconSymbol;

	private static Sprite errorIcon;
	private static Sprite okIcon;

	private Text txtConflict;

	Transform emptyCell;


	private MovesBoardController(){}
	

	void Update(){
		if (Input.GetKey(KeyCode.Escape)) { 
			DontDestroyOnLoad(gameObject);
		}
	}

	public void FindViewByID(){
		emptyCell = GameObject.Find("MovesBoard/EmptyCell").GetComponent<RectTransform>();
		Transform sizeCell = GameObject.Find("MovesBoard/SizeCell").GetComponent<RectTransform>();
		Transform colorCell = GameObject.Find("MovesBoard/ColorCell").GetComponent<RectTransform>();
		Transform symbolCell = GameObject.Find("MovesBoard/SymbolCell").GetComponent<RectTransform>();



		//CanvasGroup
		emptyCanvas = emptyCell.GetComponent<CanvasGroup>();
		sizeCanvas = sizeCell.GetComponent<CanvasGroup>();
		colorCanvas = colorCell.GetComponent<CanvasGroup>();
		symbolCanvas = symbolCell.GetComponent<CanvasGroup>();

		//Texts
		txtConflict = emptyCell.transform.GetChild(1).GetComponent<Text>();

		//Icons
		iconEmpty = emptyCell.transform.GetChild(0).GetComponent<Image>();
		iconSize = sizeCell.transform.GetChild(0).GetComponent<Image>();
		iconColor = colorCell.transform.GetChild(0).GetComponent<Image>();
		iconSymbol = symbolCell.transform.GetChild(0).GetComponent<Image>();

		//Sprites
		okIcon = Resources.Load("Icons/ok_icon2", typeof(Sprite)) as Sprite;
		errorIcon = Resources.Load("Icons/error_icon", typeof(Sprite)) as Sprite;

	
		/*
		emptyCanvas.alpha = Convert.ToInt32(emptyFlag);
		*/
	}

	public void HideAllMessages(){
		//Debug.Log("MovesBoardController HideAllMessages");
		emptyCanvas.alpha = 0;
		sizeCanvas.alpha = 0;
		colorCanvas.alpha = 0;
		symbolCanvas.alpha = 0;
	}

	public void ShowTileHeighError(){
		//Text txtConflict = emptyCell.transform.GetChild(1).GetComponent<Text>();
		txtConflict.text = "Invalid Height";
		iconEmpty.sprite = errorIcon;
		emptyCanvas.alpha = 1;
	}


	public void ShowInvalidMovementError(){
		txtConflict.text = "Invalid Move";
		iconEmpty.sprite = errorIcon;
		emptyCanvas.alpha = 1;
	}

	public void ShowEmptyTileOK(){
		//Debug.Log("MovesBoardController ShowEmptyCellOK");
		txtConflict.text = "Empty space";
		iconEmpty.sprite = okIcon;
		emptyCanvas.alpha = 1;
	}

	public void ShowEmptyTileError(){
		//Debug.Log("MovesBoardController ShowEmptyCellError");
		iconEmpty.sprite = errorIcon;
		emptyCanvas.alpha = 1;
	}

	public void ShowSameSizeOK(){
		//Debug.Log("MovesBoardController ShowSameSizeOK");
		iconSize.sprite = okIcon;
		sizeCanvas.alpha = 1;
	}

	public void ShowSameSizeError(){
		//Debug.Log("MovesBoardController ShowSameSizeError");
		iconSize.sprite = errorIcon;
		sizeCanvas.alpha = 1;
	}

	public void ShowSameColorOK(){
		//Debug.Log("MovesBoardController ShowSameSizeOK");
		iconColor.sprite = okIcon;
		colorCanvas.alpha = 1;
	}
	
	public void ShowSameColorError(){
		//Debug.Log("MovesBoardController ShowSameSizeError");
		iconColor.sprite = errorIcon;
		colorCanvas.alpha = 1;
	}

	public void ShowSameSymbolOK(){
		//Debug.Log("MovesBoardController ShowSameSymbolOK");
		iconSymbol.sprite = okIcon;
		symbolCanvas.alpha = 1;
	}
	
	public void ShowSameSymbolError(){
		//Debug.Log("MovesBoardController ShowSameSymbolError");
		iconSymbol.sprite = errorIcon;
		symbolCanvas.alpha = 1;
	}

}
