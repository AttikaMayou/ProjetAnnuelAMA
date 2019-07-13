using Assets.LastToTheGlobe.Scripts.Avatar;
using Assets.LastToTheGlobe.Scripts.Management;
using LastToTheGlobe.Scripts.Environment.Planets;
using LastToTheGlobe.Scripts.Inventory;
using LastToTheGlobe.Scripts.Management;
using LastToTheGlobe.Scripts.UI;
using Photon.Pun;
using UnityEngine;

//Auteur : Margot, Abdallah et Attika

namespace LastToTheGlobe.Scripts.Avatar
{
    public class CharacterExposerScript : Avatar
    {
        public static bool debug = false;

        //The id value of this player. Updated at awakening
        public int Id;
        
        [Header("Player Component References")] 
        public GameObject CharacterRootGameObject;
        public Rigidbody CharacterRb;
        public Transform CharacterTr;
        public Collider CharacterCollider;
        public HitPointComponent HitPointComponent;
        public CollisionEnterDispatcherScript CollisionDispatcher;

        [Header("Avatar Animation")]
        public Animator CharacterAnimator;

        [Header("Bumper Reference")] 
        public BumpScript Bumper;

        [Header("Chest Reference")] 
        public ChestScript Chest;
        public int seedChest;
        
        [Header("Network Parameters")]
        public PhotonView CharacterPhotonView;
        public PhotonRigidbodyView CharacterRbPhotonView;
        
        [Header("Camera Control Parameters")] 
        public GameObject CameraRotatorX;

//        [Header("UI references")] 
//        //public ActivateObjects inventoryUI;
//        public ActivateObjects LifeUi;
//        public ActivateObjects VictoryUi;
//        public ActivateObjects DefeatUi;
//
//        public GameObject Interaction;
//        public GameObject ChestInventory;
//        public GameObject PlayerInventory;
//        
//        public InventoryScript InventoryScript;
        
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

        public void DeactivateRb()
        {
            CharacterRb.isKinematic = true;
        }

        public void ActivateRb()
        {
            CharacterRb.isKinematic = false;
            CharacterRb.useGravity = true;
        }

        public void DisableGravity()
        {
            CharacterRb.useGravity = false;
        }

        public void EnableGravity()
        {
            CharacterRb.useGravity = true;
        }
    }
}
