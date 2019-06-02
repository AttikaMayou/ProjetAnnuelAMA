using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel_Root_Rotator : MonoBehaviour
{
    public Transform rootTf;
    public Transform selfTf;

    // Update is called once per frame
    void Update()
    {
        selfTf.rotation = rootTf.rotation;
    }
}
