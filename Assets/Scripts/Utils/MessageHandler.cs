using UnityEngine;
using System.Collections;

public class MessageHandler {

	static Language lang;


	public static void ShowNoInternetConnection()
	{
		new MobileNativeMessage("Error", "Sorry, You are not connected no internet!");
	}

	public static void ShowWarning()
	{
		lang = new Language(MainController.AppCurrentLanguage, false);
		new MobileNativeMessage(lang.GetString("popup_warning_title"), lang.GetString("popup_warning_text"));
	}

	public static void ShowNoCountrySelected()
	{
		new MobileNativeMessage("Katoiz", "You have to select your country from the settings menu option");
	}

	public static void ShowFriendInvitation()
	{
		new MobileNativeMessage("Katoiz", "The invitation has been sent to your friend!");
	
	}

	public static void ShowNoHaveFriends()
	{
		new MobileNativeMessage("Katoiz", "You have no friends yet, don't forget to invite  them over!");
	}

	public static void ShowQuitGame()
	{
		MobileNativeDialog confirmDialog = new MobileNativeDialog("Katoiz", "Do you want to quit the game?");
		confirmDialog.OnComplete += OnSaveDialogCloseQuitGame;
	}

	private static void OnSaveDialogCloseQuitGame(MNDialogResult result) 
	{
		//parsing result
		switch(result) {
		case MNDialogResult.YES:
			Application.Quit ();
			//System.Diagnostics.Process.GetCurrentProcess().Kill();
			break;
		case MNDialogResult.NO:
			break;
		default:
			break;
		}
	}


	private void InviteUserToPlay()
	{
		MobileNativeDialog confirmDialog = new MobileNativeDialog("Katoiz", "Do you want to invite this friend to a new game?");
		confirmDialog.OnComplete += OnSaveDialogClose;
	}
	
	private void OnSaveDialogClose(MNDialogResult result) 
	{
		//parsing result
		switch(result) {
		case MNDialogResult.YES:
			break;
		case MNDialogResult.NO:
			break;
		default:
			break;
		}
	}

	private void OnSaveDialogClose1(MNDialogResult result) 
	{
		//parsing result
		switch(result) {
		case MNDialogResult.YES:
			new MobileNativeMessage("Saved", "your changes have been saved successfully");
			//SaveData();
			MainController.SwitchScene(MainController.previousScene);
			break;
		case MNDialogResult.NO:
			MainController.SwitchScene(MainController.previousScene);
			break;
		default:
			break;
		}
	}


	public static void ShowGameOver(string player)
	{
		new MobileNativeMessage("Katoiz", "Great! "+ player +" has won the game!");
	}

	public static void ShowTimeIsUp(string player)
	{
		new MobileNativeMessage("Katoiz", "Time is over! " + player + " has lost the game!");
	}
}

