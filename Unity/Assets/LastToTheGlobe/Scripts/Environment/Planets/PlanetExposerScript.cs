﻿using Assets.LastToTheGlobe.Scripts.Management;
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

        public bool IsSpawnPlanet;
        public Transform SpawnPosition;

        //TODO : make this on awake
        //Reference itself to the ColliderDirectory
        private void OnEnable()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.AddPlanetExposer(this, out Id);
        }
        
        //TODO : make this on destroy
        //Dereference itself to the ColliderDirectory
        private void OnDisable()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.RemovePlanetExposer(this);
        }
    }
}
