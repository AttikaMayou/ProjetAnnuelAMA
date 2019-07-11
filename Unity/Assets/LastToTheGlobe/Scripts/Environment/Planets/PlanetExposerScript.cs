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
        [FormerlySerializedAs("Id")] public int id;
        
        [FormerlySerializedAs("PlanetTransform")] public Transform planetTransform;
        [FormerlySerializedAs("PlanetCollider")] public Collider planetCollider;
        public Collider planetGroundCollider;
        [FormerlySerializedAs("AttractorScript")] public AttractorScript attractorScript;
        [FormerlySerializedAs("PlanetsPhotonView")] public PhotonView planetsPhotonView;

        [FormerlySerializedAs("IsSpawnPlanet")] public bool isSpawnPlanet;
        [FormerlySerializedAs("SpawnPosition")] public Transform spawnPosition;

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
            planetCollider.enabled = false;
        }

        public void ActivateCollider()
        {
            planetCollider.enabled = true;
        }
    }
}
