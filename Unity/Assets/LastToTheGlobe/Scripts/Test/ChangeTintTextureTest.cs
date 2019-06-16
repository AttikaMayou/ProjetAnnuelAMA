using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Assets.LastToTheGlobe.Scripts.Avatar;

public class ChangeTintTextureTest : MonoBehaviour
{
    private Material mat;
    [SerializeField]
    private Color color;

    void Start()
    {
        mat = GetComponent<Renderer>().material;

        if(PhotonNetwork.IsMasterClient)
        {
            mat.SetColor("_Color", color);
        }
    }
}
