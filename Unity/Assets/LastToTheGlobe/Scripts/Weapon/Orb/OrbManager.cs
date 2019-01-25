using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour {

    [SerializeField]
    private Rigidbody selfOrbRigibody;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private Transform playerTransform;

    private Vector3 Direction;

    private float timeToDisable;

    // Use this for initialization
    void OnEnable()
    {
        timeToDisable = Time.deltaTime;

        selfOrbRigibody.position = playerTransform.position + playerTransform.forward;
        Direction = playerTransform.forward;
    }

    // Update is called once per frame
    void Update () {
        timeToDisable += Time.deltaTime;
        selfOrbRigibody.AddForce(Direction * speed);
        //selfOrbRigibody.MovePosition(selfOrbRigibody.position + transform.TransformDirection(Direction) * speed * Time.deltaTime);
        if(timeToDisable >= 2f)
        {
            timeToDisable = 0f;
            gameObject.SetActive(false);
        }
    }
}
