using UnityEngine;
using System.Collections;
using System;


public class WebServiceAdapter : MonoBehaviour {

	private static string APP_ID = "1";
	private static string URL = "http://ws-app.quovasys.net:88/user.asmx/";
	private static string GET_USER_METHOD = "GetPrivateInfoByKey";
	private static string REGISTER_USER_METHOD = "RegisterKey";
	private static string SAVE_USER_DATA_METHOD = "RegisterKey";
	private static string APP_NAME_KEY = "katoiz-001";


	//<summary>
	//	Register the DeviceID if not exist
	//</summary>
	// <param name="userId">Device Id</param>
	// <param name="userLang">Device current lang</param>
	public IEnumerator GetUserByKey(string userId, string userLang){
		string requestQueryUser = URL + GET_USER_METHOD;

		if(userLang.Equals("English")){
			userLang = "en";
		}
		else if(userLang.Equals("Spanish")){
			userLang = "es";
		}

		//Debug.Log(">>>>> GetUserByKey()  "  + requestQueryUser);
		WWWForm requestForm = new WWWForm();
		requestForm.AddField ("appid", APP_ID);
		requestForm.AddField ("keyid", userId);
		requestForm.AddField ("userlang", userLang);
		requestForm.AddField ("seckey", "");

		WWW queryUserResponse = new WWW(requestQueryUser, requestForm);
		yield return queryUserResponse;


		if (!string.IsNullOrEmpty(queryUserResponse.error)) {
			Debug.Log (string.Format ("User Not Found or Empty {0}", queryUserResponse.error));
		}
		else {
			Debug.Log("queryUserResponse:  "  + queryUserResponse.text);
			JSONObject json = new JSONObject(queryUserResponse.text);

			//AccessData(json);

			if(!json.type.Equals(JSONObject.Type.STRING)){
				try{
					string stringObj = json.list[4].list[0].str;
					Debug.Log("json.list[4].list[0].str" + json.list[4].list[0].str);
					if(stringObj.Equals("OK")){
						Debug.Log("User already registered");
					}
				}
				catch(Exception ex){
					Debug.Log("GetUserByKey:ex " + ex);
				}
			}
			else{
				Debug.Log("Registering user...");

				string requestRegisterURL = URL + REGISTER_USER_METHOD;

				JSONObject jsonParams = new JSONObject(JSONObject.Type.OBJECT);
				jsonParams.AddField("appid", APP_ID);
				jsonParams.AddField("keyid", userId);
				jsonParams.AddField("UserLang", userLang);

				string seckey = generateSecurityKey(requestRegisterURL, jsonParams);

				//Debug.Log("App_id: " + APP_ID + " keyid: " + userId + " userLang: " + userLang + " seckey: " + seckey.Trim());

				//Debug.Log (">>>>>(seckey) GetUserByKey: " + seckey.Trim());

				WWWForm requestRegisterForm = new WWWForm();
				requestRegisterForm.AddField ("appid", APP_ID); //APP_ID
				requestRegisterForm.AddField ("keyid", userId); //userId
				requestRegisterForm.AddField ("UserLang", userLang);
				requestRegisterForm.AddField ("seckey", seckey.Trim());
				
				Debug.Log("ws response: " + requestRegisterForm.ToString());
				

				WWW registerUserResponse = new WWW(requestRegisterURL, requestRegisterForm);
				yield return registerUserResponse;
				
				//Debug.Log(registerUserResponse.ToString());
				
				if (!string.IsNullOrEmpty(registerUserResponse.error)) {
					Debug.Log("Error Null Or Empty " + registerUserResponse.error);
				}
				else {
					Debug.Log("User Registration OK  "  + registerUserResponse.text);
				}

			}
		}

	}

	string generateSecurityKey(string url, JSONObject param){

		string strToEncrypt = url + param.ToString () + APP_NAME_KEY;

		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding ();
		byte[] bytes = ue.GetBytes (strToEncrypt);

		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider ();
		byte[] hashBytes = md5.ComputeHash (bytes);

		string hashString = "";
		
		for (int i = 0; i < hashBytes.Length; i++) {
			hashString += System.Convert.ToString (hashBytes [i], 16);
		}
		return hashString.PadLeft (32);
	}


	public IEnumerator RegisterUser(string userId, string userLang){
		
		string requestURL = URL + REGISTER_USER_METHOD; 

		JSONObject jsonParams = new JSONObject(JSONObject.Type.OBJECT);
		jsonParams.AddField("appid", APP_ID);
		jsonParams.AddField("keyid", userId);
		jsonParams.AddField("UserLang", userLang);
		
		string seckey = generateSecurityKey(requestURL, jsonParams);
		
		Debug.Log("requestURL " +requestURL);
		
		// Create a Post Form
		WWWForm requestForm = new WWWForm();
		requestForm.AddField ("appid", APP_ID); //APP_ID
		requestForm.AddField ("keyid", userId); //userId
		requestForm.AddField ("UserLang", userLang);
		requestForm.AddField ("seckey", seckey.Trim());
		
		Debug.Log("postForm " + requestForm);
		
		WWW registerUserResponse = new WWW(requestURL, requestForm);
		yield return registerUserResponse;
		
		Debug.Log(registerUserResponse.ToString());
		
		if (!string.IsNullOrEmpty(registerUserResponse.error)) {
			Debug.Log("Error Null Or Empty " + registerUserResponse.error);
		}
		else {
			Debug.Log("User Registration OK  "  + registerUserResponse.text);
		}
	}


	public IEnumerator SaveUserDataOnServer(User user){
		
		string requestURL = URL + SAVE_USER_DATA_METHOD; 
		
		JSONObject jsonParams = new JSONObject(JSONObject.Type.OBJECT);
		//jsonParams.AddField("appid", APP_ID);
		jsonParams.AddField("userName", user.Name);
		jsonParams.AddField("UserSureName", user.Surname);
		jsonParams.AddField("UserAvatarName", user.AvatarName);
		jsonParams.AddField("UserPhone", user.MobilePhone);
		jsonParams.AddField("UserAllowChat", user.AllowChat.ToString());
		jsonParams.AddField("UserHasPublicProfile", user.HasPublicProfile.ToString());
		jsonParams.AddField("UserCountry", user.country.Id);
		jsonParams.AddField("UserLevel", user.level.Name);
		jsonParams.AddField("UserLevelIndex", user.level.Id);
		jsonParams.AddField("UserGender", user.Gender.ToString());
		jsonParams.AddField("UserAvatarImage", user.AvatarImage.ToString());

		//User user = new User(UserNameInput.text,UserSurenameInput.text, AvatarNameInput.text,UserPhoneInput.text,
		//AllowChatToggle.isOn,AllowChatToggle.isOn,PublicProfileToggle.isOn,selectedCountryIndex,selectedLevelIndex,
		//selectedGenderIndex, avatarImage);
		string seckey = generateSecurityKey(requestURL, jsonParams);
		
		Debug.Log("requestURL " +requestURL);
		
		// Create a Post Form
		WWWForm requestForm = new WWWForm();
		requestForm.AddField("userName", user.Name);
		requestForm.AddField("UserSureName", user.Surname);
		requestForm.AddField("UserAvatarName", user.AvatarName);
		requestForm.AddField("UserPhone", user.MobilePhone);
		requestForm.AddField("UserAllowChat", user.AllowChat.ToString());
		requestForm.AddField("UserHasPublicProfile", user.HasPublicProfile.ToString());
		requestForm.AddField("UserCountry", user.country.Id);
		requestForm.AddField("UserLevel", user.level.Name);
		requestForm.AddField("UserLevelIndex", user.level.Id);
		requestForm.AddField("UserGender", user.Gender);
		requestForm.AddField("UserAvatarImage", user.AvatarImage.ToString());
		requestForm.AddField("seckey", seckey.Trim());
		
		Debug.Log("postForm " + requestForm);
		
		WWW registerUserResponse = new WWW(requestURL, requestForm);
		yield return registerUserResponse;
		
		Debug.Log(registerUserResponse.ToString());
		
		if (!string.IsNullOrEmpty(registerUserResponse.error)) {
			Debug.Log("Error Null Or Empty " + registerUserResponse.error);
		}
		else {
			Debug.Log("User Registration OK  "  + registerUserResponse.text);
		}
	}

	//
	public static void AccessData(JSONObject obj){
		switch(obj.type){
		case JSONObject.Type.OBJECT:
			for(int i = 0; i < obj.list.Count; i++){
				string key = (string)obj.keys[i];
				JSONObject j = (JSONObject)obj.list[i];
				Debug.Log("OBJECT_KEY " + key);
				AccessData(j);
			}
			break;
		case JSONObject.Type.ARRAY:
			foreach(JSONObject j in obj.list){
				Debug.Log("ARRAY" );
				AccessData(j);
			}
			break;
		case JSONObject.Type.STRING:
			Debug.Log("STRING " + obj.str);
			break;
		case JSONObject.Type.NUMBER:
			Debug.Log(obj.n);
			break;
		case JSONObject.Type.BOOL:
			Debug.Log(obj.b);
			break;
		case JSONObject.Type.NULL:
			Debug.Log("NULL");
			break;
			
		}
	}

}
