using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;


public class MenuController : MonoBehaviour {


	public GoogleAnalyticsV4 googleAnalytics;
	private static MenuController menuController;

	void OnEnable(){
		Debug.Log("OnEnable() .......");
		GoogleMobileAdsController.Instance.ShowBanner();
	}

	void Awake(){
		menuController = this;

		Debug.Log("Awake() .......");
		//menuController.RegisterUserDeviceOnServer ();
		menuController.SetMenuButtonsText();
	}

	void Start(){
		googleAnalytics.LogScreen(new AppViewHitBuilder().SetScreenName("Main Menu"));
		Debug.Log("OnStart()....");
	}

	void OnDestroy(){
		if(menuController != null)
		{
			menuController = null;
		}
	}

	void SetMenuButtonsText(){
		Text MenuButtonText;
		string currentLang;

		if(Language.CURRENT_LANG != string.Empty)
			currentLang = Language.CURRENT_LANG;
		else
			currentLang = Application.systemLanguage.ToString();

		var lang = new Language(currentLang, false);

		MenuButtonText = GameObject.Find("NewGameTxt").GetComponent<Text>();
		MenuButtonText.text = lang.GetString("new_game");
		MenuButtonText = GameObject.Find("InvitesTxt").GetComponent<Text>();
		MenuButtonText.text = lang.GetString("game_invites");
		MenuButtonText = GameObject.Find("RulesTxt").GetComponent<Text>();
		MenuButtonText.text = lang.GetString("game_rules");
		MenuButtonText = GameObject.Find("FriendsTxt").GetComponent<Text>();
		MenuButtonText.text = lang.GetString("game_friends");
		MenuButtonText = GameObject.Find("SettingsTxt").GetComponent<Text>();
		MenuButtonText.text = lang.GetString("game_settings");
	}

//	void RegisterUserDeviceOnServer(){
//		var device = new Device ();
//		string deviceId = device.DeviceId;
//		Debug.Log("deviceId: " + deviceId + " device Model: " + device.Model + " device UniqueIdentifier: " + device.UniqueIdentifier);
//		WebServiceAdapter wsa = new WebServiceAdapter ();
//		StartCoroutine (wsa.GetUserByKey(device.DeviceId, Application.systemLanguage.ToString()));
//	}
	
	public void OpenNewGame () {
		MainController.SwitchScene(MainController.GAMEBOARD_SCENE);
	}

	public void OpenInvites () {
		Debug.Log("GooglePlayGameController.Instance.GetUserName:" + GooglePlayGameController.Instance.GetUserName());
		GooglePlayGameController.Instance.ShowLeaderboard ();
	}

	public void OpenRules () {
		MainController.SwitchScene(MainController.RULES_SCENE);
		//GooglePlayGameController.Instance.ShowAchivements ();
	}

	public void OpenFriends() {
		MainController.SwitchScene(MainController.FRIENDS_SCENE);
	}

	public void OpenSettings () {
		MainController.SwitchScene(MainController.SETTINGS_SCENE);
	}

	void Update(){
		if (Input.GetKey(KeyCode.Escape)) { 
			MessageHandler.ShowQuitGame();
		}
	}

	private void OnSaveDialogClose(MNDialogResult result) {
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

	void OnApplicationPause () {
		//Application.Quit();
		//or
		//System.Diagnostics.Process.GetCurrentProcess().Kill();
	}
		
	void ExitGame () {
		GooglePlayGameController.SignOut();
		Application.Quit ();
	}
}
