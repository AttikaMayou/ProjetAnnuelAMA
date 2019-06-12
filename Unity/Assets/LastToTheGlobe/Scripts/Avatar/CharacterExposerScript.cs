using Assets.LastToTheGlobe.Scripts.Environment.Planets;
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
        public bool debug = true;

        //The id value of this player. Updated at awakening
        public int Id;
        
        public GameObject CharacterRootGameObject;
        [Header("Player Control Parameters")] 
        public Rigidbody CharacterRb;
        public Transform CharacterTr;
        public Collider CharacterCollider;
        [Header("Network Parameters")]
        public PhotonView CharacterPhotonView;
        public PhotonRigidbodyView CharacterRbPhotonView;
        [Header("Camera Control Parameters")] 
        public GameObject CameraRotatorX;

        public AttractorScript AttractorDebug;
        
        [Header("UI references")] 
        //public ActivateObjects inventoryUI;
        public ActivateObjects LifeUi;
        public ActivateObjects VictoryUi;
        public ActivateObjects DefeatUi;
        
        public InventoryScript InventoryScript;
        
        //Character Parameters
        public Vector3 Movedir;
        public float DashSpeed = 30;

        //Reference itself to the ColliderDirectory and CameraScript when activated
        private void OnEnable()
        {
            //only the Master Client add the player to the directory and get his ID
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.AddCharacterExposer(this, out Id);
        }
        
        //Dereference itself to the ColliderDirectory and CameraScript when deactivated
        private void OnDisable()
        {
            //only the Master Client remove the player to the directory and reset his ID
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.RemoveCharacterExposer(this);
        }
    }
}
