using Assets.LastToTheGlobe.Scripts.Environment.Planets;
using Assets.LastToTheGlobe.Scripts.Management;
using LastToTheGlobe.Scripts.Inventory;
using LastToTheGlobe.Scripts.UI;
using Photon.Pun;
using UnityEngine;

//Auteur : Margot, Abdallah et Attika

namespace Assets.LastToTheGlobe.Scripts.Avatar
{
    public class CharacterExposerScript : global::LastToTheGlobe.Scripts.Avatar.Avatar
    {
        public bool debug = true;

        public int id;
        
        public GameObject characterRootGameObject;
        [Header("Player Control Parameters")] 
        public Rigidbody characterRb;
        public Transform characterTr;
        public Collider characterCollider;
        [Header("Network Parameters")]
        public PhotonView characterPhotonView;
        public PhotonRigidbodyView characterRbPhotonView;
        [Header("Camera Control Parameters")] 
        public GameObject cameraRotatorX;

        public AttractorScript attractorDebug;
        
        [Header("UI references")] 
        //public ActivateObjects inventoryUI;
        public ActivateObjects lifeUI;
        public ActivateObjects victoryUI;
        public ActivateObjects defeatUI;
        
        public InventoryScript inventoryScript;
        
        //Character Parameters
        public Vector3 _movedir;
        public float dashSpeed = 30;

        //Reference itself to the ColliderDirectory and CameraScript when activated
        private void OnEnable()
        {
            //only the Master Client and the player to the directory
            if (PhotonNetwork.IsMasterClient)
            {
                ColliderDirectoryScript.Instance.AddCharacterExposer(this);
            }
        }

        //Give the player his ID
        public void SetId(int value)
        {
            id = value;
        }
        
        private void LateUpdate()
        {
            if(debug)
                attractorDebug = attractor;
        }
    }
}
