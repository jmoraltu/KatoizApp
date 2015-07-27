using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Kender.uGUI;

public class FriendsController : MonoBehaviour {

	private FriendsController friendsController;
	public GoogleAnalyticsV4 googleAnalytics;
	public GameObject itemGridPrefab;
	public GameObject itemListPrefab;
	public GameObject EmptyListPrefab;
	Language lang;
	User user;
	UserDAO userDao;
	List<User> UserFriends;
	List<User> OnlineFriends;
	List<User> CurrentList;
	Button GridButton;
	Button ListButton;
	Toggle FilterOnlineFriends;
	Toggle FilterAllFriends;
	Toggle SortAlphapeticFilter;
	Toggle SortByFreqFilter;
	ComboBox FilterUserLevel;
	const int GRID_COLUMN_COUNT = 3;
	const int LIST_COLUMN_COUNT = 1;
	int CurrentColumnView;

	RectTransform containerRectTransform;
	//RectTransform headerList;
	GameObject objHeaderList;
	int itemCount;

	void Awake () 
	{
		friendsController = this;
		GoogleMobileAdsController.Instance.ShowInterstitial();

	}

	void OnEnable()
	{
		lang = new Language(MainController.AppCurrentLanguage, false);
		GoogleMobileAdsController.Instance.ShowBanner();
	}

	void OnDisable()
	{
		GoogleMobileAdsController.Instance.HideBanner();
	}

	void Start()
	{
		googleAnalytics.LogScreen(new AppViewHitBuilder().SetScreenName("Friends Menu"));
		FindViewByID();
		SetSettingButtonsText();
		if(Application.internetReachability != NetworkReachability.NotReachable){
			Debug.Log("Application.internetReachability1 " + Application.internetReachability);
			UserFriends = Utils.GetUserFriends();
			OnlineFriends = GetOnlineFriends();	
			SaveData(UserFriends);
		}
		else{
			UserFriends = LoadFriends();
			OnlineFriends = GetOnlineFriends();
			Debug.Log("Application.internetReachability2 " + Application.internetReachability);
		}


		CurrentList = UserFriends;
		CreateUsersScrollList(CurrentList, GRID_COLUMN_COUNT);
	}

	void LoadData()
	{
		Debug.Log("FriendsController:LoadingData().....");
		UserDAO userDAO = new UserDAO();
		User userData = userDAO.LoadUserFromLocalMemory();
		Debug.Log("userData.AvatarName: " + userData.AvatarName);
	}

	void SaveData(List<User> UserFriends)
	{
		user = new User();
		user.Friends = UserFriends;

		/*foreach(User userItem in UserFriends){
			Debug.Log("user.AvatarName: " + userItem.AvatarName);
		}*/

		userDao = new UserDAO();
		userDao.SaveUserFriendsOnLocalMemory(user);
	}

	List<User> LoadFriends()
	{
		userDao = new UserDAO();
		User userData = userDao.LoadUserFriendsFromLocalMemory();

		/*if(userData.Friends != null){
			foreach(User userloop in userData.Friends){
				Debug.Log("user.AvatarName: " +userloop.AvatarName);
			}
		}else{
			Debug.Log("userData.Friends: NUULLL" );
		}*/

		return userData.Friends;
	}

	void Update(){
		if (Input.GetKey(KeyCode.Escape)) { 
			MainController.SwitchScene(MainController.previousScene);
		}
	}

	void FindViewByID(){
		try{
			containerRectTransform = GameObject.Find("ScrollableUserListPanel").GetComponent<RectTransform>();
			GridButton = GameObject.Find("GridMode").GetComponent<Button>();
			ListButton = GameObject.Find("ListMode").GetComponent<Button>();

			objHeaderList = GameObject.Find("HeaderListPanel");


			FilterOnlineFriends = GameObject.Find("OnlyAvailableUsersFilter").GetComponent<Toggle>();
			FilterAllFriends = GameObject.Find("AllUsersFilter").GetComponent<Toggle>();
			SortAlphapeticFilter = GameObject.Find("SortAlphapeticFilter").GetComponent<Toggle>();
			SortByFreqFilter = GameObject.Find("SortByFreqFilter").GetComponent<Toggle>();
			FilterUserLevel = GameObject.Find("LevelComboBox").GetComponent<ComboBox>();
				
			GridButton.onClick.AddListener(delegate{CreateUsersScrollList(CurrentList, GRID_COLUMN_COUNT);});
			ListButton.onClick.AddListener(delegate{CreateUsersScrollList(CurrentList, LIST_COLUMN_COUNT);});
			FilterOnlineFriends.onValueChanged.AddListener(OnLineFilterStatusChange);
			FilterAllFriends.onValueChanged.AddListener(AllUserFilterStatusChange);
			SortAlphapeticFilter.onValueChanged.AddListener(AlphabeticSortHandler);
			SortByFreqFilter.onValueChanged.AddListener(FreqSortHandler);

			FilterUserLevel.OnItemSelected += OnLevelSelected;
		}
		catch (Exception ex)
		{
			Debug.Log("FriendsController:FindViewByID() " + ex.ToString());
		}
	}

	void OnLevelSelected(int item){

		switch(item)
		{
			case 1:
				CreateUsersScrollList(GetFriendsByLevel(MainController.EASY_LEVEL), CurrentColumnView);
				break;
			case 2:
			CreateUsersScrollList(GetFriendsByLevel(MainController.NORMAL_LEVEL), CurrentColumnView);
				break;
			case 3:
			CreateUsersScrollList(GetFriendsByLevel(MainController.HIGH_LEVEL), CurrentColumnView);
				break;
			case 4:
			CreateUsersScrollList(GetFriendsByLevel(MainController.HARD_LEVEL), CurrentColumnView);
				break;
			default:
				CreateUsersScrollList(Utils.GetUserFriends(), CurrentColumnView);
				break;
		}
		Debug.Log("Item has been selected: " + item);
	}

	void CheckItemSelected(int index){
		Debug.Log("selectedIndex: " + index);
	}

	void AlphabeticSortHandler(bool isActivated){
		if(isActivated){
			SortByFreqFilter.isOn = false;
			CurrentList.Sort();
		}else{
			Comparison<User> compByGames = new Comparison<User>(User.CompareUserByGamesPlayed);
			CurrentList.Sort(compByGames);
			SortByFreqFilter.isOn = true;
		}
		CreateUsersScrollList(CurrentList, CurrentColumnView);
	}

	void FreqSortHandler(bool isActivated){
		if(isActivated){
			SortAlphapeticFilter.isOn = false;
			Comparison<User> compByGames = new Comparison<User>(User.CompareUserByGamesPlayed);
			CurrentList.Sort(compByGames);
		}else{
			SortAlphapeticFilter.isOn = true;
			CurrentList.Sort();
		}
		CreateUsersScrollList(CurrentList, CurrentColumnView);
	}

	void OnLineFilterStatusChange(bool isActivated){
		if(isActivated){
			FilterAllFriends.isOn = false;
			CreateUsersScrollList(OnlineFriends, CurrentColumnView);
		}
		else{
			FilterAllFriends.isOn = true;
			CreateUsersScrollList(UserFriends, CurrentColumnView);
		}
	}

	void AllUserFilterStatusChange(bool isActivated){
		if(isActivated){
			FilterOnlineFriends.isOn = false;
			CreateUsersScrollList(UserFriends, CurrentColumnView);
		}
		else{
			FilterOnlineFriends.isOn = true;
			CreateUsersScrollList(OnlineFriends, CurrentColumnView);
		}
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
		//GooglePlayGameController.Instance.ShowLeaderboard ();
		MainController.SwitchScene(MainController.RANKING_SCENE);
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


	public List<User> GetOnlineFriends(){
		List<User> OnlineFriendsList = new List<User>();
		User user = new User();
		for(int i=0; i < UserFriends.Count; i++){
			user = UserFriends[i];
			//Debug.Log("user.AvatarName: " + user.AvatarName + " onLine: " + user.IsOnLine);
			if(user.IsOnLine){
				//Debug.Log("ONLINE user: " + user.AvatarName);
				OnlineFriendsList.Add(user);
			}
		}
		return OnlineFriendsList;
	}

	public List<User> GetFriendsByLevel(String Level){
		List<User> OnlineFriendsList = new List<User>();
		User user = new User();
		for(int i=0; i < UserFriends.Count; i++){
			user = UserFriends[i];
			//Debug.Log("user.AvatarName: " + user.AvatarName + " onLine: " + user.IsOnLine);
			if(user.level.Name.Equals(Level)){
				//Debug.Log("ONLINE user: " + UserFriends[i].AvatarName);
				OnlineFriendsList.Add(user);
			}
		}
		return OnlineFriendsList;
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
		Image UserIconConn;
		Button InviteButton;
		user = new User();

		//containerRectTransform.sizeDelta = new Vector2(0, 0);
		//containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, 0);
		//Resets the view when changing to grid/list Mode
		RemoveElementsFromList();
		
		if(userFriendsList != null && userFriendsList.Count>0)
		{

			CurrentColumnView = columnCount;
			CurrentList = userFriendsList;
			//Reset the view when changing to grid/list Mode

			if(columnCount == GRID_COLUMN_COUNT){
				GridButton.enabled = false;
				ListButton.enabled = true;
				rowRectTransform = itemGridPrefab.GetComponent<RectTransform>();
				HideHeaderTexts();
			}
			else{
				GridButton.enabled = true;
				ListButton.enabled = false;
				rowRectTransform = itemListPrefab.GetComponent<RectTransform>();
				ShowHeaderTexts();
			}
			
			//calculate the width and height of each child item.
			itemCount = userFriendsList.Count;
			float width = containerRectTransform.rect.width / columnCount;
			float ratio = width / rowRectTransform.rect.width;
			float height = rowRectTransform.rect.height * ratio;
			//float realHeight = rowRectTransform.rect.height;
			int rowCount;

			if(itemCount > GRID_COLUMN_COUNT){
				rowCount = itemCount / columnCount;
			}
			else{
				rowCount = LIST_COLUMN_COUNT;
			}

			if (itemCount % rowCount > 0){
				rowCount++;
			}
			
			//adjust the height of the container so that it will just barely fit all its children
			//Debug.Log("rowCount: " + rowCount);

			if(columnCount == GRID_COLUMN_COUNT){
				//Debug.Log("rowRectTransform.rect.height: " + rowRectTransform.rect.height);
				containerRectTransform.sizeDelta = new Vector2(0, rowRectTransform.rect.height);
			}
			else{
				//Debug.Log("rowRectTransform.rect.height: " + rowRectTransform.rect.height * rowCount);
				//Debug.Log("rect.height.ratio: " + height * rowCount);
				containerRectTransform.sizeDelta = new Vector2(0, rowRectTransform.rect.height * rowCount);
			}


			//containerRectTransform.sizeDelta = new Vector2(0, 0);
			//containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
			//containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x,height);
			//containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);
			//containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, -headerList.rect.height);
			containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, 0);

			int j = 0;
			for (int i = 0; i < itemCount; i++)
			{
				//this is used instead of a double for loop because itemCount may not fit perfectly into the rows/columns
				if (i % columnCount == 0)
					j++;

				//create a new item, name it, and set the parent
				if(columnCount == GRID_COLUMN_COUNT){
					newItem = Instantiate(itemGridPrefab) as GameObject;
					UserIconConn = newItem.GetComponentsInChildren<Image>()[3]; //connection_icon
				}else{
					newItem = Instantiate(itemListPrefab) as GameObject;
					UserIconConn = newItem.GetComponentsInChildren<Image>()[3]; //connection_icon
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

					Sprite newSprite;

					if(user.IsOnLine){
						newSprite =  Resources.Load<Sprite>("Icons/connection_on");
					}else{
						newSprite =  Resources.Load<Sprite>("Icons/connection_off");
					}

					UserIconConn.sprite = newSprite;
					
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
			MessageHandler.ShowNoHaveFriends();
		}
	}


	void RemoveElementsFromList()
	{
		if(containerRectTransform.childCount > 0){
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
		Debug.Log("FriendsController:OnDestroy()");
		if(friendsController != null)
		{
			friendsController = null;
		}
	}
}
