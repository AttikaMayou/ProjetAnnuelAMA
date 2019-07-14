using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Environment.Planets;
using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Weapon.Orb
{
    public class OrbExposerScript : MonoBehaviour
    {
        public static bool Debug = true;

        //The id value of this orb. Updated at awakening
        public int id;

         
        [Header("Component References")] 
        public Transform orbTransform;
        public Rigidbody orbRb;
        public Collider orbCollider;
        public PhotonView orbsPhotonView;
        
        [Header("Gravity Parameters")]
        public AttractorScript Attractor;
        
        [Header("Player Parameters")] 
        public CharacterExposerScript playerExposer;
        
        private void OnEnable()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.AddOrbExposer(this, out id);
        }

        private void OnDisable()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.RemoveOrbExposer(this);
        }
    }
}
