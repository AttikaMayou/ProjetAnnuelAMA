using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LastToTheGlobe.Scripts.Dev;
using UnityEngine;

//Auteur : Abdallah
public class PlayerColliderDirectoryScript : MonoBehaviour
{
    [SerializeField]
    public List<CharacterExposer> CharacterExposers;
    
    private readonly Dictionary<Collider, CharacterExposer> directory = new Dictionary<Collider, CharacterExposer>();

    private void Awake()
    {
        foreach (var exposer in CharacterExposers)
        {
            directory.Add(exposer.Collider, exposer);
        }
    }

    public CharacterExposer GetExposer(Collider col)
    {
        return directory[col];
    }
}
