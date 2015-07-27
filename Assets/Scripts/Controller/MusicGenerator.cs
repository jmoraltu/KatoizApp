using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MusicGenerator : MonoBehaviour {

	public GameObject Music;
	// Use this for initialization
	void Start () {
		UserDAO userDAO = new UserDAO();
		User userData = userDAO.LoadUserFromLocalMemory();

		GameObject MusicObject = GameObject.FindGameObjectWithTag("MusicBackground");

		if(!MusicObject){
			GameObject musicClone = (GameObject)Instantiate(Music, Vector3.zero, Quaternion.identity);
			AudioSource audioMusic = musicClone.GetComponent<AudioSource>();
			//Por defecto las variables de tipo bool son false
			Debug.Log("MusicGenerator:user.EnableMusic(): " + userData.EnableMusic); 
			if(userData.EnableMusic){
				audioMusic.Play();
			}else{
				audioMusic.Stop();
			}

		}
	}
}
