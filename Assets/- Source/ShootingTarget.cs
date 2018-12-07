using UnityEngine;

public class ShootingTarget : MonoBehaviour {

  [PunRPC]
  public void DidShoot(int id){
    print(string.Format("RPC: Parameter: {0} ", id));
    var I = FindObjectOfType<Player>();
    I.GotHit(id);
  }

  PhotonView view{ get{ return GetComponent<PhotonView>(); }}

}
