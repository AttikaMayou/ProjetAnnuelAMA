using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Abdallah

namespace LastToTheGlobe.Scripts.Avatar
{
    public class PlayerColliderDirectoryScript : MonoBehaviour
    {
        [SerializeField]
        public List<CharacterExposer> characterExposers;
    
        private readonly Dictionary<Collider, CharacterExposer> directory = new Dictionary<Collider, CharacterExposer>();

        private void Awake()
        {
            if (characterExposers == null)
            {
                Debug.LogError("CharacterExposers list is empty");
                return;
            }
            foreach (var exposer in characterExposers)
            {
                directory.Add(exposer.playerCollider, exposer);
            }
        }

        public CharacterExposer GetExposer(Collider col)
        {
            return directory?[col];
        }
    }
}
