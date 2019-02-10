using LastToTheGlobe.Scripts.Singleton;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Avatar
{
    public class IndexAttribution : MonoBehaviourSingleton<IndexAttribution>
    {
         
        public static int AttributeIndexToPlayers()
        {
//            var i = 0;
//            for (; i < PlayerNumbering.SortedPlayers.Length; i++)
//            {
//                if (PhotonNetwork.LocalPlayer.ActorNumber == PlayerNumbering.SortedPlayers[i].ActorNumber)
//                {
//                    return i;
//                }
//            }
            return 0;
        }
    }
}
