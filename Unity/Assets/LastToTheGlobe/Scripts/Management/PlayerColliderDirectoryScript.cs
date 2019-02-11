using System.Collections.Generic;
using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Singleton;
using UnityEngine;

//Auteur : Abdallah
//Modification : Attika

namespace LastToTheGlobe.Scripts.Management
{
    public class PlayerColliderDirectoryScript : MonoBehaviourSingleton<PlayerColliderDirectoryScript>
    {
        [SerializeField]
        public List<CharacterExposer> characterExposers;
    
        private readonly Dictionary<Collider, CharacterExposer> _directory = new Dictionary<Collider, CharacterExposer>();

        /// <summary>
        /// Get the player whom belongs the collider
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public CharacterExposer GetExposer(Collider col)
        {
            return _directory?[col];
        }

        /// <summary>
        /// Add player in the list of CharactersExposers
        /// </summary>
        /// <param name="player"></param>
        public void AddExposer(CharacterExposer player)
        {
            characterExposers.Add(player);
            AddPlayerInDirectory(player);
        }

        private void AddPlayerInDirectory(CharacterExposer player)
        {
            _directory.Add(player.playerCollider, player);
        }
    }
}
