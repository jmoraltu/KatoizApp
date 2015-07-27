using System;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using GooglePlayGames;

// Example script showing how to invoke the Google Game Services Unity plugin.
public class GoogleGameServices : MonoBehaviour
{
	
	private BannerView bannerView;
	private InterstitialAd interstitial;
	public String achivement1 = "CgkIuoqW9doZEAIQAQ";
	public String achivement2 = "CgkIuoqW9doZEAIQAg";
	public String leaderboard = "CgkIuoqW9doZEAIQBg";
	
	
	void Start(){
		PlayGamesPlatform.Activate ();
		RequestBanner();
		bannerView.Show();
	}
	
	void OnGUI()
	{
		// Puts some basic buttons onto the screen.
		GUI.skin.button.fontSize = (int) (0.05f * Screen.height);
		
		
		Rect loginRect = new Rect(0.1f * Screen.width, 0.05f * Screen.height,
		                          0.8f * Screen.width, 0.1f * Screen.height);
		if (GUI.Button(loginRect, "Login"))
		{
			loginApp();
		}

		
		Rect showUnlockAchivementRect = new Rect(0.1f * Screen.width, 0.175f * Screen.height,
		                               0.8f * Screen.width, 0.1f * Screen.height);
		if (GUI.Button(showUnlockAchivementRect, "Unlock Achivement (ads)"))
		{
			unlockAchivement();
		}
		
		Rect unlockIncrementAchivementRect = new Rect(0.1f * Screen.width, 0.3f * Screen.height,
		                               0.8f * Screen.width, 0.1f * Screen.height);
		if (GUI.Button(unlockIncrementAchivementRect, "Unlock Increment Achivement"))
		{
			unlockIncrementAchivement();
		}
		
		Rect setScoreToLeaderboardRect = new Rect(0.1f * Screen.width, 0.425f * Screen.height,
		                                  0.8f * Screen.width, 0.1f * Screen.height);
		if (GUI.Button(setScoreToLeaderboardRect, "Set Leaderboard Score"))
		{
			setScoreToLeaderboard();
		}
		
		Rect showLeaderBoardRect = new Rect(0.1f * Screen.width, 0.55f * Screen.height,
		                                        0.8f * Screen.width, 0.1f * Screen.height);
		if (GUI.Button(showLeaderBoardRect, "Show Leaderboard"))
		{
			showLeaderboardUI();
		}


		Rect showAchivementUIRect = new Rect(0.1f * Screen.width, 0.675f * Screen.height,
		                                     0.8f * Screen.width, 0.1f * Screen.height);
		if (GUI.Button(showAchivementUIRect, "Show Achivements"))
		{
			showAchivementUI();
		}

		Rect showLogOutRect = new Rect(0.1f * Screen.width, 0.8f * Screen.height,
		                                     0.8f * Screen.width, 0.1f * Screen.height);
		if (GUI.Button(showLogOutRect, "Sign Out"))
		{
			signOut();
		}




	}


	//Login app
	private void loginApp(){
		Social.localUser.Authenticate((bool success) => {
			if(success){
				Debug.Log("You have logged in");
			}
			else{
				Debug.Log("Login Fail try again");
			}
		});
	}
	
	
	//Achivement
	private void unlockAchivement(){
		Social.ReportProgress(achivement1, 100.0f, (bool success) => {
			// Googles automatically throws a pop up window
		});
	}
	
	
	//Increment achivement
	private void unlockIncrementAchivement(){
		PlayGamesPlatform.Instance.IncrementAchievement(
			achivement2, 5, (bool success) => {
			// Googles automatically throws a pop up window
		});
	}
	
	//Set Leaderboard
	private void setScoreToLeaderboard(){
		Social.ReportScore(1500, leaderboard, (bool success) => {
			// handle success or failure
		});
	}
	
	private void showLeaderboardUI(){
		//Social.ShowLeaderboardUI ();
		((PlayGamesPlatform)Social.Active).ShowLeaderboardUI ();
		//PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboard);
	}
	
	private void showAchivementUI(){
		Social.ShowAchievementsUI();
	}
	
	private void signOut(){
		((PlayGamesPlatform)Social.Active).SignOut();
	}
	
	
	
	
	
	private void RequestBanner()
	{
		#if UNITY_EDITOR
		string adUnitId = "unused";
		#elif UNITY_ANDROID
		//string adUnitId = "INSERT_ANDROID_BANNER_AD_UNIT_ID_HERE";
		//string adUnitId = "ca-app-pub-3940256099942544/6300978111";
		string adUnitId = "ca-app-pub-6491690976590361/1556486938";
		
		#elif UNITY_IPHONE
		string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
		#else
		string adUnitId = "unexpected_platform";
		#endif
		
		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);
		// Register for ad events.
		bannerView.AdLoaded += HandleAdLoaded;
		bannerView.AdFailedToLoad += HandleAdFailedToLoad;
		bannerView.AdOpened += HandleAdOpened;
		bannerView.AdClosing += HandleAdClosing;
		bannerView.AdClosed += HandleAdClosed;
		bannerView.AdLeftApplication += HandleAdLeftApplication;
		// Load a banner ad.
		//bannerView.LoadAd(createAdRequest());
		bannerView.LoadAd(new AdRequest.Builder().Build());
	}
	
	private void RequestInterstitial()
	{
		#if UNITY_EDITOR
		string adUnitId = "unused";
		#elif UNITY_ANDROID
		//string adUnitId = "INSERT_ANDROID_INTERSTITIAL_AD_UNIT_ID_HERE";
		string adUnitId = "ca-app-pub-6491690976590361/1556486938";
		#elif UNITY_IPHONE
		string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
		#else
		string adUnitId = "unexpected_platform";
		#endif
		
		// Create an interstitial.
		interstitial = new InterstitialAd(adUnitId);
		// Register for ad events.
		interstitial.AdLoaded += HandleInterstitialLoaded;
		interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;
		interstitial.AdOpened += HandleInterstitialOpened;
		interstitial.AdClosing += HandleInterstitialClosing;
		interstitial.AdClosed += HandleInterstitialClosed;
		interstitial.AdLeftApplication += HandleInterstitialLeftApplication;
		// Load an interstitial ad.
		interstitial.LoadAd(createAdRequest());
	}
	
	// Returns an ad request with custom ad targeting.
	private AdRequest createAdRequest()
	{
		return new AdRequest.Builder()
			.AddTestDevice(AdRequest.TestDeviceSimulator)
				.AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
				.AddKeyword("game")
				.SetGender(Gender.Male)
				.SetBirthday(new DateTime(1985, 1, 1))
				.TagForChildDirectedTreatment(false)
				.AddExtra("color_bg", "9B30FF")
				.Build();
		
	}
	
	private void ShowInterstitial()
	{
		if (interstitial.IsLoaded())
		{
			interstitial.Show();
		}
		else
		{
			print("Interstitial is not ready yet.");
		}
	}
	
	#region Banner callback handlers
	
	public void HandleAdLoaded(object sender, EventArgs args)
	{
		print("HandleAdLoaded event received.");
	}
	
	public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		print("HandleFailedToReceiveAd event received with message: " + args.Message);
	}
	
	public void HandleAdOpened(object sender, EventArgs args)
	{
		print("HandleAdOpened event received");
	}
	
	void HandleAdClosing(object sender, EventArgs args)
	{
		print("HandleAdClosing event received");
	}
	
	public void HandleAdClosed(object sender, EventArgs args)
	{
		print("HandleAdClosed event received");
	}
	
	public void HandleAdLeftApplication(object sender, EventArgs args)
	{
		print("HandleAdLeftApplication event received");
	}
	
	#endregion
	
	#region Interstitial callback handlers
	
	public void HandleInterstitialLoaded(object sender, EventArgs args)
	{
		print("HandleInterstitialLoaded event received.");
	}
	
	public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		print("HandleInterstitialFailedToLoad event received with message: " + args.Message);
	}
	
	public void HandleInterstitialOpened(object sender, EventArgs args)
	{
		print("HandleInterstitialOpened event received");
	}
	
	void HandleInterstitialClosing(object sender, EventArgs args)
	{
		print("HandleInterstitialClosing event received");
	}
	
	public void HandleInterstitialClosed(object sender, EventArgs args)
	{
		print("HandleInterstitialClosed event received");
	}
	
	public void HandleInterstitialLeftApplication(object sender, EventArgs args)
	{
		print("HandleInterstitialLeftApplication event received");
	}
	
	#endregion
}