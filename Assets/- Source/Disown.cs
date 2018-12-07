using UnityEngine;

public class Disown : MonoBehaviour {

	public MonoBehaviour[] controllers;

	void Start(){
		if (!GetComponent<PhotonView>().isMine){
			foreach(var c in controllers) Destroy(c);
		} Destroy(this);
	}

}
