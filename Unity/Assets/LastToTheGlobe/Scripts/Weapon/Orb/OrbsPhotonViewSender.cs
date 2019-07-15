using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

//Auteur : Attika et Abdallah

namespace Assets.LastToTheGlobe.Scripts.Weapon.Orb
{
    public class OrbsPhotonViewSender : MonoBehaviour
    {
        public bool debug = true;

        [PunRPC]
        public void InflictDamage(int idPlayerShooting, int idPlayerDamaged)
        {
            Debug.LogFormat("The Player {0} Damaged the player {1}", idPlayerShooting, idPlayerDamaged);
            
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(idPlayerDamaged);
            
            player.avatarLifeManager.InflictDamage();
        }
        
    }
}
