using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Network;
using Photon.Pun;

//Auteur : Margot

public class AvatarAnimation : MonoBehaviour
{
    public bool debug = false;

    public CharacterExposerScript[] character;
    public AIntentReceiver[] intentReceivers;


    void Update()
    {

        if (intentReceivers == null || character == null)
        {
            Debug.LogError("[AvatarAnimation] " +
                           "There is something wrong with avatars animation and intents setup !");
            return;
        }

        for (int i = 0; i < intentReceivers.Length; i++)
        {
            var intent = intentReceivers[i];

            character[i].CharacterAnimator.SetFloat("Forward", intent.Forward);
            character[i].CharacterAnimator.SetFloat("Strafe", intent.Strafe);

            if (intent.Move == true)
            {
                character[i].CharacterAnimator.SetBool("IsWalking", true);
            }
            /*else if(intent.Run == true)
            {
                character[i].CharacterAnimator.SetBool("IsRunning", true);
            }*/
           else
            {
                character[i].CharacterAnimator.SetBool("IsWalking", false);
            }

            if(intent.Shoot)
            {
                character[i].CharacterAnimator.SetBool("IsShooting", true);

                if(intent.Shoot && intent.CanShoot)
                {
                    character[i].CharacterAnimator.SetBool("ShootLoaded", true);
                }
            }
            else
            {
                character[i].CharacterAnimator.SetBool("IsShooting", false);
            }
        }
    }   
}
