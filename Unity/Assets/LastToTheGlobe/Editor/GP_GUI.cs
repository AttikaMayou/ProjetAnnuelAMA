using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LastToTheGlobe.Editor;

//Auteur : Margot
namespace Editor
{
    public class GP_GUI : EditorWindow
    {
        public GP_GUISettings GUISettings { get; set; }
        private List<GameObject> forPrefab = new List<GameObject>();
        private GameObject planet;
        private Material mat;
        private GameObject spawnPlanet;
        private GP_AssetInstanciate spawnAsset;
        private GameObject newTree;
        private int randomBasicTrees;

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Is a spawn planet ? : ");
            GUISettings.SpawnPlanet = EditorGUILayout.Toggle(GUISettings.SpawnPlanet);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Scale : ");
            GUISettings.Scale = EditorGUILayout.IntField(GUISettings.Scale);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("PlanetType : ");
            GUISettings.PlanetType = (PlanetType)EditorGUILayout.EnumPopup(GUISettings.PlanetType);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("WIP Number of trees : ");
            GUISettings.NumberOfTree = EditorGUILayout.IntField(GUISettings.NumberOfTree);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("WIP Number of rock : ");
            GUISettings.NumberOfRock = EditorGUILayout.IntField(GUISettings.NumberOfRock);
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

        public void GeneratePlanet()
        {
            if(GUISettings.SpawnPlanet == true)
            {
                spawnPlanet = Instantiate(Resources.Load("SM_SpawnPlanet", typeof(GameObject))) as GameObject;
                forPrefab.Insert(0, planet);
            }
            else
            {
                //Create planet sphere and set scale
                planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                forPrefab.Insert(0, planet);
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
/*
                randomBasicTrees = Random.Range(1, 4);

                for (int i = 1; i <= GUISettings.NumberOfTree; i++)
                {
                    //_randomScaleTree = Random.Range(0.05f, 0.15f)
                    
                    newTree = Instantiate(Resources.Load("SM_Basic_Tree_0" + randomBasicTrees, typeof(GameObject))) as GameObject;
                    var spawnPosition = Random.onUnitSphere * (((planet.transform.localScale.x / 2) - 0.2f) + newTree.transform.localScale.y - 0.1f) + planet.transform.position;
                    newTree.transform.position = spawnPosition;
                    //newTree.transform.position = new Vector3(0, 0, 0);
                    newTree.transform.LookAt(planet.transform.position);
                    //newTree.transform.localScale = new Vector3(_randomScaleTree, _randomScaleTree, _randomScaleTree);
                    newTree.transform.Rotate(-90, 0, 0);
                    //newTree.transform.parent = transform;
                }*/
            }
            //SpawnObject
            spawnAsset.CreateAssets(GUISettings.NumberOfTree, GUISettings.NumberOfRock, GUISettings.PlanetType, planet);

            //Spawn Trees
            
        }


        public void CreatePrefab()
        {
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
            string list = forPrefab[0].name;
            Debug.Log("reset");
            Debug.Log(forPrefab[0].name);
            DestroyImmediate(forPrefab[0]);
            forPrefab.Clear();
            Debug.Log("liste après: " + list);
            //TODO : vérifier la liste vidée
        }
       
    }
}