using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Auteur : Abdallah

public class spellThrower : MonoBehaviour {

    [SerializeField]
    private GameObject testOrb;

    private bool canThrowSpell;
    
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A) && canThrowSpell)
        {
            canThrowSpell = false;
            testOrb.SetActive(true);
            

        }
        if (!testOrb.activeSelf)
        {
            canThrowSpell = true;
        }
    }
}
