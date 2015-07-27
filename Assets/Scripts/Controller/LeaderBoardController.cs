using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Kender.uGUI;

public class LeaderBoardController : MonoBehaviour {
	
	private LeaderBoardController leaderBoardController;
	public GoogleAnalyticsV4 googleAnalytics;
	public GameObject itemListPrefab;
	Language lang;
	List<User> UserFriends;
	List<User> CurrentList;
	Button FilterWorldWideUsers;
	Button FilterAllFriends;
	Button FilterUserByCountry;
	const int GRID_COLUMN_COUNT = 3;
	const int LIST_COLUMN_COUNT = 1;
	int CurrentColumnView;
	
	RectTransform containerRectTransform;
	//RectTransform headerList;
	GameObject objHeaderList;
	int itemCount;
	
	void Awake () {
		leaderBoardController = this;
		GoogleMobileAdsController.Instance.ShowInterstitial();
		//GoogleMobileAdsController.Instance.HideBanner();
	}
	
	void OnEnable(){
		lang = new Language(MainController.AppCurrentLanguage, false);
		GoogleMobileAdsController.Instance.ShowBanner();
	}
	
	void Start(){
		googleAnalytics.LogScreen(new AppViewHitBuilder().SetScreenName("LeadersBoard Menu"));
		FindViewByID();
		SetSettingButtonsText();

		LoadFriends();
		UserFriends = Utils.GetUserFriends();
		Comparison<User> compByGames = new Comparison<User>(User.CompareUserByGamesWon);
		UserFriends.Sort(compByGames);
		CurrentList = UserFriends;
		CreateUsersScrollList(CurrentList, LIST_COLUMN_COUNT);

	}
	
	void LoadFriends(){
		//userDAO = new UserDAO();
		//User userData = userDAO.LoadUserFromLocalMemory();
	}
	
	void Update(){
		if (Input.GetKey(KeyCode.Escape)) { 
			MainController.SwitchScene(MainController.previousScene);
		}
	}
	
	void FindViewByID(){
		try{
			containerRectTransform = GameObject.Find("ScrollableUserListPanel").GetComponent<RectTransform>();

			//headerList = GameObject.Find("HeaderListPanel").GetComponent<RectTransform>();
			objHeaderList = GameObject.Find("HeaderListPanel");
			
			FilterAllFriends = GameObject.Find("UserFriendsFilter").GetComponent<Button>();
			FilterWorldWideUsers = GameObject.Find("WorldUsersFilter").GetComponent<Button>();
			FilterUserByCountry = GameObject.Find("CountryUsersFilter").GetComponent<Button>();

			
			//Filters toggles Friends/WorldWide/Country
			FilterAllFriends.onClick.AddListener(AllUserFilterStatusChanged);
			FilterWorldWideUsers.onClick.AddListener(WorldWideFilterStatusChanged);
			FilterUserByCountry.onClick.AddListener(CountryFilterStatusChanged);

		}
		catch (Exception ex)
		{
			Debug.Log("FriendsController:FindViewByID() " + ex.ToString());
		}
	}

	void AllUserFilterStatusChanged(){
		Comparison<User> compByGames = new Comparison<User>(User.CompareUserByGamesWon);
		CurrentList.Sort(compByGames);	
		CreateUsersScrollList(UserFriends, CurrentColumnView);
	}
	
	void WorldWideFilterStatusChanged(){

		if(Application.internetReachability != NetworkReachability.NotReachable){
			Comparison<User> compByGames = new Comparison<User>(User.CompareUserByGamesWon);
			CurrentList.Sort(compByGames);	
			CreateUsersScrollList(UserFriends, CurrentColumnView);
		}
		else{
			MessageHandler.ShowNoInternetConnection();
		}
	}

	void CountryFilterStatusChanged(){

		if(Application.internetReachability != NetworkReachability.NotReachable){
			if(GameApplication.Instance.Player1 != null){
				string playerCountry = GameApplication.Instance.Player1.country.Name;
				int playerCountryId = GameApplication.Instance.Player1.country.Id;
				Debug.Log("playerCountry  " + playerCountry);
				if(playerCountry != null && !playerCountry.Equals("") && playerCountryId != 0){
					CreateUsersScrollList(GetUsersByCountry(playerCountry), CurrentColumnView);
				}
				else{
					MessageHandler.ShowNoCountrySelected();
				}
			}
			else{
				MessageHandler.ShowNoCountrySelected();
			}
		}
		else{
			MessageHandler.ShowNoInternetConnection();
		}
	}

	public List<User> GetUsersByCountry(string country){
		List<User> usersByCountry = new List<User>();
		User user = new User();
		for(int i=0; i < UserFriends.Count; i++){
			user = UserFriends[i];
			Debug.Log("user.AvatarName: " + user.AvatarName + " Country: " + user.country.Name);
			if(user.country.Name.Contains(country)){
				//Debug.Log("ONLINE user: " + UserFriends[i].AvatarName);
				usersByCountry.Add(user);
			}
		}
		return usersByCountry;
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
	
	public void OpenSettings(){
		MainController.SwitchScene(MainController.SETTINGS_SCENE);
	}
	
	public void OpenGameBoard(){
		MainController.SwitchScene(MainController.GAMEBOARD_SCENE);
	}
	
	public void OpenRanking () {
		GooglePlayGameController.Instance.ShowLeaderboard ();
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

	void HideHeaderTexts(){
		Text[] headerText = objHeaderList.GetComponentsInChildren<Text>();
		for(int i=0; i<headerText.Length; i++){
			headerText[i].enabled = false;
		}
	}
	
	void ShowHeaderTexts(){
		Text[] headerText = objHeaderList.GetComponentsInChildren<Text>();
		for(int i=0; i < headerText.Length; i++){
			headerText[i].enabled = true;
		}
	}
	
	public void CreateUsersScrollList(List<User> userFriendsList, int columnCount)
	{

		RectTransform rowRectTransform;
		GameObject newItem;
		//Image UserIconConn;
		Button InviteButton;
		User user = new User();
		
		//Resets the view when changing to grid/list Mode
		RemoveElementsFromList();


		Debug.Log("userFriendsList Count: " + userFriendsList.Count);
		if(userFriendsList.Count>0)
		{

			CurrentColumnView = columnCount;
			CurrentList = userFriendsList;
			
			if(columnCount == GRID_COLUMN_COUNT){
				//rowRectTransform = itemGridPrefab.GetComponent<RectTransform>();
				rowRectTransform = null;
				HideHeaderTexts();
			}
			else{
				rowRectTransform = itemListPrefab.GetComponent<RectTransform>();
				ShowHeaderTexts();
			}

			//calculate the width and height of each child item.
			float width = containerRectTransform.rect.width / columnCount;
			float ratio = width / rowRectTransform.rect.width;
			float height = rowRectTransform.rect.height * ratio;
			//float realHeight = rowRectTransform.rect.height;
			int rowCount = 0;
			itemCount = userFriendsList.Count;

		

			if(itemCount > GRID_COLUMN_COUNT){
				rowCount = itemCount / columnCount;
			}
			else{
				rowCount = LIST_COLUMN_COUNT;
			}
			
			if (itemCount % rowCount > 0){
				rowCount++;
			}

			//prefab real height 66

			//adjust the height of the container so that it will just barely fit all its children
			//float scrollHeight = height * rowCount;// + headerList.rect.height;
			//float scrollHeight = realHeight * rowCount;//+ headerList.rect.height;
			containerRectTransform.sizeDelta = new Vector2(0, rowRectTransform.rect.height * rowCount);
			//containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
			//containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x,height);
			//containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);
			containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, 0);
			
			
			int j = 0;
			for (int i = 0; i < itemCount; i++)
			{
				//this is used instead of a double for loop because itemCount may not fit perfectly into the rows/columns
				if (i % columnCount == 0)
					j++;
				
				//create a new item, name it, and set the parent
				if(columnCount == GRID_COLUMN_COUNT){
					//newItem = Instantiate(itemGridPrefab) as GameObject;
					newItem = null;
					//UserIconConn = newItem.GetComponentsInChildren<Image>()[3]; //connection_icon
				}else{
					newItem = Instantiate(itemListPrefab) as GameObject;
					//UserIconConn = newItem.GetComponentsInChildren<Image>()[3]; //connection_icon
				}
				
				if(newItem != null){
					newItem.name = containerRectTransform.name + " item at (" + i + "," + j + ")";
					newItem.transform.SetParent(containerRectTransform.transform, false);
					
					//Asigns an array item to an instance of user
					user = userFriendsList[i];
					
					//Set prefab values
					Text[] LayoutTexts = newItem.GetComponentsInChildren<Text>();
					LayoutTexts[0].text = user.AvatarName;
					
					if(columnCount == GRID_COLUMN_COUNT){
						//Add any specific requirement for grid view
					}
					else{ //LIST_COLUMN_COUNT
						LayoutTexts[1].text = user.level.Name;
						LayoutTexts[2].text = user.game.Wins.ToString();
						LayoutTexts[3].text = user.game.Ties.ToString();
						LayoutTexts[4].text = user.game.Played.ToString();
						
					}
					InviteButton = newItem.GetComponentInChildren<Button>();
					InviteButton.onClick.AddListener(InviteUserToPlay);
					
					//move and size the new item
					RectTransform rectTransform = newItem.GetComponent<RectTransform>();
					
					float x = -containerRectTransform.rect.width / 2 + width * (i % columnCount);
					float y = containerRectTransform.rect.height / 2 - height * j;
					rectTransform.offsetMin = new Vector2(x, y);
					
					x = rectTransform.offsetMin.x + width;
					y = rectTransform.offsetMin.y + height;
					rectTransform.offsetMax = new Vector2(x, y);
				}
			}
		}
		else{
			//rowRectTransform = EmptyListPrefab.GetComponent<RectTransform>();
		}
	}
	
	
	void RemoveElementsFromList()
	{
		if(containerRectTransform.childCount>0){
			foreach(Transform children in containerRectTransform)
			{
				Destroy(children.gameObject);
			}
		}
	}
	
	private void InviteUserToPlay()
	{
		if(Application.internetReachability != NetworkReachability.NotReachable){
			MessageHandler.ShowFriendInvitation();
		}else{
			MessageHandler.ShowNoInternetConnection();
		}
	}
	

	void OnDestroy()
	{
		Debug.Log("LeaderBoardController:OnDestroy()");
		if(leaderBoardController != null)
		{
			leaderBoardController = null;
		}
	}
}
