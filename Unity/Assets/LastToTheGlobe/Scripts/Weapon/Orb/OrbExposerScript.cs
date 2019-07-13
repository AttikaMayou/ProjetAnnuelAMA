using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Weapon.Orb
{
    public class OrbExposerScript : MonoBehaviour
    {
        public static bool Debug = true;

        //The id value of this orb. Updated at awakening
        [FormerlySerializedAs("Id")] public int id;

        [FormerlySerializedAs("OrbTransform")] 
        [Header("Component References")] 
        public Transform orbTransform;
        [FormerlySerializedAs("OrbRb")] public Rigidbody orbRb;
        [FormerlySerializedAs("OrbCollider")] public Collider orbCollider;
        [FormerlySerializedAs("OrbsPhotonView")] public PhotonView orbsPhotonView;
        
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
