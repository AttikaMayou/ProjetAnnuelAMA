using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Network;
using Photon.Pun;

//Auteur : Margot

public class AvatarAnimation : MonoBehaviour
{
    public bool debug = false;

    public CharacterExposerScript[] character;
    public AIntentReceiver[] intentReceivers;

    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected)
        {
            return;
        }

        if (intentReceivers == null || character == null)
        {
            Debug.LogError("[AvatarAnimation] " +
                           "There is something wrong with avatars animation and intents setup !");
            return;
        }

        var i = 0;
        for (; i < intentReceivers.Length; i++)
        {
            var intent = intentReceivers[i];

            character[i].characterAnimator.SetFloat("Forward", intent.forward);
            character[i].characterAnimator.SetFloat("Strafe", intent.strafe);

            if (intent.Move == true)
            {
                character[i].characterAnimator.SetBool("IsWalking", true);
            }
            else if(intent.Run == true)
            {
                character[i].characterAnimator.SetBool("IsRunning", true);
                
                if(debug == true)
                {
                    Debug.Log("run ? : {} " + intent.Run);
                }
            }
            else
            {
                character[i].characterAnimator.SetBool("IsWalking", false);
            }

            if(intent.Shoot)
            {
                character[i].characterAnimator.SetBool("IsShooting", true);

                if(intent.Shoot && intent.canShoot)
                {
                    character[i].characterAnimator.SetBool("ShootLoaded", true);
                }
            }
            else
            {
                character[i].characterAnimator.SetBool("IsShooting", false);
            }
        }
    }   
}
