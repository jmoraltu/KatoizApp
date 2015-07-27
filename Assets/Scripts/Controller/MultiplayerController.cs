using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;


public class MultiplayerController : RealTimeMultiplayerListener{

	private uint minimumOpponents = 1;
	private uint maximumOpponents = 1;
	private uint gameVariation = 0;

	private static MultiplayerController _instance = null;


	// Byte + Byte + 2 floats for position + 2 floats for velcocity + 1 float for rotZ
	//private int updateMessageLength = 22;
	private List<byte> updateMessage;

	
	private MultiplayerController() {
		updateMessage = new List<byte>();

		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate ();
	}
	
	public static MultiplayerController Instance {
		get {
			if (_instance == null) {
				_instance = new MultiplayerController();
			}
			return _instance;
		}
	}

	public void SignInAndStartMPGame() {
		if (!PlayGamesPlatform.Instance.localUser.authenticated) {
			PlayGamesPlatform.Instance.localUser.Authenticate((bool success) => {
				if (success) {
					Debug.Log ("SignInAndStartMPGame: We're signed in! Welcome " + PlayGamesPlatform.Instance.localUser.userName);
					// We could start our game now
					StartMatchMaking();
				} else {
					Debug.Log ("SignInAndStartMPGame: Oh... we're not signed in.");
				}
			});
		} else {
			Debug.Log ("SignInAndStartMPGame: You're already signed in.");
			// We could also start our game now
			StartMatchMaking();
		}
	}

	public void TrySilentSignIn() {
		if (! PlayGamesPlatform.Instance.localUser.authenticated) {
			PlayGamesPlatform.Instance.Authenticate ((bool success) => {
				if (success) {
					Debug.Log ("TrySilentSignIn: Silently signed in! Welcome " + PlayGamesPlatform.Instance.localUser.userName);
					StartMatchMaking();
				} else {
					Debug.Log ("TrySilentSignIn: Oh... we're not signed in.");
				}
			}, true);
		} else {
			Debug.Log("TrySilentSignIn: We're already signed in");
			StartMatchMaking();
		}
	}


	private void ShowMPStatus(string message) {
		Debug.Log("ShowMPStatus call");
		Debug.Log(message);
	}

	private void StartMatchMaking() {
		Debug.Log("StartMatchMaking....");
		PlayGamesPlatform.Instance.TurnBased.CreateQuickMatch (minimumOpponents, maximumOpponents, gameVariation, OnMatchStarted);
		//PlayGamesPlatform.Instance.RealTime.CreateQuickGame (minimumOpponents, maximumOpponents, gameVariation, this);
	}

	// Callback:
	void OnMatchStarted(bool success, TurnBasedMatch match) {
		if (success){
			// go to the game screen and play!
			// get the match data
			//byte[] myData = match.Data;
			if(match != null && match.PendingParticipantId !=null)
				Debug.Log("PendingParticipantID: " + match.PendingParticipantId);
			else
				Debug.Log("PendingParticipant ERROR");
		} else {
			Debug.Log("StartMatchMaking Error");
			// show error message
		}
	}

	public void SignOut() {
		PlayGamesPlatform.Instance.SignOut ();
	}
	
	public bool IsAuthenticated() {
		return PlayGamesPlatform.Instance.localUser.authenticated;
	}

	public void OnRoomSetupProgress (float percent)
	{
		ShowMPStatus ("We are " + percent + "% done with setup");
		throw new System.NotImplementedException ();
	}

	public void OnRoomConnected (bool success)
	{
		if (success) {
			ShowMPStatus ("We are connected to the room! I would probably start our game now.");
		} else {
			ShowMPStatus ("Uh-oh. Encountered some error connecting to the room.");
		}
	}

	public void OnLeftRoom ()
	{
		ShowMPStatus ("We have left the room. We should probably perform some clean-up tasks.");
	}

	public void OnParticipantLeft (Participant participant)
	{
		throw new System.NotImplementedException ();
	}

	public void OnPeersConnected (string[] participantIds)
	{
		foreach (string participantID in participantIds) {
			ShowMPStatus ("Player " + participantID + " has joined.");
		}
	}

	public void OnPeersDisconnected (string[] participantIds)
	{
		foreach (string participantID in participantIds) {
			ShowMPStatus ("Player " + participantID + " has left.");
		}
	}

	public void OnRealTimeMessageReceived (bool isReliable, string senderId, byte[] data)
	{
		Debug.Log("OnRealTimeMessageReceived: " + GetString(data));
		ShowMPStatus ("We have received some gameplay messages from participant ID:" + senderId);
	}

	public List<Participant> GetAllPlayers() 
	{
		return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants ();
	}

	public string GetMyParticipantId()
	{
		//return PlayGamesPlatform.Instance.TurnBased.;
		return PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId;
	}


	static byte[] GetBytes(string str)
	{
		byte[] bytes = new byte[str.Length * sizeof(char)];
		System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
		return bytes;
	}

	static string GetString(byte[] bytes)
	{
		char[] chars = new char[bytes.Length / sizeof(char)];
		System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
		return new string(chars);
	}

	public void SendMyUpdate(string message)
	{
		updateMessage.Clear();
		Debug.Log("SendMyUpdate " + message);
		byte[] messageToSend = GetBytes(message);

		PlayGamesPlatform.Instance.RealTime.SendMessageToAll (false, messageToSend);
	}


	
}
