using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

//Auteur : Attika et Abdallah

namespace Assets.LastToTheGlobe.Scripts.Weapon.Orb
{
    public class OrbsPhotonViewSender : MonoBehaviour
    {
        public bool debug = true;
        
        //Erase this
        //public CharacterExposerScript player;

        [PunRPC]
        public void InflictDamage(int idPlayerShooting, int idPlayerDamaged)
        {
            Debug.LogFormat("The Player {0} Damaged the player {1}", idPlayerShooting, idPlayerDamaged);
            
            
            //dont forget to uncomment
            var player = ColliderDirectoryScript.Instance.GetCharacterExposer(idPlayerDamaged);
            print("The Player i want to hurt " +(idPlayerDamaged));
            print("The Player i am hurting" +player.Id);
            player.avatarLifeManager.InflictDamage();
            if (player.avatarLifeManager.myLife <= 0)
            {
                player.avatarLifeManager.alive = false;
                player.DefeatUi.Activation();
                player.CharacterRootGameObject.SetActive(false);
            }
        }
        
    }
}
