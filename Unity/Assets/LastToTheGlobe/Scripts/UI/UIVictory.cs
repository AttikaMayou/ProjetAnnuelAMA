using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using LastToTheGlobe.Scripts.Avatar;

//Auteur: Margot

public class UIVictory : MonoBehaviour
{
    [SerializeField] private Canvas victory;
    [SerializeField] private Canvas defeat;
    [SerializeField] private AvatarLifeManager myLife;
    // Start is called before the first frame update
    void Start()
    {
        victory.enabled = false;
        defeat.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
//        if(myLife.myLife <= 0)
//        {
//            defeat.enabled = true;
//            victory.enabled = false;
//        }
//        
//        if(PhotonNetwork.PlayerList.Length == 1)
//        {
//            victory.enabled = true;
//            defeat.enabled = false;
//        }
    }
}
