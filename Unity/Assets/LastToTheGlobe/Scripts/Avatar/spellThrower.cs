using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellThrower : MonoBehaviour {

    [SerializeField]
    private GameObject testOrb;

    private bool canThrowSpell;

	// Use this for initialization
	void Start () {
        //testOrb = Resources.Load<GameObject>("testOrb");
	}
	
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
