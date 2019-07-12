using System.Collections;
using System.Collections.Generic;
using LastToTheGlobe.Scripts.Chest;
using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

//Auteur : Abdallah

public class ChestScript : MonoBehaviour
{
    private int _i;

    public bool Debug;

    public ChestExposerScript Exposer;
    public ChestContentManagerScript chestContentManagerScript;
    public bool Generated = false;

    

    public void AssignAndGen()
    {
        if(Generated) return;
        Generated = true;
        Exposer.ChestPhotonView.RPC("AssignChestRPC", RpcTarget.MasterClient, 0, 0);
        chestContentManagerScript.GenerateChestItem(Exposer.seedChest);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (_i == 1)
        {
            return;
        }
        _i = 1;

        if(Debug) UnityEngine.Debug.LogFormat("[ChestScript] {0} get triggered by something : {1}",
            this.gameObject.name, other.gameObject.name);
            
        //Only the Master Client interact with collider and stuff like this
        if (!PhotonNetwork.IsMasterClient) return;

        var playerId = ColliderDirectoryScript.Instance.GetPlayerId(other);

        
        
        //if playerId is different from -1, that means this is a player which near the chest
        if (playerId != -1)
        {
            //Send to MasterClient a message to warn him with its own ID and playerId
            Exposer.ChestPhotonView.RPC("AssignChestRPC", RpcTarget.MasterClient,
                Exposer.Id, playerId);
        }

        chestContentManagerScript.GenerateChestItem(Exposer.seedChest);

        StartCoroutine(ResetTrigger());
    }

    private void OnTriggerExit(Collider other)
    {
        if (_i == 1)
        {
            return;
        }
        _i = 1;
            
        if(Debug) UnityEngine.Debug.LogFormat("[ChestScript] {1} left {0}",
            this.gameObject.name, other.gameObject.name);
             
        //Only the Master Client interact with collider and stuff like this
        if (!PhotonNetwork.IsMasterClient) return;

        var playerId = ColliderDirectoryScript.Instance.GetPlayerId(other);

        //if playerId is different from -1, that means this is a player which left the chest
        if (playerId != -1)
        {
            //Send to MasterClient a message to warn him with its own ID and playerId
            Exposer.ChestPhotonView.RPC("UnassignChestRPC", RpcTarget.MasterClient,
                playerId);
        }
            
        StartCoroutine(ResetTrigger());
    }
    
    private IEnumerator ResetTrigger()
    {
        yield return new WaitForSeconds(0.2f);
        _i = 0;
    }
}
