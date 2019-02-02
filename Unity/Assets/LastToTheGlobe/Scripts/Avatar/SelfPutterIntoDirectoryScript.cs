using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Dev;
using UnityEngine;

//Auteur : Abdallah
public class SelfPutterIntoDirectoryScript : MonoBehaviour
{
    [SerializeField] private PlayerColliderDirectoryScript PlayerColliderDirectoryScript;
    [SerializeField] private CharacterExposer CharacterExposer;
        
    // Start is called before the first frame update
    void Awake()
    {
        PlayerColliderDirectoryScript.CharacterExposers.Add(CharacterExposer);
    }

    
}
