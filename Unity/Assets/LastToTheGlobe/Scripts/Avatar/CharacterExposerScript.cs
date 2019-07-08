﻿using Assets.LastToTheGlobe.Scripts.Environment.Planets;
using Assets.LastToTheGlobe.Scripts.Management;
using LastToTheGlobe.Scripts.Inventory;
using LastToTheGlobe.Scripts.UI;
using Photon.Pun;
using UnityEngine;

//Auteur : Margot, Abdallah et Attika

namespace Assets.LastToTheGlobe.Scripts.Avatar
{
    public class CharacterExposerScript : Avatar
    {
        public static bool debug = true;

        //The id value of this player. Updated at awakening
        public int Id;
        
        public GameObject CharacterRootGameObject;
        
        [Header("Player Control Parameters")] 
<<<<<<< HEAD
        public Rigidbody characterRb;
        public Transform characterTr;
        public Collider characterCollider;
        public AvatarsController avatarsController;
=======
        public Rigidbody CharacterRb;
        public Transform CharacterTr;
        public Collider CharacterCollider;
        public HitPointComponent HitPointComponent;
        public CollisionEnterDispatcherScript CollisionDispatcher;

        [Header("Avatar Animation")]
        public Animator CharacterAnimator;
        
>>>>>>> master
        [Header("Network Parameters")]
        public PhotonView CharacterPhotonView;
        public PhotonRigidbodyView CharacterRbPhotonView;
        
        [Header("Camera Control Parameters")] 
        public GameObject CameraRotatorX;

        public AttractorScript AttractorDebug;
        
        [Header("UI references")] 
        //public ActivateObjects inventoryUI;
<<<<<<< HEAD
        public ActivateObjects lifeUI;
        public ActivateObjects victoryUI;
        public ActivateObjects defeatUI;
        public GameObject PlayerInventory;
        public GameObject ChestInventory;
        public GameObject Interaction;
=======
        public ActivateObjects LifeUi;
        public ActivateObjects VictoryUi;
        public ActivateObjects DefeatUi;
>>>>>>> master
        
        public InventoryScript InventoryScript;
        
        //Character Parameters
        public Vector3 Movedir;
        public float DashSpeed = 30;

        //Reference itself to the ColliderDirectory and CameraScript when activated
        private void OnEnable()
        {
            //only the Master Client add the player to the directory and get his ID
            if (!PhotonNetwork.IsMasterClient) return;
            if(debug) Debug.LogFormat("[CharacterExposer] OnEnable : {0}", this.gameObject.name);
            ColliderDirectoryScript.Instance.AddCharacterExposer(this, out Id);
        }
        
        //Dereference itself to the ColliderDirectory and CameraScript when deactivated
        private void OnDisable()
        {
            //only the Master Client remove the player to the directory and reset his ID
            if (!PhotonNetwork.IsMasterClient) return;
            if(debug) Debug.LogFormat("[CharacterExposer] OnDisable : {0}", this.gameObject.name);
            ColliderDirectoryScript.Instance.RemoveCharacterExposer(this);
        }

//        private void LateUpdate()
//        {
//            if (!debug) return;
//            AttractorDebug = Attractor;
//        }

        //TODO : Deactivate Rb when player is teleported
        public void DeactivateRb()
        {
            CharacterRb.isKinematic = true;
        }
    }
}
