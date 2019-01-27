using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Auteur: Abdallah
public class OrbManager : MonoBehaviour {

    [SerializeField]private Rigidbody selfOrbRigibody;

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

        selfOrbRigibody.position = playerTransform.position + playerTransform.forward * 2f;
        //Direction = playerTransform.forward;
        transform.forward = playerTransform.forward;
        centerPointAttractor = attractedScript.attractor.selfTransform.position;
        
    }

    // Update is called once per frame
    private void Update () {
    transform.RotateAround(centerPointAttractor,playerTransform.right,1f);

    timeToDisable += Time.deltaTime;
        
        if(timeToDisable >= 2f)
        {
            timeToDisable = 0f;
            gameObject.SetActive(false);
        }
    }
}
