using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Environment.Planets
{
    public class PlanetExposerScript : MonoBehaviour
    {
        public Transform planetTransform;
        public Collider planetCollider;

        private void OnEnable()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                ColliderDirectoryScript.Instance.AddPlanetExposer(this);
            }
        }
    }
}
