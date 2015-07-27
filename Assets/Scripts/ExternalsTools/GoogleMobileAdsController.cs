using UnityEngine;
using System;
using System.Net;
using System.Collections;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class GoogleMobileAdsController : Singleton<GoogleMobileAdsController> {

	private BannerView bannerView;
	private InterstitialAd interstitial;
	private bool LogsActivated = false;

	private GoogleMobileAdsController(){
		
	}

	public void Activate(){
		Debug.Log("GoogleMobileAdsController:Activate()....");
		RequestBanner ();
		RequestInterstitial();
	}


	public void ShowBanner(){
		//Debug.Log("GoogleMobileAdsController:ShowBanner()....");
		if(bannerView != null){
			bannerView.Show();
		}
	}

	public void HideBanner(){
		if(bannerView != null){
			bannerView.Hide();
		}
	}

	public void DestroyBanner(){
		if(bannerView != null){
			bannerView.Destroy();
		}
	}

	public void ShowInterstitial(){

		if (interstitial != null && interstitial.IsLoaded())
		{
			interstitial.Show();
		}else{
			Debug.Log("ShowInterstitial(): interstitial.IsNotLoaded()");
		}
	}

	public void DestroyInterstitia(){
		if(interstitial != null){
			interstitial.Destroy();
		}
	}

	private IEnumerator Wait() 
	{
		yield return new WaitForSeconds(3);
		bannerView.Show();
	}

	
	private void RequestBanner()
	{
		#if UNITY_EDITOR
			string adUnitId = "unused";
		#elif UNITY_ANDROID
			//string adUnitId = "ca-app-pub-3940256099942544/6300978111"; //testing ad
			string adUnitId = "ca-app-pub-4397098216678632/4567041306"; //Quo Admob ads
		#elif UNITY_IPHONE
			string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
		#else
			string adUnitId = "unexpected_platform";
		#endif
		
		// Create a AdSize.Banner 320x50 banner at the bottom of the screen.
		bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
		// Create a AdSize.CustomBanner 300x50 banner at the bottom of the screen.
		//bannerView = new BannerView(adUnitId, AdSize.CustomBanner, AdPosition.Bottom);
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

	
	// Returns an ad request with custom ad targeting.
	private AdRequest createAdRequest()
	{
		//PublisherAdView pav;
		//PublisherAdRequest adRequest = new PublisherAdRequest.Builder ();
		
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


	private void RequestInterstitial()
	{
		#if UNITY_EDITOR
			string adUnitId = "unused";
		#elif UNITY_ANDROID
			string adUnitId = "ca-app-pub-4397098216678632/6534798900"; //Quo ads
			//string adUnitId = "ca-app-pub-3940256099942544/1033173712"; //Ads Test
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
		//interstitial.LoadAd(createAdRequest());
		interstitial.LoadAd(new AdRequest.Builder().Build());

	}

	private void ShowInterstitialBanner()
	{
		if (interstitial.IsLoaded())
		{
			interstitial.Show();
		}
		else
		{
			if(LogsActivated)
				Debug.Log("ShowInterstitialBanner(): Interstitial is not ready yet.");
		}
	}
	
	#region Banner callback handlers
	
	public void HandleAdLoaded(object sender, EventArgs args)
	{
		if(LogsActivated)
			print("HandleAdLoaded event received.");
	}
	
	public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		if(LogsActivated)
			print("HandleFailedToReceiveAd event received with message: " + args.Message);
	}
	
	public void HandleAdOpened(object sender, EventArgs args)
	{
		if(LogsActivated)
			print("HandleAdOpened event received");
	}
	
	void HandleAdClosing(object sender, EventArgs args)
	{
		if(LogsActivated)
			print("HandleAdClosing event received");
	}
	
	public void HandleAdClosed(object sender, EventArgs args)
	{
		if(LogsActivated)
			print("HandleAdClosed event received");
	}
	
	public void HandleAdLeftApplication(object sender, EventArgs args)
	{
		if(LogsActivated)
			print("HandleAdLeftApplication event received");
	}
	
	#endregion


	#region Interstitial callback handlers
	
	public void HandleInterstitialLoaded(object sender, EventArgs args)
	{
		if(LogsActivated)
			print("HandleInterstitialLoaded event received.");
	}
	
	public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		if(LogsActivated)
			print("HandleInterstitialFailedToLoad event received with message: " + args.Message);
	}
	
	public void HandleInterstitialOpened(object sender, EventArgs args)
	{
		if(LogsActivated)
			print("HandleInterstitialOpened event received");
	}
	
	void HandleInterstitialClosing(object sender, EventArgs args)
	{
		if(LogsActivated)
			print("HandleInterstitialClosing event received");
	}
	
	public void HandleInterstitialClosed(object sender, EventArgs args)
	{
		if(LogsActivated)
			print("HandleInterstitialClosed event received");
	}
	
	public void HandleInterstitialLeftApplication(object sender, EventArgs args)
	{
		if(LogsActivated)
			print("HandleInterstitialLeftApplication event received");
	}
	
	#endregion
}
