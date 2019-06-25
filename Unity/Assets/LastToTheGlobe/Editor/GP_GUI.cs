using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Editor
{
    public class GP_GUI : EditorWindow
    {
        public GP_GUISettings GUISettings { get; set; }
        private GameObject[] forPrefab;

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("WIP Scale : ");
            GUISettings.Scale = EditorGUILayout.IntField(GUISettings.Scale);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("WIP PlanetType : ");
            GUISettings.PlanetType = (PlanetType)EditorGUILayout.EnumPopup(GUISettings.PlanetType);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("WIP Depth : ");
            GUISettings.Depth = EditorGUILayout.IntField(GUISettings.Depth);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Generate planet"))
            {
                GeneratePlanet();
            }

           if (GUILayout.Button("Save planet "))
            {
                CreatePrefab();
            }

            if (GUILayout.Button("WIP Create another planet "))
            {
                ResetScene();
            }
            
            EditorGUILayout.EndVertical();
        }

        public void Awake()
        {
            forPrefab = new GameObject[200];
        }

        public void GeneratePlanet()
        {
            //Create planet sphere and set scale
            var planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            forPrefab[0] = planet;
            planet.transform.localScale = new Vector3(GUISettings.Scale, GUISettings.Scale, GUISettings.Scale);

            Undo.RegisterCreatedObjectUndo(planet, "Create planets");
        }
        
        public void CreatePrefab()
        {
            for (int i = 0; i < forPrefab.Length; i++)
            {
                //Debug.Log("[GP_TOOL] tableau de gameobject :" + forPrefab[i].name);
            }

            foreach (GameObject gameObject in forPrefab)
            {
                //Set the path as within the ressources folder
                string localPath = "Assets/Resources" + gameObject.name + ".prefab";
 
                //Check if the Prefab and/or name already exists at the path
                if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)))
                {
                    //Create dialog to ask if User is sure they want to overwrite existing Prefab
                    if (EditorUtility.DisplayDialog("Are you sure?",
                        "The Prefab already exists. Do you want to overwrite it?",
                        "Yes",
                        "No"))
                    //If the user presses the yes button, create the Prefab
                    {
                        CreateNew(gameObject, localPath);
                    }
                }
                //If the name doesn't exist, create the new Prefab
                else
                {
                    Debug.Log(gameObject.name + " is not a Prefab, will convert");
                    CreateNew(gameObject, localPath);
                }
            }
        }
        
        public void CreateNew(GameObject planet, string localPath)
        {
            bool success = false;
            //Create a new Prefab at the path given
            PrefabUtility.SaveAsPrefabAsset(planet, localPath, out success);
        }

        //reset object
        public void ResetScene()
        {
            Debug.Log("reset");
            //TODO : supprimer prefab + vider forPrefab[]
        }
        

    }
}