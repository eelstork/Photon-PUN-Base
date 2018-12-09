This is a sample multiplayer project with support for PUN (Photon Unity
Networking) v1 and v2.

# Who is this for?

This is a crash course/refresher, not a tutorial.

- You already know how to program games using Unity 3D;
- You need a quick walkthrough to understand Photon networking.
- You'd like to migrate from PUN1 to PUN2 (check the
[migration notes][v2-migration] and [this diff][v1-to-v2]?)

# PUN in a nutshell

PUN provides peer-to-peer multiplayer as a service. This means that there is no
'master server'; therefore instead of a unique source of truth for what is
happening in the game, every client manages networked game objects (NGOs).

Each NGO synchronizes state (position, rotation, animation) and will message
session-wise clones via RPC (remote procedure call)

PUN provides its own 'Instantiate' and 'Destroy' methods thereby a client
creates/destroys NGOs, such as an avatar representing the player, or a
projectile.

# Getting started

Install either [PUN2][pun2] or [PUN classic][pun1].
At the time of writing PUN2 (rel. August 2018) looks ready for production use.

# Setting up credentials

Create/login [here][photon-home]; make a new app in the dashboard:

- Type: Photon Realtime
- Name: same as your unity project (keep it neat).
- URL: don't care

Copy your app id from the app's 'manage' section to your project's
`PhotonServerSettings`. Can't see `PhotonServerSettings`? Reload your project
and it should regenerate automatically.

TIP: add `PhotonServerSettings` to `.gitignore`: your app id is a secret which
shouldn't be shared.

# Creating/Joining a room

This [utility class][cs-main] exposes the flow of connecting to the Photon
server and creating/joining a room; added to any project it will create or join
an existing room. No UI makes it perfect for iterating your multiplayer
prototype.



# Setting up and configuring a player avatar.

- Create an `Avatar` game object; for this the sample uses a simple Blender
model.
- Add `PhotonView` to the avatar; this enables message passing and
state synchronization.
- Add `PhotonTransformView` or `PhotonTransformViewClassic`; these are
handy components which do the work of syncing position, rotation and scale.
Do check the boxes to sync position and rotation.
- Make `Avatar` into a prefab and place it in a `Resources` folder; then
deactivate `Avatar` in the scene (you could remove it but keeping clone in the
scene helps modify and inspect the prefab).

To instantiate the player avatar across the network, after joining a room,
call:

```
PhotonNetwork.Instantiate(avatar, Vector3.zero, Quaternion.identity, 0);
```

Note: the `Vector` and `Quaternion` are used to init the position and
rotation of the avatar.

# Adding controls

You can adapt single player controllers for multiplayer but since all scripts
are running on all clients, how do we know to control 'our' avatar vs everybody
else's? `PhotonView` provides the `IsMine` property, often demonstrated like
this:

```
if(photonView.isMine){
  // ... check input and move avatar
}
```

This pattern has disadvantages:
- Needs to be repeated in many places, which makes it error prone.
- Conflates multiplayer logic with control logic.

Instead, you might leverage a utility component like [`Disown`][cs-disown] to
remove the controller (and other functionality specific to the local avatar)
from un-owned NGOs.

# Interaction via RPC (remote procedure calls)

In this sample [Shooter][cs-shooter] implements firing at other avatars via
ray-cast. On success, instead of calling `Hit()` on the
[ShootingTarget][cs-shootingtarget], the function is messaged as an RPC:

```
that.view.RPC("Hit", RpcTarget.All, that.view.Owner.ActorNumber)
```

For this to work the target function must be tagged `[PunRPC]`;

```
[PunRPC] public void Hit(int id)
```

What's the difference between a procedure call (the usual thing) and a remote
procedure call?

- The procedure call (`that.GetComponent<ShootingTarget>().Hit()`) would only
get invoked locally.
- The RPC gets called on every clone of the receiving NGO.

Since NGOs are owned, only the 'original clone' can destroy its avatar. In
this case I didn't use `Disown`. If we removed `ShootingTarget` from other
clones, the RPC couldn't be handled, which would cause a PUN error; we'd
have to elaborate on the original pattern to make this work.

# Where to go from here?

This walkthrough is not exhaustive. If you enjoyed it, follow
[Eelstork][eelstork] on Unity Connect. 

Finally, official P.U.N resources:

- [Overview](https://doc.photonengine.com/en-us/pun/v2/getting-started/pun-intro)
- [Tutorial](https://doc.photonengine.com/en-us/pun/v2/demos-and-tutorials/pun-basics-tutorial)
- [API](https://doc-api.photonengine.com/en/pun/v2)

[cs-shootingtarget]: https://github.com/eelstork/Photon-PUN-Base/blob/master/Assets/-%20Source/ShootingTarget.cs
[cs-shooter]: https://github.com/eelstork/Photon-PUN-Base/blob/master/Assets/-%20Source/Shooter.cs
[cs-disown]: https://github.com/eelstork/Photon-PUN-Base/blob/master/Assets/-%20Source/Disown.cs
[cs-main]: https://github.com/eelstork/Photon-PUN-Base/blob/master/Assets/-%20Source/Main.cs
[eelstork]: https://connect.unity.com/u/588604d732b306001cd00baf
[photon-home]: https://www.photonengine.com
[pun1]: https://assetstore.unity.com/packages/tools/network/photon-unity-networking-classic-free-1786
[pun2]: https://assetstore.unity.com/packages/tools/network/pun-2-free-119922
[v1-to-v2]: https://github.com/eelstork/Photon-PUN-Base/commit/c7fe79cf3259d26e9dc7da2f1ab6f052e9a610b5
[v2-migration]: https://doc.photonengine.com/en-us/pun/v2/getting-started/migration-notes
