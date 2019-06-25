using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Auteur : Margot
namespace Editor
{
    public class GP_GUI : EditorWindow
    {
        public GP_GUISettings GUISettings { get; set; }
        private GameObject[] forPrefab;
        private GameObject planet;
        private Material mat;

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("WIP : Is a spawn planet ? : ");
            GUISettings.SpawnPlanet = EditorGUILayout.Toggle(GUISettings.SpawnPlanet);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Scale : ");
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

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Prefab Name : ");
            GUISettings.Name = EditorGUILayout.TextField(GUISettings.Name);
            EditorGUILayout.EndHorizontal();

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
            if(GUISettings.SpawnPlanet = true)
            {
               //TODO : spawn planet
            }
            else
            {
                //Create planet sphere and set scale
                planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                forPrefab[0] = planet;
                Undo.RegisterCreatedObjectUndo(planet, "Create planets");
                //SCALE
                planet.transform.localScale = new Vector3(GUISettings.Scale, GUISettings.Scale, GUISettings.Scale);

                //----------------------------------PLANET TYPE---------------------------------------------------
                switch (GUISettings.PlanetType)
                {
                    //Planet Material
                    case PlanetType.Frozen:
                        {
                            mat = Resources.Load("M_FrozenPlanet", typeof(Material)) as Material;
                            break;
                        }
                    case PlanetType.Desert:
                        {
                            mat = Resources.Load("M_DesertPlanet", typeof(Material)) as Material;
                            break;
                        }
                    case PlanetType.Basic:
                        {
                            mat = Resources.Load("M_BasicPlanet", typeof(Material)) as Material;
                            break;
                        }
                    default:
                        mat = Resources.Load("M_BasicPlanet", typeof(Material)) as Material;
                        break;
                }
                //MATERIAL
                planet.GetComponent<Renderer>().material = mat;
            }
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
                string localPath = "Assets/Resources/" + GUISettings.Name + ".prefab";
                CreateNew(gameObject, localPath);
           
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
            for (int i = 0; i < forPrefab.Length; i++)
            {
                System.Array.Clear(forPrefab, i, 200 - i);
                Debug.Log("reset");
            }
            Debug.Log("reset");
            //TODO : supprimer prefab + vider forPrefab[]
        }
       
    }
}