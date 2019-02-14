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
        //TODO : Add methods to remove a CharacterExposer from the list and the directory dictionary
        
        [SerializeField]
        public List<CharacterExposer> characterExposers;
    
        private readonly Dictionary<Collider, CharacterExposer> _directory = new Dictionary<Collider, CharacterExposer>();
        private CharacterExposer _value;
        
        /// <summary>
        /// Get the player whom belongs to the collider
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public CharacterExposer GetExposer(Collider col)
        {
            if (_directory.TryGetValue(col, out _value))
            {
                return _value;
            }
            Debug.LogError("The Collider has no CharacterExposer associated : " +
                           "something wrong happened at instantiation of the player whom it belongs");
            return null;
        }

        /// <summary>
        /// Add player in the list of CharactersExposers
        /// </summary>
        /// <param name="player"></param>
        public void AddExposer(CharacterExposer player)
        {
            if (characterExposers == null)
            {
                characterExposers = new List<CharacterExposer>();
            }
            
            if (!characterExposers.Contains(player) && player)
            {
                characterExposers.Add(player);
            }
            
            AddPlayerInDirectory(player);
        }

        private void AddPlayerInDirectory(CharacterExposer player)
        {
            if (_directory.ContainsValue(player)) return;
            _directory.Add(player.characterCollider, player);
        }
    }
}
