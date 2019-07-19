//Create a folder and call it "Editor" if one doesn't already exist. Place this script in it.

using UnityEngine;
using System.Collections;
using UnityEditor;

// Create a 180 degrees wire arc with a ScaleValueHandle attached to the disc
// lets you visualize some info of the transform

[CustomEditor(typeof(HandleExample))]
class LabelHandle : UnityEditor.Editor
{
    void OnSceneGUI()
    {
        HandleExample handleExample = (HandleExample)target;
        if (handleExample == null)
        {
            return;
        }

        Handles.color = Color.blue;
        Handles.Label(handleExample.transform.position + Vector3.up * 2,
            handleExample.transform.position.ToString() + "\nShieldArea: " +
            handleExample.shieldArea.ToString());

        Handles.BeginGUI();
        if (GUILayout.Button("Reset Area", GUILayout.Width(100)))
        {
            handleExample.shieldArea = 5;
        }
        Handles.EndGUI();


        Handles.DrawWireArc(handleExample.transform.position,
            handleExample.transform.up,
            -handleExample.transform.right,
            180,
            handleExample.shieldArea);
        handleExample.shieldArea =
            Handles.ScaleValueHandle(handleExample.shieldArea,
                handleExample.transform.position + handleExample.transform.forward * handleExample.shieldArea,
                handleExample.transform.rotation,
                1,
                Handles.ConeHandleCap,
                1);
    }
}