using UnityEngine;
using System.Collections;


//Class definition
public class MainController : MonoBehaviour {
	
	
	private static MainController mainController;
	
	private string currentSceneName;
	private string nextSceneName;
	private AsyncOperation resourceUnloadTask;
	private AsyncOperation sceneLoadTask;
	private enum SceneState {Reset,Preload,Load,Unload,PostLoad,Ready,Run,Count};
	private SceneState sceneState;
	private delegate void UpdateDelegate();
	private UpdateDelegate[] updateDelegates;
	public static string AppCurrentLanguage;
	public static string previousScene;
	public const string MENU_SCENE = "MenuScene";
	public const string GAMEBOARD_SCENE = "GameBoardScene";
	public const string SETTINGS_SCENE = "SettingsScene";
	public const string RULES_SCENE = "RulesScene";
	public const string FRIENDS_SCENE = "FriendsScene";
	public const string RANKING_SCENE = "LeaderBoardScene";
	public const string EASY_LEVEL = "Easy";
	public const string NORMAL_LEVEL = "Normal";
	public const string HIGH_LEVEL = "High";
	public const string HARD_LEVEL = "Hard";

	
	
	//public static methods
	public static void SwitchScene(string nextSceneName)
	{
		MainController.previousScene = mainController.currentSceneName;

		if(mainController != null){
			if(mainController.currentSceneName != nextSceneName){
				mainController.nextSceneName = nextSceneName;
			}
		}
	}
	
	public static void SetAppLanguage(){
		if(Language.CURRENT_LANG != string.Empty)
			AppCurrentLanguage = Language.CURRENT_LANG;
		else
			AppCurrentLanguage = Application.systemLanguage.ToString();
	}
	
	//protected Mono methods
	protected void Awake(){
		Object.DontDestroyOnLoad(gameObject);
		SetAppLanguage();
		RegisterUserDeviceOnServer ();
		GooglePlayGameController.Instance.Activate ();
		GoogleMobileAdsController.Instance.Activate ();

		mainController = this;
		updateDelegates = new UpdateDelegate[(int) SceneState.Count];
		//Set each updateDelegate
		updateDelegates[(int) SceneState.Reset] = UpdateSceneReset;
		updateDelegates[(int) SceneState.Preload] = UpdateScenePreload;
		updateDelegates[(int) SceneState.Load] = UpdateSceneLoad;
		updateDelegates[(int) SceneState.Unload] = UpdateSceneUnload;
		updateDelegates[(int) SceneState.PostLoad] = UpdateScenePostLoad;
		updateDelegates[(int) SceneState.Ready] = UpdateSceneReady;
		updateDelegates[(int) SceneState.Run] = UpdateSceneRun;
		//updateDelegates[(int) SceneState.Count] = UpdateSceneCount;
		nextSceneName = MENU_SCENE;
		sceneState  = SceneState.Reset;
	}
	
	protected void OnDestroy(){
		//Clean up all the delegates
		if(updateDelegates != null)
		{
			for(int i= 0; i< (int)SceneState.Count; i++){
				updateDelegates[i] = null;
			}
			updateDelegates = null;
		}
		//Clean up the singleton instance
		if(mainController != null){
			mainController = null;
		}
	}
	
	protected void Update(){
		
		if(updateDelegates[(int)sceneState] !=null){
			updateDelegates[(int)sceneState]();
		}
	}
	
	//attach the new scene controller to start cascade of loading
	private void UpdateSceneReset(){
		System.GC.Collect();
		sceneState = SceneState.Preload;
	}
	
	//handle anything that needs to happen before loading
	private void UpdateScenePreload ()
	{
		sceneLoadTask = Application.LoadLevelAsync(nextSceneName);
		sceneState = SceneState.Load;
	}
	
	//Show the loading screen until its loaded
	private void UpdateSceneLoad ()
	{
		if(sceneLoadTask.isDone == true){
			sceneState = SceneState.Unload;
		}
		else{
			//update scene loading progress
		}
	}
	
	//
	private void UpdateSceneUnload ()
	{
		//clean up resources
		if(resourceUnloadTask == null){
			resourceUnloadTask = Resources.UnloadUnusedAssets();
		}
		else{
			if(resourceUnloadTask.isDone== true){
				resourceUnloadTask = null;
				sceneState = SceneState.PostLoad;
			}
		}
		
	}
	
	private void UpdateScenePostLoad ()
	{
		currentSceneName = nextSceneName;
		sceneState = SceneState.Ready;
	}
	
	private void UpdateSceneReady ()
	{
		//run a garbage collector pass
		System.GC.Collect();
		sceneState = SceneState.Run;
	}
	
	//Wait for scene change
	private void UpdateSceneRun ()
	{
		if(currentSceneName != nextSceneName){
			sceneState = SceneState.Reset;
		}
	}
	
	private void OnFailedToConnect(NetworkConnectionError error) {
		Debug.Log("Could not connect to server: " + error);
	}
	
	void RegisterUserDeviceOnServer(){
		var device = new Device ();
		string deviceId = device.DeviceId;
		Debug.Log("deviceId: " + deviceId + " device Model: " + device.Model + " device UniqueIdentifier: " + device.UniqueIdentifier);
		WebServiceAdapter wsa = new WebServiceAdapter ();
		StartCoroutine (wsa.GetUserByKey(device.DeviceId, Application.systemLanguage.ToString()));
	}
}
