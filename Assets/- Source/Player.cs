using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public string prefabName;
	public GameObject owned;

	public void OnEnable(){
		owned = PhotonNetwork.Instantiate(
			prefabName, Vector3.zero, Quaternion.identity, 0);
	}

	public bool IsMine(int ownerId){
		var view = owned.GetComponent<PhotonView>();
		return view.ownerId==ownerId;
	}

	public void GotHit(int id){
		if(IsMine(id)) {
			print("I got shot");
			PhotonNetwork.Destroy(owned);
		} else print("wasn't me");
	}

}
