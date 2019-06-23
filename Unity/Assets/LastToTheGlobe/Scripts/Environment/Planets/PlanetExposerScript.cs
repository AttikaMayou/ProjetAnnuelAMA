using Assets.LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

//Auteur : Attika

namespace Assets.LastToTheGlobe.Scripts.Environment.Planets
{
    public class PlanetExposerScript : MonoBehaviour
    {
        //The id value of this planet. Updated at awakening
        public int Id;
        
        public Transform PlanetTransform;
        public Collider PlanetCollider;
        public AttractorScript AttractorScript;
        public PhotonView PlanetsPhotonView;

        public bool IsSpawnPlanet;
        public Transform SpawnPosition;

        //Reference itself to the ColliderDirectory
        private void Awake()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.AddPlanetExposer(this, out Id);
        }

        //Dereference itself to the ColliderDirectory
        private void OnDestroy()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.RemovePlanetExposer(this);
        }

        public void DeactivateCollider()
        {
            PlanetCollider.enabled = false;
        }

        public void ActivateCollider()
        {
            PlanetCollider.enabled = true;
        }
    }
}
