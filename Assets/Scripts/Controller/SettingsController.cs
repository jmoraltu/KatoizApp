using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Kender.uGUI;
using ImageVideoContactPicker;


public class SettingsController : MonoBehaviour {
	
	Text UserTabText;
	Text ThemeTabText;
	Image UserTabImg;
	Image ThemeTabImg;
	Image AvatarImg;
	string FilePath;
	InputField AvatarNameInput;
	InputField UserNameInput;
	InputField UserSurenameInput;
	InputField UserPhoneInput;
	Toggle AllowMatchInvitations;
	Toggle AllowChatToggle;
	Toggle PublicProfileToggle;
	Toggle MusicToggle;
	Toggle AllowPromotions;
	ComboBox CountryComboBox;
	ComboBox LevelComboBox;
	ComboBox GenderComboBox;
	byte[] avatarImage;
	string currentLang;
	Language lang;

	//bool isDialogShown;
	
	UserDAO userDAO;
	
	public GoogleAnalyticsV4 googleAnalytics;
	private SettingsController settingsController;
	
	// Use this for initialization
	void Awake () 
	{
		settingsController = this;
		GoogleMobileAdsController.Instance.ShowInterstitial();
		GoogleMobileAdsController.Instance.HideBanner();
	}

	void OnEnable()
	{
		lang = new Language(MainController.AppCurrentLanguage, false);
		MainController.SetAppLanguage();
		PickerEventListener.onImageSelect += OnImageSelect;
		PickerEventListener.onImageLoad += OnImageLoad;
		PickerEventListener.onError += OnError;
		PickerEventListener.onCancel += OnSelectCancel;
	}

	void OnDisable()
	{
		PickerEventListener.onImageSelect -= OnImageSelect;
		PickerEventListener.onImageLoad -= OnImageLoad;
		PickerEventListener.onError -= OnError;
		PickerEventListener.onCancel -= OnSelectCancel;
	}
	
	void Start(){
		googleAnalytics.LogScreen(new AppViewHitBuilder().SetScreenName("Setting Menu"));
		FindViewByID();
		SetSettingButtonsText();
		FillCountriesComboBox();
		InitTabStates();
		LoadData();
		//saveAvatarImage = false;
	}
	

	void Update(){
		if (Input.GetKey(KeyCode.Escape)) { 
			MainController.SwitchScene(MainController.MENU_SCENE);
		}
	}

	public void OpenGameBoard(){
		MainController.SwitchScene(MainController.GAMEBOARD_SCENE);
	}

	public void OpenRanking () {
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


	
	void FindViewByID(){
		try{
			UserTabText = GameObject.Find("UserProfileTabText").GetComponent<Text>();
			ThemeTabText = GameObject.Find("ThemeTabText").GetComponent<Text>();
			UserTabImg = GameObject.Find("UserTabSelectedImg").GetComponent<Image>();
			ThemeTabImg = GameObject.Find("ThemeTabSelectedImg").GetComponent<Image>();
			AvatarImg = GameObject.Find("AvatarImage").GetComponent<Image>();
			
			AvatarNameInput = GameObject.Find("AvatarNameInput").GetComponent<InputField>();
			UserNameInput = GameObject.Find("UserNameInput").GetComponent<InputField>();
			UserSurenameInput = GameObject.Find("UserSurnameInput").GetComponent<InputField>();
			UserPhoneInput = GameObject.Find("UserPhoneInput").GetComponent<InputField>();

			AllowMatchInvitations = GameObject.Find("AllowMatchInvitationToggle").GetComponent<Toggle>();
			AllowChatToggle = GameObject.Find("AllowChatToggle").GetComponent<Toggle>();
			PublicProfileToggle = GameObject.Find ("PublicProfileToggle").GetComponent<Toggle> ();
			MusicToggle = GameObject.Find ("MusicToggle").GetComponent<Toggle> ();
			AllowPromotions = GameObject.Find("AllowPromotionsToggle").GetComponent<Toggle> ();
			
			CountryComboBox = GameObject.Find("CountryComboBox").GetComponent<ComboBox>();
			LevelComboBox = GameObject.Find("LevelComboBox").GetComponent<ComboBox>();
			GenderComboBox = GameObject.Find("GenderComboBox").GetComponent<ComboBox>();


			MusicToggle.onValueChanged.AddListener(MuteMusic);

		}
		catch (Exception ex)
		{
			Debug.Log("SettingController:FindViewByID() " + ex.ToString());
		}
	}

	private void MuteMusic(bool active){

		AudioSource[] obj = FindObjectsOfType(typeof (AudioSource)) as AudioSource[];
		foreach (AudioSource audiosrc in obj)
		{
			//GameObject g = (GameObject)o;
			//AudioSource asource = g.GetComponent<AudioSource>();
			Debug.Log(audiosrc.name);
			if(audiosrc.name.Contains("MusicSource")){
				Debug.Log("we are now able to turn off the MusicSource prefab");
				if(MusicToggle.isOn){
					Debug.Log("audioMusic.Play()...");
					audiosrc.Play();
				}else{
					Debug.Log("audioMusic.Stop()...");
					audiosrc.Stop();
				}
			}
		}
	}


	void SaveData(){
		try{
			Debug.Log(".......SaveData().......");
			
			int selectedCountryIndex = CountryComboBox.SelectedIndex;
			int selectedLevelIndex = LevelComboBox.SelectedIndex;
			int selectedGenderIndex = GenderComboBox.SelectedIndex;			


			Debug.Log("SETTINGSCONTROLLER:saveAvatarImage: " + GameApplication.Instance.SaveAvatar);

			if(GameApplication.Instance.SaveAvatar)
				avatarImage = AvatarImg.sprite.texture.EncodeToJPG();
			else
				avatarImage = null;

			User user = new User();
			user.Name = UserNameInput.text;
			user.Surname = UserSurenameInput.text;
			user.AvatarName = AvatarNameInput.text;
			user.Gender = selectedGenderIndex;
			user.MobilePhone = UserPhoneInput.text;
			user.AllowMatchInvitations = AllowMatchInvitations.isOn;
			user.AllowChat = AllowChatToggle.isOn;
			user.HasPublicProfile = PublicProfileToggle.isOn;
			user.AvatarImage = avatarImage;
			user.EnableMusic = MusicToggle.isOn;
			user.AllowPromotionalOffers = AllowPromotions.isOn;

			Country playerCountry = new Country();
			playerCountry.Id = selectedCountryIndex;
			playerCountry.Name = CountryComboBox.Items[selectedCountryIndex].Caption;
			user.country = playerCountry;

			Level playerLevel = new Level();
			playerLevel.MinMatchLevelId = selectedLevelIndex;
			playerLevel.MinMatchLevelName = LevelComboBox.Items[selectedLevelIndex].Caption;
			user.level = playerLevel;
		
			userDAO.StoreUserOnLocalMemory(user);

			GameApplication.Instance.Player1 = user;

			//if(Application.internetReachability != NetworkReachability.NotReachable){
			//	Debug.Log("internetReachability Connection OK...");
			//	WebServiceAdapter wsa = new WebServiceAdapter ();
				//StartCoroutine (wsa.SaveUserDataOnServer(user));
			//}
		}
		catch(Exception ex){
			Debug.LogError("SettingsController:SaveData() " + ex.ToString());
		}

	}
	
	void LoadData()
	{
		Debug.Log("LoadingData().....");
		userDAO = new UserDAO();
		User userData = userDAO.LoadUserFromLocalMemory();
		Debug.Log("userData.AvatarName: " + userData.AvatarName);
		if(userData != null)
			SetUserProfileInfo(userData);
		else
			AvatarNameInput.text = "Player1";
	}
	
	void SetUserProfileInfo(User user)
	{
		if(user != null){
			Debug.Log("user is not NULL...");
		}
		else{
			Debug.Log("user is NULL...");
		}

		try{
			if(user != null){
				Debug.Log("user.AvatarName: " + user.AvatarName);

				if(user.AvatarName != null)
					AvatarNameInput.text = user.AvatarName;
				else
					AvatarNameInput.text = string.Empty;

				if(user.Name != null)
					UserNameInput.text = user.Name;
				else
					UserNameInput.text = string.Empty;

				if(user.Surname != null)
					UserSurenameInput.text = user.Surname;
				else
					UserSurenameInput.text = string.Empty;

				if(user.MobilePhone != null)
					UserPhoneInput.text = user.MobilePhone;
				else
					UserPhoneInput.text = string.Empty;
			
				//if(user.AllowChat != null)
					AllowChatToggle.isOn = user.AllowChat;

				//if(user.HasPublicProfile != false)
					PublicProfileToggle.isOn = user.HasPublicProfile;
		

				if(user.country.Id != 0)
					CountryComboBox.SelectItem(user.country.Id);
				else
					CountryComboBox.SelectItem(0);

				if(user.level.MinMatchLevelId != 0)
					LevelComboBox.SelectItem(user.level.MinMatchLevelId);
				else
					LevelComboBox.SelectItem(0);

				if(user.Gender != 0)
					GenderComboBox.SelectItem(user.Gender);
				else
					GenderComboBox.SelectItem(0);

				//if(user.EnableMusic != null)
				MusicToggle.isOn = user.EnableMusic;

				Debug.Log("user.EnableMusic(): " + user.EnableMusic);
	

				if(AvatarImg != null)
				{
					Texture2D textureData = new Texture2D(AvatarImg.mainTexture.width, AvatarImg.mainTexture.height, TextureFormat.RGB24, false);
					if(user.AvatarImage != null)
					{
						GameApplication.Instance.SaveAvatar = true;
						textureData.LoadImage(user.AvatarImage);
						AvatarImg.sprite =  Sprite.Create(textureData,new Rect(0,0, textureData.width, textureData.height),new Vector2(0.5f, 0.5f));
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log("SetUserProfileInfo:ex " + ex.ToString());
		}
	}
	
	/*
	void OnEditTextEnd ()
	{
		var input = gameObject.GetComponent<InputField>();
		var se= new InputField.SubmitEvent();
		se.AddListener(SubmitName);
		input.onEndEdit = se;
	}
	
	private void SubmitName(string arg0)
	{
		Debug.Log(arg0);
	}
	*/
	
	void InitTabStates(){
		ThemeTabImg.enabled = false;
		//Button UserTab = GameObject.Find("UserProfileTabText").GetComponent<Button>();
	}

	public void BrowseImageFromDevice(){
	#if UNITY_ANDROID
		AndroidPicker.BrowseImage();
	#elif UNITY_IPHONE
		IOSPicker.BrowseImage();
	#endif
	}

	void OnImageSelect(string imgPath)
	{
		GameApplication.Instance.SaveAvatar = true;
		//saveAvatarImage = true;
	}

	void OnSelectCancel(){

	}

	void OnImageLoad(string imgPath, Texture2D texture)
	{
		try{
			Debug.Log("SettingsController:OnImageLoad()");
			Sprite avatarPic = Sprite.Create(texture,new Rect(0,0, texture.width, texture.height),new Vector2(0.5f, 0.5f));
			avatarImage = texture.EncodeToJPG();
			AvatarImg.sprite =  avatarPic;

		}
		catch(Exception ex){
			Debug.LogError("OnImageLoad:Error() " + ex.ToString());
		}
	}

	void OnError(string error)
	{
		//Debug.LogError("SettingsController:OnError: " + error);
		MessageHandler.ShowWarning();
	}
	
	public void SelectUserProfile(){
		UserTabText.color = Color.white;
		ThemeTabText.color = Color.black;
		UserTabImg.enabled = true;
		ThemeTabImg.enabled= false;
	}
	
	public void SelectThemeSetup(){
		ThemeTabText.color = Color.white;
		UserTabText.color = Color.black;
		UserTabImg.enabled = false;
		ThemeTabImg.enabled= true;
	}
	
	
	void FillCountriesComboBox(){
		
		Language.SetCountriesName(MainController.AppCurrentLanguage);
		
		string[] countries = new string[Language.AppCountries.Count];
		
		for ( int j = 0; j < Language.AppCountries.Count; j++ )  {
			countries[j] = Language.AppCountries.GetByIndex(j).ToString();
		}
		CountryComboBox.AddItems(countries);
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
		
		ThemeTabText = GameObject.Find("MainSceneTitle").GetComponent<Text>();
		ThemeTabText.text = lang.GetString("game_settings");
		
		ThemeTabText = GameObject.Find("ThemeTabText").GetComponent<Text>();
		ThemeTabText.text = lang.GetString("theme_profile_tab");
		
		SettingButtonText = GameObject.Find("UserProfileTabText").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("user_profile_tab");
		
		SettingButtonText = GameObject.Find("UserProfileTabText").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("user_profile_tab");
		
		SettingButtonText = GameObject.Find("AvatarSettingTitle").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("avatar_settings_title");
		
		SettingButtonText = GameObject.Find("AvatarNameText").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("avatar_name");
		
		SettingButtonText = GameObject.Find("UserGenderText").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("user_gender");
		
		SettingButtonText = GameObject.Find("AllowMatchesText").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("user_allow_match_invites");
		
		SettingButtonText = GameObject.Find("AllowChatText").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("user_allow_chat");
		
		SettingButtonText = GameObject.Find("PublicProfileText").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("user_public_profile");
		
		SettingButtonText = GameObject.Find("PersonalDetailTitle").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("user_personal_details_title");
		
		SettingButtonText = GameObject.Find("UserNameText").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("user_name");
		
		SettingButtonText = GameObject.Find("UserSurnameText").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("user_surname");
		
		SettingButtonText = GameObject.Find("UserPhoneText").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("user_phone");
		
		SettingButtonText = GameObject.Find("AllowPromotionsText").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("user_allow_offers");
		
		SettingButtonText = GameObject.Find("UserCountryText").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("user_country");
		
		SettingButtonText = GameObject.Find("UserMinLevelText").GetComponent<Text>();
		SettingButtonText.text = lang.GetString("user_min_level");					
	}

	void OnDestroy(){
		SaveData();
		Debug.Log("SettingsController:OnDestroy()");
		if(settingsController != null)
		{
			settingsController = null;
		}
	}
}