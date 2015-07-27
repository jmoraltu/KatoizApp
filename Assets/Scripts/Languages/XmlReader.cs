using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;


public class XmlReader : MonoBehaviour {

	public TextAsset dictionary;
	public string languageName;
	public int currentLanguage;
	public string btnFriends;
	public string btnNewGame;
	public string btnGameRules;
	public string btnGameInvitations;
	public string btnGameSettings;
	public string spanishLang;
	public string englishLang;
	

	List<Dictionary<string,string>> languages = new List<Dictionary<string,string>>();
	Dictionary<string,string> tags;


	void Awake(){
		Reader ();
	}

	void Start () {
		setAvailableLang ();
	}

	void Update () {
		languages[currentLanguage].TryGetValue("name", out languageName);
		languages[currentLanguage].TryGetValue("newgame", out btnNewGame);
		languages[currentLanguage].TryGetValue("gamefriends", out btnFriends);
	}

	void setAvailableLang(){
		languages[0].TryGetValue("name", out spanishLang);
		languages[1].TryGetValue("name", out englishLang);
	}
	

	void OnGUI(){


		GUI.skin.button.fontSize = (int) (0.05f * Screen.height);
		
		
		Rect newGameRect = new Rect(0.1f * Screen.width, 0.05f * Screen.height,
		                          0.8f * Screen.width, 0.1f * Screen.height);
		if (GUI.Button(newGameRect, btnNewGame))
		{

		}
		
		
		Rect btnFriendsRect = new Rect(0.1f * Screen.width, 0.175f * Screen.height,
		                                         0.8f * Screen.width, 0.1f * Screen.height);
		if (GUI.Button(btnFriendsRect, btnFriends))
		{

		}

		Rect spanishRect = new Rect(0.1f * Screen.width, 0.8f * Screen.height,
		                               0.4f * Screen.width, 0.1f * Screen.height);


		if (GUI.Button(spanishRect,spanishLang))
		{
			currentLanguage = 0;
		}

		Rect englishRect = new Rect(0.1f * Screen.width + spanishRect.width, 0.8f * Screen.height,
		                                 0.4f * Screen.width, 0.1f * Screen.height);

		if (GUI.Button(englishRect, englishLang))
		{
			currentLanguage = 1;
		}
	}
	
	void Reader(){
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml (dictionary.text);
		XmlNodeList languageList = xmlDoc.GetElementsByTagName ("language");

		foreach (XmlNode languageValue in languageList) {
			XmlNodeList langContent = languageValue.ChildNodes;
			tags = new Dictionary<string, string>();

			foreach(XmlNode value in langContent){
				if(value.Name == "name"){
					tags.Add(value.Name,value.InnerText);
				}
				if(value.Name == "newgame"){
					tags.Add(value.Name,value.InnerText);
				}
				if(value.Name == "gamefriends"){
					tags.Add(value.Name,value.InnerText);
				}
			}

			languages.Add(tags);
		}
	}
	
}
