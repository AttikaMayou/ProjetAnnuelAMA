using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveOrbAnimation : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float speed = 1f;

    private float actualTime = 0f;
    // Update is called once per frame
    void Update()
    {
        actualTime += Time.deltaTime;
        actualTime %= (Mathf.PI * 2);
        transform.RotateAround(playerTransform.position, playerTransform.up, speed);
        transform.Translate(new Vector3(0,Mathf.Cos(actualTime) * Time.deltaTime/2,0),Space.Self);
    }
}
