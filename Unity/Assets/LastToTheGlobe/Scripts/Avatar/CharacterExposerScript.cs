﻿using UnityEngine;
using Photon.Pun;

//Auteur : Margot, Abdallah et Attika

namespace LastToTheGlobe.Scripts.Avatar
{
    public class CharacterExposerScript : MonoBehaviour
    {
        public GameObject characterRootGameObject;
        [Header("Player Control Parameters")] 
        public Rigidbody characterRb;
        public Transform characterTr;
        //public Bumper bumperScript;
        //public AttractedScript selfPlayerAttractedScript;
        //public AttractedScript selfOrbAttractedScript;
        public Collider characterCollider;
        [Header("Network Parameters")]
        public PhotonView characterPhotonView;
        public PhotonRigidbodyView characterRbPhotonView;
        [Header("Camera Control Parameters")] 
        public Transform cameraRotateX;

        //Reference itself to the ColliderDirectory and CameraScript when activated
        private void OnEnable()
        {
            //only the Master Client and the player to the directory
            if (PhotonNetwork.IsMasterClient)
            {
                //TODO : add this script to the directory
            }
        }
    }
}