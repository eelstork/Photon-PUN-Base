using UnityEngine;
using Photon.Pun;

public class ShootingTarget : MonoBehaviour {

  [PunRPC]
  public void Hit(int id){
    if(!view.IsMine)return;
    print("Destroy our avatar");
    PhotonNetwork.Destroy(gameObject);
  }

  public PhotonView view{ get{ return GetComponent<PhotonView>(); }}

}
