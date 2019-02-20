using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LastToTheGlobe.Scripts.Avatar;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private CharacterExposer playerExposer;
    public KeyCode defensiveOrbInput;
    private bool _canHyperJump;
    private float _cooldownFinished = 0.0f;

    private void Update()
    {
        if (Input.GetKeyDown(defensiveOrbInput) && _canHyperJump && _cooldownFinished <= 0f)
        {
            _cooldownFinished = 10f;
            playerExposer.characterRb.AddForce(transform.up * 1300f);
        }

        if (_cooldownFinished >= 0f)
        {
            _cooldownFinished -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Jumper"))
        {
            _canHyperJump = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Jumper"))
        {
            _canHyperJump = false;
        }

    }
}
