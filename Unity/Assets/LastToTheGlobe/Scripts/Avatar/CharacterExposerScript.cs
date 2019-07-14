using Assets.LastToTheGlobe.Scripts.Avatar;
using Assets.LastToTheGlobe.Scripts.Management;
using LastToTheGlobe.Scripts.Environment.Planets;
using LastToTheGlobe.Scripts.Inventory;
using LastToTheGlobe.Scripts.Management;
using LastToTheGlobe.Scripts.UI;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Margot, Abdallah et Attika

namespace LastToTheGlobe.Scripts.Avatar
{
    public class CharacterExposerScript : MonoBehaviour
    {
        public static bool Debug = false;

        //The id value of this player. Updated at awakening
        [FormerlySerializedAs("Id")] public int id;
        
        [FormerlySerializedAs("CharacterRootGameObject")] [Header("Player Component References")] 
        public GameObject characterRootGameObject;
        [FormerlySerializedAs("CharacterRb")] public Rigidbody characterRb;
        [FormerlySerializedAs("CharacterTr")] public Transform characterTr;
        [FormerlySerializedAs("CharacterCollider")] public Collider characterCollider;
        [FormerlySerializedAs("HitPointComponent")] public HitPointComponent hitPointComponent;
        [FormerlySerializedAs("CollisionDispatcher")] public CollisionEnterDispatcherScript collisionDispatcher;

        [FormerlySerializedAs("CharacterAnimator")] [Header("Avatar Animation")]
        public Animator characterAnimator;

        [FormerlySerializedAs("Attractor")] [Header("Planets Reference")]
        public AttractorScript attractor;
        
        [FormerlySerializedAs("Bumper")] [Header("Bumper Reference")] 
        public BumpScript bumper;

        [FormerlySerializedAs("Chest")] [Header("Chest Reference")] 
        public ChestScript chest;
        
        [FormerlySerializedAs("CharacterPhotonView")] [Header("Network Parameters")]
        public PhotonView characterPhotonView;
        [FormerlySerializedAs("CharacterRbPhotonView")] public PhotonRigidbodyView characterRbPhotonView;
        
        
        [FormerlySerializedAs("CameraRotatorX")] [Header("Camera Control Parameters")] 
        public GameObject cameraRotatorX;

        [FormerlySerializedAs("LifeUi")] [Header("UI references")] 
        //public ActivateObjects inventoryUI;
        public ActivateObjects lifeUi;
        [FormerlySerializedAs("VictoryUi")] public ActivateObjects victoryUi;
        [FormerlySerializedAs("DefeatUi")] public ActivateObjects defeatUi;
        [FormerlySerializedAs("Interaction")] public Canvas interaction;
        public Canvas inventory;
        public Canvas chestCanvas;

        [FormerlySerializedAs("InventoryExposer")] [Header("Inventory references")]
        public PlayerInventoryExposer inventoryExposer;
        public InventoryScript inventoryScript;

        //Reference itself to the ColliderDirectory and CameraScript when activated
        private void OnEnable()
        {
            //only the Master Client add the player to the directory and get his ID
            if (!PhotonNetwork.IsMasterClient) return;
            if(Debug) UnityEngine.Debug.LogFormat("[CharacterExposer] OnEnable : {0}", this.gameObject.name);
            ColliderDirectoryScript.Instance.AddCharacterExposer(this, out id);
            inventoryExposer.playerId = id;

        }


        //Dereference itself to the ColliderDirectory and CameraScript when deactivated
        private void OnDisable()
        {
            //only the Master Client remove the player to the directory and reset his ID
            if (!PhotonNetwork.IsMasterClient) return;
            if(Debug) UnityEngine.Debug.LogFormat("[CharacterExposer] OnDisable : {0}", this.gameObject.name);
            ColliderDirectoryScript.Instance.RemoveCharacterExposer(this);
        }

        public void DeactivateRb()
        {
            characterRb.isKinematic = true;
        }

        public void ActivateRb()
        {
            characterRb.isKinematic = false;
        }

        public void DisableGravity()
        {
            characterRb.useGravity = false;
        }

        public void EnableGravity()
        {
            characterRb.useGravity = true;
        }
    }
}
