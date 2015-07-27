using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RulesController : MonoBehaviour {

	private RulesController rulesController;
	public GoogleAnalyticsV4 googleAnalytics;
	Language lang;
	UserDAO userDAO;
	
	void Awake () {
		rulesController = this;
		GoogleMobileAdsController.Instance.ShowInterstitial();
		GoogleMobileAdsController.Instance.HideBanner();
	}
	
	void OnEnable(){
		lang = new Language(MainController.AppCurrentLanguage, false);
		GoogleMobileAdsController.Instance.ShowBanner();
	}
	
	void Start(){
		googleAnalytics.LogScreen(new AppViewHitBuilder().SetScreenName("Rules Menu"));
		//FindViewByID();
		SetSettingButtonsText();
		//FillCountriesComboBox();
		//InitTabStates();
	}

	void Update(){
		if (Input.GetKey(KeyCode.Escape)) { 
			MainController.SwitchScene(MainController.previousScene);
		}
	}


	public void OpenSettings(){
		MainController.SwitchScene(MainController.SETTINGS_SCENE);
	}
	
	public void OpenGameBoard(){
		MainController.SwitchScene(MainController.GAMEBOARD_SCENE);
	}
	
	public void OpenRanking () {
		MainController.SwitchScene(MainController.RANKING_SCENE);
		//GooglePlayGameController.Instance.ShowLeaderboard ();
	}
	
	public void OpenRules () {
		MainController.SwitchScene(MainController.RULES_SCENE);
	}
	
	public void OpenFriends () {
		MainController.SwitchScene(MainController.FRIENDS_SCENE);
	}
	
	public void OpenKatoizWC () {
		GooglePlayGameController.Instance.ShowAchivements ();
	}

	void SetSettingButtonsText(){
		
		Text SettingButtonText;
		
		SettingButtonText = GameObject.Find("ConfigTxt").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("game_settings");
		
		SettingButtonText = GameObject.Find("PlayTxt").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("play_game");
		
		SettingButtonText = GameObject.Find("RankingTxt").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("game_ranking");
		
		SettingButtonText = GameObject.Find("RulesTxt").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("game_rules");
		
		SettingButtonText = GameObject.Find("FriendsTxt").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("game_friends");
		
		SettingButtonText = GameObject.Find("KatoizWcTxt").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("katoiz_wc");
	}
	
	void OnDestroy(){
		GoogleMobileAdsController.Instance.HideBanner();
		Debug.Log("RulesController:OnDestroy()");
		if(rulesController != null)
		{
			rulesController = null;
		}
	}
}
