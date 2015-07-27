using UnityEngine;
using System.Collections;
using System.IO;

interface IUserDAO {
	void CreateUser();
	User GetUser();
	void UpdateUser();
	void DeleteUser();
 	void StoreUserOnLocalMemory(User user);
	User LoadUserFromLocalMemory();
}
