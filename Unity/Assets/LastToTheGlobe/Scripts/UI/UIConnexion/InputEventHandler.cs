using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI; // Required when Using UI elements.

public class InputEventHandler : MonoBehaviour
{
    public TMP_InputField mainInputField;
    [HideInInspector]
    public bool inputEntered = false;

    // Checks if there is anything entered into the input field.
    void LockInput(TMP_InputField input)
    {
        if (input.text.Length > 0)
        {
            inputEntered = true;
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