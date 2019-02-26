using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTintTextureTest : MonoBehaviour
{
    private Renderer rend;
    [SerializeField]
    private Color color = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = color;
    }

}
