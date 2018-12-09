using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour {

	public string prefabName;

	public void OnEnable(){
	  PhotonNetwork.Instantiate(
			prefabName, Vector3.zero, Quaternion.identity, 0);
	}

}
