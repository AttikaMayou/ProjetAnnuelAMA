using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUtilisationDesData : MonoBehaviour
{
   
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            //appel de la fonction RegisterEnnemyKill si on veut l'ajouter au data
            DataCollector.RegisterEnnemyKillWithTime(this);
        }
              
    }
}
