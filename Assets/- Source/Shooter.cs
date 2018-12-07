using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

	public KeyCode key = KeyCode.Space;
	public GameObject beam;

	void Update () {
		if(Input.GetKeyDown(key)) Shoot();
		if(Input.GetKeyUp(key))   beam.SetActive(false);
	}

	void Shoot(){
		beam.SetActive(true);
		RaycastHit hit;
		bool didHit = Physics.Raycast(transform.position + Vector3.up*0.5f,
																  transform.forward, out hit, 10f);
		if(didHit){
			var that = hit.collider.GetComponent<ShootingTarget>();
			if(that)
				that.view.RPC("Hit", PhotonTargets.All, that.view.ownerId);
			else
				print("Shot a wall or something");
		}
	}

	PhotonView view{ get{ return GetComponent<PhotonView>(); }}

}
