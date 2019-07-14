using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Environment.Planets
{
    public class PlanetExposerScript : MonoBehaviour
    {
        //The id value of this planet. Updated at awakening
        public int id;
        
        public Transform planetTransform;
        public Collider planetGravityField;
        public Collider planetGroundCollider;
        public AttractorScript attractorScript;
        public PhotonView planetsPhotonView;

        public bool isSpawnPlanet;
        public Transform spawnPosition;

        //Reference itself to the ColliderDirectory
        private void Awake()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.AddPlanetExposer(this, out id);
        }

        //Dereference itself to the ColliderDirectory
        private void OnDestroy()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.RemovePlanetExposer(this);
        }

        public void DeactivateCollider()
        {
            planetGravityField.enabled = false;
        }

        public void ActivateCollider()
        {
            planetGravityField.enabled = true;
        }
    }
}
