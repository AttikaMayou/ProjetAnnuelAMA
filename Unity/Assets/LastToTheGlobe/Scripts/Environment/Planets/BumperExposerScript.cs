﻿using Assets.LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

namespace Assets.LastToTheGlobe.Scripts.Environment.Planets
{
    public class BumperExposerScript : MonoBehaviour
    {
        //The id value of this bumper. Updated at awakening
        public int Id;

        public Collider BumperCollider;
        public BumpScript BumpScript;

        //Determines whether this bumper is on a spawn planet or not
        public bool IsSpawnBumper;
        //Determines whether A bumper from those on spawn planets has been used or not
        public static bool SpawnBumperUsed = false;
        //Determines whether THIS bumper, which is on a spawn planet, has been used or not
        public bool GetUsed;

        private void OnEnable()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.AddBumperExposer(this, out Id);
        }

        private void OnDisable()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.RemoveBumperExposer(this);
        }

        public void DeactivateBumper()
        {
            BumperCollider.enabled = false;
        }

        public void ActivateBumper()
        {
            BumperCollider.enabled = true;
        }
    }
}
