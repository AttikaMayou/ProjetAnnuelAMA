using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine.UI; // Required when Using UI elements.

public class InputEventHandler : MonoBehaviour
{
    public TMP_InputField mainInputField;
    [HideInInspector] public bool inputEntered = false;
    [HideInInspector] public string textInput;

    // Checks if there is anything entered into the input field.
    void LockInput(TMP_InputField input)
    {
        if (input.text.Length > 0)
        {
            inputEntered = true;
            textInput = input.text;

        }
        else if (input.text.Length == 0)
        {
            inputEntered = false;
            Debug.Log("Main Input Empty");
        }
    }

    public void Start()
    {

        //Adds a listener that invokes the "LockInput" method when the player finishes editing the main input field.
        //Passes the main input field into the method when "LockInput" is invoked
        mainInputField.onEndEdit.AddListener(delegate {LockInput(mainInputField); });
    }

}