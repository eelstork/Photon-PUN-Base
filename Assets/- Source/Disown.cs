using UnityEngine;
using Photon.Pun;

public class Disown : MonoBehaviour {

	public MonoBehaviour[] controllers;

	void Start(){
		if (!GetComponent<PhotonView>().IsMine){
			foreach(var c in controllers) Destroy(c);
		} Destroy(this);
	}

}
