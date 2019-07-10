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
        public int Id;

        [Header("Component References")] 
        public Transform OrbTransform;
        public Rigidbody OrbRb;
        public Collider OrbCollider;
        public PhotonView OrbsPhotonView;

        private void OnEnable()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.AddOrbExposer(this, out Id);
        }

        private void OnDisable()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.RemoveOrbExposer(this);
        }
    }
}
