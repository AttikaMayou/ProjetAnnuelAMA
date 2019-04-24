using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTCameraMovement : MonoBehaviour
{
    [SerializeField]private Transform planetTransform;

    [SerializeField] private float speed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(planetTransform.position, Vector3.up, speed * Time.deltaTime);
    }
}
