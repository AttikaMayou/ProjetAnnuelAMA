using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Assets.LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Avatar;

public class ChangeTintTexture : MonoBehaviour
{
    private Material mat;
    private Color color;
    
    [SerializeField]
    private CharacterExposerScript[] players;

    void Start()
    {
        mat = GetComponent<Renderer>().material;

        if (PhotonNetwork.IsMasterClient)
        {
            mat.SetColor("_Color", Color.red);
        }

        /*else
        {
            for (int i = 1; i < players.Length; i++)
            {
                mat.SetColor("_Color", new Color(0.1f * players[i].Id, 0.1f * players[i].Id, 0.1f * players[i].Id, 1));
            }
        }*/
    }
    /*
    [PunRPC]
    public void setColor(int c)
    {
        if (c == 0)
        {
            players.GetComponent<Renderer>().material = Color.red;
        }
        else
        {
            players.GetComponent<Renderer>().material = Color.blue;
        }

    }
    */
}
