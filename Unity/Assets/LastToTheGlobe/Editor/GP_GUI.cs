using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    public class GP_GUI : EditorWindow
    {
        public GP_GUISettings GUISettings { get; set; }

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Width : ");
            GUISettings.Scale = EditorGUILayout.IntField(GUISettings.Scale);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Depth : ");
            GUISettings.Depth = EditorGUILayout.IntField(GUISettings.Depth);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("DigRate : ");
            GUISettings.DigRate = EditorGUILayout.FloatField(GUISettings.DigRate);
            EditorGUILayout.EndHorizontal();
        }

    }
}