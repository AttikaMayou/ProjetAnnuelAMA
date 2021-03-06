﻿using System;
using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Management;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Weapon.Orb
{
    public class OrbDefensiveScript : MonoBehaviour
    {
        [SerializeField] private CharacterExposerScript player;
        [SerializeField] private Vector3 initialPosition;
        [SerializeField] private Quaternion initialRotation;

        private float _timeSpend;
        [SerializeField] private Transform orbTransform;

        private void Awake()
        {
            if (!player)
            {
                gameObject.SetActive(false);
                return;
            }
            _timeSpend = 0.0f;
            initialPosition = orbTransform.localPosition;
            initialRotation = orbTransform.localRotation;
        }
        
        private void OnEnable()
        {
            if (!player)
            {
                gameObject.SetActive(false);
                return;
            }
            _timeSpend = 0.0f;
            initialPosition = orbTransform.localPosition;
            initialRotation = orbTransform.localRotation;
        }

        private void FixedUpdate()
        {
            _timeSpend += Time.deltaTime;
            _timeSpend %= (Mathf.PI * 2);

            orbTransform.RotateAround(player.CharacterTr.position, player.CharacterTr.up, 
                GameVariablesScript.Instance.orbDefensiveSpeed);
            
            orbTransform.Translate(new Vector3(0,Mathf.Cos(_timeSpend) * 
                                                Time.deltaTime/2,0),Space.Self);
        }

        private void OnDisable()
        {
            _timeSpend = 0.0f;
            orbTransform.position = initialPosition;
            orbTransform.rotation = initialRotation;
        }
    }
}
