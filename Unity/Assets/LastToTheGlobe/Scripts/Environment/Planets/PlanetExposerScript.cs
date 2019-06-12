using Assets.LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

//Auteur : Attika

namespace Assets.LastToTheGlobe.Scripts.Environment.Planets
{
    public class PlanetExposerScript : MonoBehaviour
    {
        public Transform PlanetTransform;
        public Collider PlanetCollider;
        public AttractorScript AttractorScript;

        public bool IsSpawnPlanet;
        public Transform SpawnPosition;

        private void OnEnable()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                ColliderDirectoryScript.Instance.AddPlanetExposer(this);
            }
        }
    }
}
