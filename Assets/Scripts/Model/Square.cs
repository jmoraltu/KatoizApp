using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Square : MonoBehaviour{

	string id;
	Vector3 position;
	Quaternion rotation;
	List<Piece> pieces; //MAX_SIZE 5
	int posX;
	int posY;

	public Square(){}
}
