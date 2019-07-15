using Assets.LastToTheGlobe.Scripts.Avatar;
using Assets.LastToTheGlobe.Scripts.Management;
using LastToTheGlobe.Scripts.Environment.Planets;
using LastToTheGlobe.Scripts.Inventory;
using LastToTheGlobe.Scripts.Management;
using LastToTheGlobe.Scripts.UI;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

//Auteur : Margot, Abdallah et Attika

namespace LastToTheGlobe.Scripts.Avatar
{
    public class CharacterExposerScript : MonoBehaviour
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

        [Header("Planets Reference")]
        public AttractorScript Attractor;
        
        [Header("Bumper Reference")] 
        public BumpScript Bumper;

        [Header("Chest Reference")] 
        public ChestScript Chest;
        
        [Header("Network Parameters")]
        public PhotonView CharacterPhotonView;
        public PhotonRigidbodyView CharacterRbPhotonView;

        [Header("LifeManager References")] 
        public AvatarLifeManager avatarLifeManager;

        [Header("Color Preferences")] public Color colorPreferences;
        
        
        [Header("Camera Control Parameters")] 
        public GameObject CameraRotatorX;

        [Header("UI references")] 
        //public ActivateObjects inventoryUI;
        public ActivateObjects LifeUi;
        public ActivateObjects VictoryUi;
        public ActivateObjects DefeatUi;
        public Canvas Interaction;
        public Canvas inventory;
        public Canvas chest;

        [Header("Inventory references")]
        public PlayerInventoryExposer InventoryExposer;
        public InventoryScript inventoryScript;
        


        private void Start()
        {
            switch (PlayerPreferences.colorSelected)
            {
                default:
                    colorPreferences = Color.white;
                    break;
                case "red":
                    colorPreferences = Color.red;
                    break;
                
                case "black":
                    colorPreferences = Color.black;
                    break;
                
                case "yellow":
                    colorPreferences = Color.yellow;
                    break;
                
                case "green":
                    colorPreferences = Color.green;
                    break;
                
                case "magenta":
                    colorPreferences = Color.magenta;
                    break;
                
                case "blue":
                    colorPreferences = Color.blue;
                    break;
            }
            /*inventory.SetActive(false);
            chest.SetActive(false);*/
        }

        //Reference itself to the ColliderDirectory and CameraScript when activated
        private void OnEnable()
        {
            //only the Master Client add the player to the directory and get his ID
            if (!PhotonNetwork.IsMasterClient) return;
            if(debug) Debug.LogFormat("[CharacterExposer] OnEnable : {0}", this.gameObject.name);
            ColliderDirectoryScript.Instance.AddCharacterExposer(this, out Id);
            InventoryExposer.playerId = Id;

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
        }
    }
}
