using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

/*
Connect to the game and join/create a room.
Referring:
PUN overview
https://doc.photonengine.com/en-us/pun/v2/getting-started/pun-intro
Tutorial
https://doc.photonengine.com/en-us/pun/v2/demos-and-tutorials/pun-basics-tutorial/intro
API
https://doc-api.photonengine.com/en/pun/v2/class_photon_1_1_pun_1_1_utility_scripts_1_1_connect_and_join_random.html
API V2 migration
https://doc.photonengine.com/en-us/pun/v2/getting-started/migration-notes
*/
public class Main : MonoBehaviourPunCallbacks {

	public enum Status {Offline, Connecting, Joining, Creating, Rooming};

	public string room    = "The Room";
	public string avatar  = "Avatar";
	public bool   verbose = true;

	[Header("Informative")]
	public Status status = Status.Offline;

	// --------------------------------------------------------------------------

	void Start () {
		status = Status.Connecting;
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster(){
		status = Status.Joining;
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinRandomFailed(short returnCode, string message){
		status = Status.Creating;
		PhotonNetwork.CreateRoom(room);
	}

	public override void OnCreatedRoom(){
		// Don't care because we still receive OnJoinedRoom
	}

	public override void OnJoinedRoom(){
		status = Status.Rooming; Log("Joined room");
		if(avatar.Length>0){
			PhotonNetwork.Instantiate(avatar, Vector3.zero, Quaternion.identity, 0);
		}
	}

	public override void OnDisconnected(DisconnectCause cause){
		if(cause!=DisconnectCause.DisconnectByClientLogic)
			Err("Photon error: " + cause);
	}

	void Log(string x){ if(verbose) Debug.Log(x); }

	void Err(string x){ Debug.LogWarning(x); }

}
