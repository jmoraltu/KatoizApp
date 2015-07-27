using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class Device{

	public Device(){}

	private string deviceId;
	private string model;
	private string uniqueIdentifier;

	public string DeviceId
	{
		get
		{
			return GetDeviceId();
		}
	}

	public string Model
	{
		get
		{
			return GetModel();
		}
	}

	public string UniqueIdentifier
	{
		get
		{
			return GetUniqueIdentifier();
		}
	}

	public string GetDeviceId()
	{
		#if UNITY_ANDROID 
			//return Settings.Secure.getString(context.getContentResolver(), Settings.Secure.ANDROID_ID);
			AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject> ("currentActivity");
			AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject> ("getContentResolver");  
			AndroidJavaClass secure = new AndroidJavaClass ("android.provider.Settings$Secure");
			string deviceUniqueId = secure.CallStatic<string> ("getString", contentResolver, "android_id");
			return deviceUniqueId;
		#elif UNITY_IPHONE
			//http://forum.unity3d.com/threads/unity-4-2-systeminfo-deviceuniqueidentifier-regression.194045/
			//problem w/ this property on http://fogbugz.unity3d.com/default.asp?672928_f9m11fc79gran1s7
			return	SystemInfo.deviceUniqueIdentifier;
		#endif
		//return	SystemInfo.deviceUniqueIdentifier;
	}

	public string GetModel(){
		return SystemInfo.deviceModel;
	}

	public string GetUniqueIdentifier(){
		return SystemInfo.deviceUniqueIdentifier;
	}




}
