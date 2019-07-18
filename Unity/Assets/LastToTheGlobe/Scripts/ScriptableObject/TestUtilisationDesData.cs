using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class TestUtilisationDesData : MonoBehaviour
{
   
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            //appel de la fonction RegisterEnnemyKill si on veut l'ajouter au data
            DataCollector.RegisterDeathByLayer(this);
            
        }
              
    }
}

[Serializable]
public class KillDataTrackerUnityEvent : UnityEvent<TestUtilisationDesData> { };