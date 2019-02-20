using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Auteur: Margot

public class UIVictory : MonoBehaviour
{
    [SerializeField] private Canvas victory;
    [SerializeField] private Canvas defeat;


    // Start is called before the first frame update
    void Start()
    {
        victory.enabled = false;
        defeat.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
