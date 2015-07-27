using System;
using UnityEngine;
using System.Collections;
using GooglePlayGames;

public class GooglePlayGameController : Singleton<GooglePlayGameController> {


	protected GooglePlayGameController () {} 

	public void Activate(){
		Debug.Log ("GooglePlayGameController :" + GooglePlayGames.PlayGamesPlatform.Instance.IsAuthenticated ());
		if (GooglePlayGames.PlayGamesPlatform.Instance.IsAuthenticated ().Equals(false)) {
			PlayGamesPlatform.Activate ();
			StartCoroutine(Wait());
		}
	}

	private IEnumerator Wait() 
	{
		yield return new WaitForSeconds(1);
		loginApp ();
	}

	//Login app you have to setup your google play game service Id con the unity play games option
	private void loginApp(){
		Social.localUser.Authenticate((bool success) => {
			if(success){
				Debug.Log("You have logged in");
				Debug.Log("USERNAME: " + Social.localUser.userName);
				Debug.Log("GetUserDisplayName: " + ((PlayGamesPlatform)Social.Active).GetUserDisplayName());
				Debug.Log("GetUserImageUrl: " + ((PlayGamesPlatform)Social.Active).GetUserImageUrl());
				Debug.Log("GetUserId: " + ((PlayGamesPlatform)Social.Active).GetUserId());
			}
			else{
				Debug.Log("Login Fail try again");
			}
		});
	}


	public void ShowLeaderboard(){
		((PlayGamesPlatform)Social.Active).ShowLeaderboardUI ();
	}

	public void ShowAchivements(){
		Social.ShowAchievementsUI();
	}

	public static void SignOut(){
		((PlayGamesPlatform)Social.Active).SignOut();
	}

	public string GetUserName(){
		return ((PlayGamesPlatform)Social.Active).GetUserDisplayName();
	}
}
