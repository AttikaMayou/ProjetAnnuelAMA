using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

namespace LastToTheGlobe.Scripts.Environment.Planets
{
    public class BumperExposerScript : MonoBehaviour
    {
        //The id value of this bumper. Updated at awakening
        public int Id;

        public Transform BumperTransform;
        public Collider BumperCollider;
        public BumpScript BumpScript;
        public PhotonView BumpersPhotonView;

        //Determines whether this bumper is on a spawn planet or not
        public bool IsSpawnBumper;
        //Determines whether A bumper from those on spawn planets has been used or not
        public static bool SpawnBumperUsed = false;
        //Determines whether THIS bumper, which is on a spawn planet, has been used or not
        public bool GetUsed;

        private void Awake()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.AddBumperExposer(this, out Id);
        }

        private void OnDestroy()
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
