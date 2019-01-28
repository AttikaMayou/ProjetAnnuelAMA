using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Auteur: Abdallah
public class OrbManager : MonoBehaviour {

    [SerializeField]private Transform selfPosition;

    [SerializeField]private float speed = 5f;

    [SerializeField]private Transform playerTransform;

    [SerializeField] private AttractedScript attractedScript;

    [SerializeField] private Vector3 centerPointAttractor;

    private Vector3 Direction;

    private float timeToDisable;

    // Use this for initialization
    void OnEnable()
    {
        timeToDisable = Time.deltaTime;
        selfPosition.position = playerTransform.position + playerTransform.forward * 2f;
        Direction = playerTransform.right;
        centerPointAttractor = attractedScript.attractor.selfTransform.position;
        
    }

    // Update is called once per frame
    private void FixedUpdate () {
    transform.RotateAround(centerPointAttractor,Direction,speed);

    timeToDisable += Time.deltaTime;
        
        if(timeToDisable >= 2f)
        {
            timeToDisable = 0f;
            gameObject.SetActive(false);
        }
    }
}
