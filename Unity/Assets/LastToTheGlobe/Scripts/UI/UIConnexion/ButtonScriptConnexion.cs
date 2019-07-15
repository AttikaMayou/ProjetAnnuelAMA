using System.Collections;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class ButtonScriptConnexion : MonoBehaviour
{
    public List<GameObject> toEnable;
    public List<GameObject> toDisable;

    public void OnClick()
    {
        foreach (var item in toEnable)
        {
            item.SetActive(true);
        }
        
        foreach (var item in toDisable)
        {
            item.SetActive(false);
        }
    }
}
