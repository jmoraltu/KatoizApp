using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	protected static T _instance;
	private static object _lock = new object();
	/**
      Returns the instance of this singleton.
   */
	public static T Instance
	{
		get
		{

			if (applicationIsQuitting) {
				Debug.LogWarning ("[Singleton] Instance '" + typeof(T) +
					"' already destroyed on application quit." +
					" Won't create again - returning null.");
				return null;
			}

			lock (_lock) {
				if (_instance == null) {
					_instance = (T)FindObjectOfType (typeof(T));
					
					if (FindObjectsOfType (typeof(T)).Length > 1) {
						Debug.LogError ("[Singleton] Something went really wrong " +
							" - there should never be more than 1 singleton!" +
							" Reopening the scene might fix it.");
						return _instance;
					}
					
					if (_instance == null) {
						GameObject singleton = new GameObject ();
						_instance = singleton.AddComponent<T> ();
						singleton.name = "(singleton) " + typeof(T).ToString ();
						
						DontDestroyOnLoad (singleton);
						
						Debug.Log ("[Singleton] An instance of " + typeof(T) + 
							" is needed in the scene, so '" + singleton +
							"' was created with DontDestroyOnLoad.");
					} else {
						Debug.Log ("[Singleton] Using instance already created: " +
							_instance.gameObject.name);
					}
				}
				
				return _instance;
			}
		}

	
	}

	private static bool applicationIsQuitting = false;

	public void OnDestroy () {
		applicationIsQuitting = true;
	}
}

//Another way to implement
/*public class Singleton<T> : GLMonoBehaviour where T : MonoBehaviour
	{
		protected static T instance;


			//Returns the instance of this singleton.

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (T)FindObjectOfType(typeof(T));
				
				if (instance == null)
				{
					Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
				}
			}
			
			return instance;
		}
	}
}*/
