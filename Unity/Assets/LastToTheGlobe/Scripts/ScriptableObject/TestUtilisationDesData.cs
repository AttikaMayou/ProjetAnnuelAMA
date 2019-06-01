using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUtilisationDesData : MonoBehaviour
{
   
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            DataCollector.RegisterEnnemyKillWithTime(this);
            Debug.Log("ajout de Data");
        }
              
    }
}
