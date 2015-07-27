using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class UserDAO : IUserDAO {
	
	
	static string FilePath;
	const string LOCAL_FILE_NAME = "/playerSettings.dat";
	const string FRIENDS_LOCAL_FILE_NAME = "/playerFriends.dat";

	
	public UserDAO(){}
	
	public void CreateUser ()
	{
		throw new System.NotImplementedException ();
	}
	
	public User GetUser ()
	{
		throw new System.NotImplementedException ();
	}
	
	public void UpdateUser ()
	{
		throw new System.NotImplementedException ();
	}
	
	public void DeleteUser ()
	{
		throw new System.NotImplementedException ();
	}
	
	public void StoreUserOnLocalMemory (User userData)
	{
		BinaryFormatter bf = null;
		FileStream file = null;
		try{
			FilePath = Application.persistentDataPath + LOCAL_FILE_NAME;
			bf = new BinaryFormatter();
			file = new FileStream(FilePath, FileMode.OpenOrCreate);
			Debug.Log("Data save succesfully on StoreUserOnLocalMemory ");
			bf.Serialize(file,userData);
			file.Close();

		}
		catch (FileNotFoundException  ex)
		{
			Debug.Log("StoreUserOnLocalMemory:FileNotFoundException " + ex.ToString());
		}
		catch (IOException ex)
		{
			Debug.Log("StoreUserOnLocalMemory:IOException " + ex.ToString());
		}
		catch (Exception ex)
		{
			Debug.Log("StoreUserOnLocalMemory:Exception " + ex.ToString());
		}
		finally
		{
			if (file != null) 
				file.Close();
		}
	}
	
	public User LoadUserFromLocalMemory ()
	{
		BinaryFormatter bf = null;
		FileStream file = null;
		User user = new User();
		try{
			FilePath = Application.persistentDataPath + LOCAL_FILE_NAME;
			//Debug.Log("loadUserFromLocalMemory():FilePath " + FilePath);
			if(File.Exists(FilePath)){
				//Debug.Log("Yes File.Exists.....@ " + FilePath);
				bf = new BinaryFormatter();
				file = File.Open(FilePath,FileMode.Open);
				user = (User)bf.Deserialize(file);
			}
		}
		
		catch (FileNotFoundException  ex)
		{
			Debug.Log("FileNotFoundException:ex " + ex.ToString());
		}
		catch (IOException ex)
		{
			Debug.Log("IOException:ex " + ex.ToString());
		}
		catch (Exception ex){
			Debug.Log("Exception:ex " + ex.ToString());
		}
		finally
		{
			if (file != null) 
				file.Close();
		}
		return user;
	}


	public void SaveUserFriendsOnLocalMemory (User userData)
	{
		BinaryFormatter bf = null;
		FileStream file = null;
		try{
			FilePath = Application.persistentDataPath + FRIENDS_LOCAL_FILE_NAME;
			bf = new BinaryFormatter();
			file = new FileStream(FilePath, FileMode.OpenOrCreate);
			Debug.Log("Data save succesfully on StoreUserOnLocalMemory ");
			bf.Serialize(file,userData);
			file.Close();
			
		}
		catch (FileNotFoundException  ex)
		{
			Debug.Log("StoreUserOnLocalMemory:FileNotFoundException " + ex.ToString());
		}
		catch (IOException ex)
		{
			Debug.Log("StoreUserOnLocalMemory:IOException " + ex.ToString());
		}
		catch (Exception ex)
		{
			Debug.Log("StoreUserOnLocalMemory:Exception " + ex.ToString());
		}
		finally
		{
			if (file != null) 
				file.Close();
		}
	}

	public User LoadUserFriendsFromLocalMemory ()
	{
		BinaryFormatter bf = null;
		FileStream file = null;
		User user = new User();
		try{
			FilePath = Application.persistentDataPath + FRIENDS_LOCAL_FILE_NAME;
			Debug.Log("loadUserFromLocalMemory():FilePath " + FilePath);
			if(File.Exists(FilePath)){
				Debug.Log("Yes File.Exists.....@ " + FilePath);
				bf = new BinaryFormatter();
				file = File.Open(FilePath,FileMode.Open);
				user = (User)bf.Deserialize(file);
			}
		}
		
		catch (FileNotFoundException  ex)
		{
			Debug.Log("FileNotFoundException:ex " + ex.ToString());
		}
		catch (IOException ex)
		{
			Debug.Log("IOException:ex " + ex.ToString());
		}
		catch (Exception ex){
			Debug.Log("Exception:ex " + ex.ToString());
		}
		finally
		{
			if (file != null) 
				file.Close();
		}
		return user;
	}
	
}
