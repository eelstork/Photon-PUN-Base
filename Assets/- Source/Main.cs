using UnityEngine;

/*
Connect to the game and join/create a room.
Referring:
PUN overview
https://doc.photonengine.com/en-us/pun/current/getting-started/pun-intro
Tutorial
https://doc.photonengine.com/en-us/pun/current/demos-and-tutorials/pun-basics-tutorial/intro
API:
http://doc-api.photonengine.com/en/PUN/current/annotated.html
*/
public class Main : MonoBehaviour {

	public enum Status {Offline, Connecting, Joining, Creating, Rooming};

	public string photonVersion = "v1";
	public string room          = "The Room";
	public bool   verbose       = true;

	[Header("Informative")]
	public Status status = Status.Offline;

	// --------------------------------------------------------------------------

	void Start () {
		status = Status.Connecting;
		PhotonNetwork.ConnectUsingSettings(photonVersion);
	}

	void OnConnectedToMaster(){
		status = Status.Joining;
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed(){
		status = Status.Creating;
		PhotonNetwork.CreateRoom(room);
	}

  void OnCreatedRoom(){ /* Don't care*/ }

	void OnJoinedRoom(){
		status = Status.Rooming; Log("Joined room");
		FindObjectOfType<Player>().enabled = true;
	}

	void OnFailedToConnectToPhoton(DisconnectCause cause){
		Debug.Log ("Photon error: " + cause);
	}

	void Log(string x){ if(verbose) Debug.Log(x); }

	void Err(string x){ Debug.LogWarning(x); }

}
