using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Utils {

	static int m_frameCounter = 0; 
	static float m_timeCounter = 0.0f; 
	static float m_lastFramerate = 0.0f; 
	static float m_refreshTime = 0.5f;

	// Use this for initialization
	public static string getDeviceId()
	{
		string deviceId = null;
		#if UNITY_ANDROID
			AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject> ("currentActivity");
			AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject> ("getContentResolver");  
			AndroidJavaClass secure = new AndroidJavaClass ("android.provider.Settings$Secure");
			deviceId = secure.CallStatic<string> ("getString", contentResolver, "android_id");
			Debug.Log("-- secure.CallStatic DeviceId: --"  + deviceId);
		    Debug.Log("-- SystemInfo.deviceUniqueIdentifier: --"  + SystemInfo.deviceUniqueIdentifier);
		#elif UNITY_IPHONE
			deviceId = SystemInfo.deviceUniqueIdentifier; //+ "-JM-1";// "iosdeviceid001-JM-01";
		#endif
		Debug.Log("-- DeviceId: --"  + deviceId);
		return deviceId;
	}
	
	public static List<User> GetUserFriends(){
		List<User> FriendsList = new List<User>();
		char[] letras = {'A','B','C','D','E','F','G','H','I','J'};
		string[] level = {"Easy","Normal","High","Advanced"};
		string[] country = {"Spain","England","United States","Australia"};
		
		User user = new User();
		Game playerGame = new Game();
		Level playerLevel = new Level();
		Country playerCountry = new Country();
		for(int i=0; i < 6; i++){
			user = new User();
			System.Random rnd = new System.Random((int) DateTime.Now.Ticks & 0x0000FFFF);
			user.AvatarName=letras[rnd.Next(10)] + "_FriendName_"+i;
			int rand = rnd.Next(2);
			user.IsOnLine = Convert.ToBoolean(rand);
			
			playerLevel = new Level();
			playerLevel.Name = level[rnd.Next(4)];
			user.level = playerLevel;
			
			playerGame = new Game();
			playerGame.Wins = rnd.Next(100,300);
			playerGame.Ties = rnd.Next(50,200);
			playerGame.Played = playerGame.Wins + playerGame.Ties;
			user.game = playerGame;

			playerCountry = new Country();
			playerCountry.Name = country[rnd.Next(4)];
			user.country = playerCountry;

			
			FriendsList.Add(user);
		}
		FriendsList.Sort();
		return FriendsList;
	}

	public static void CalculateFrameRate(){
		if( m_timeCounter < m_refreshTime ) 
		{ 
			m_timeCounter += Time.deltaTime; 
			m_frameCounter++; 
		} else { 
			//This code will break if you set your m_refreshTime to 0, which makes no sense. 
			m_lastFramerate = (float)m_frameCounter/m_timeCounter; m_frameCounter = 0; m_timeCounter = 0.0f; 
			Debug.Log("m_lastFramerate: " + m_lastFramerate);
		} 
	}

}
