using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerObject : NetworkBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        //check if I own this player
        if (isLocalPlayer == false)
        {
            //doesnt belongs to me
            return;
        }
        //PlayerConnection needs to add a visible player
        //Command the server to spawn the unit
        CmdSpawnMyUnit();
    }
    public GameObject PlayerUnitPrefab;
    // Update is called once per frame
    void Update()
    {

    }

    //Commands are functions that run on the server
    [Command]
    void CmdSpawnMyUnit()
    {
        GameObject ob = Instantiate(PlayerUnitPrefab);//change for specific class
        //ob.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
        //Tells the clients aboit the spawn object
        NetworkServer.SpawnWithClientAuthority(ob, connectionToClient);

    }

}