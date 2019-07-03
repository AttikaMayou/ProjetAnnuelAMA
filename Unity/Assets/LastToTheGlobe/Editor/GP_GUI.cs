using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Auteur : Margot
namespace LastToTheGlobe.Editor
{
    public class GP_GUI : EditorWindow
    {
        public GP_GUISettings GUISettings { get; set; }
        private List<GameObject> forPrefab = new List<GameObject>();
        private GameObject planet;
        private Material mat;
        private int compteur = 0;

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.HelpBox("\n                                PLANET GENERATOR                            \n\n" +
                "This is a tool to quickly generate a planet with custom parameters. \nRefer to the instructions below.\n", MessageType.None);

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
            EditorGUILayout.LabelField("Number of trees : ");
            GUISettings.NumberOfTree = EditorGUILayout.IntField(GUISettings.NumberOfTree);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Number of rocks : ");
            GUISettings.NumberOfRock = EditorGUILayout.IntField(GUISettings.NumberOfRock);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Number of chests : ");
            GUISettings.NumberOfChest = EditorGUILayout.IntField(GUISettings.NumberOfChest);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Generate planet"))
            {
                GeneratePlanet();
                compteur++;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Prefab Name : ");
            GUISettings.Name = EditorGUILayout.TextField(GUISettings.Name);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Number of object to add : ");
            GUISettings.NumberOfgameObjectAdd = EditorGUILayout.IntField(GUISettings.NumberOfgameObjectAdd);
            EditorGUILayout.EndHorizontal();

            if(GUISettings.NumberOfgameObjectAdd > 5)
            {
                GUISettings.NumberOfgameObjectAdd = 5;
            }
            
            if (GUISettings.NumberOfgameObjectAdd > 0)
            {
                for (int i = 0; i < GUISettings.NumberOfgameObjectAdd; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Add gameObject to prefab : ");
                    GUISettings.GameObjectAdd[i] = (GameObject)EditorGUILayout.ObjectField(GUISettings.GameObjectAdd[i], typeof(GameObject), true);
                    EditorGUILayout.EndHorizontal();
                }
            }
            
            if (GUILayout.Button("Save planet "))
            {
                CreatePrefab(GUISettings.NumberOfTree, GUISettings.NumberOfChest, GUISettings.NumberOfRock, planet, GUISettings.NumberOfgameObjectAdd);
            }

            if (GUILayout.Button("Erase planet "))
            {
                ResetScene();
            }

            if (GUILayout.Button("Create another planet "))
            {
                CreateAnotherPlanet(planet, compteur);
                ClearList();
            }

            EditorGUILayout.HelpBox("Instructions : \n\nTo create a planet, choose its scale, type and numbers of assets to instanciate. Then press" +
                "Generate Planet button.\n\nIf you want to add some custom object, add them to Number Of Object To Add field.\n\n" +
                "When you're satisfied with the created planet, you can save it with Save Planet button and create another one.\n\n" +
                "If you want to erase it, press Erase Planet button.", MessageType.None);

            //-----------------------------------------------EXCEPTION------------------------------------------------
            if (GUISettings.PlanetType == PlanetType.Crystal && GUISettings.NumberOfTree > 0)
            {
                EditorGUILayout.HelpBox("Can't grow any tree on a crystal planet ! \n Please, put 0", MessageType.Warning);
            }

            if (GUISettings.NumberOfgameObjectAdd >= 5)
            {
                EditorGUILayout.HelpBox("You can't add more than 5 GameObjects !", MessageType.Warning);
            }

            if (GUISettings.PlanetType != PlanetType.Basic && GUISettings.SpawnPlanet == true)
            {
                EditorGUILayout.HelpBox("Spawn planet can't be another type than basic !", MessageType.Warning);
            }

            EditorGUILayout.EndVertical();
        }

        public void GeneratePlanet()
        {
            if (GUISettings.SpawnPlanet == true)
            {
                planet = Instantiate(Resources.Load("SM_SpawnPlanet", typeof(GameObject))) as GameObject;
                forPrefab.Insert(0, planet);
            }
            else
            {
                //Create planet sphere and set scale
                planet = Instantiate(Resources.Load("SM_Planet", typeof(GameObject))) as GameObject;
                forPrefab.Insert(0, planet);
                Undo.RegisterCreatedObjectUndo(planet, "Create planets");
                //-------------------------------------SCALE------------------------------------------------
                planet.transform.localScale = new Vector3(GUISettings.Scale, GUISettings.Scale, GUISettings.Scale);
                //----------------------------------PLANET TYPE---------------------------------------------
                switch (GUISettings.PlanetType)
                {
                    //Planet Material
                    case PlanetType.Frozen:
                        {
                            mat = Resources.Load("M_FrozenPlanet", typeof(Material)) as Material;
                            break;
                        }
                    case PlanetType.Crystal:
                        {
                            mat = Resources.Load("M_CrystalPlanet", typeof(Material)) as Material;
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
                //----------------------------------MATERIAL------------------------------------------------
                //planet.GetComponentsInChildren<Renderer>().GetComponent<Renderer>().material = mat;
                planet.GetComponentInChildren<Renderer>().material = mat;
            }
            //-----------------------------------SPAWN CHEST-----------------------------------------------
            SpawnChest(GUISettings.NumberOfChest, planet);

            //-----------------------------------SPAWN TREES-----------------------------------------------
            SpawnTrees(GUISettings.NumberOfChest, GUISettings.NumberOfTree, planet, GUISettings.PlanetType);

            //-----------------------------------SPAWN ROCKS-----------------------------------------------
            SpawnRocks(GUISettings.NumberOfTree, GUISettings.NumberOfRock, planet, GUISettings.PlanetType);


        }

        public void CreatePrefab(int numberOfTrees, int numberOfChest, int newRocks, GameObject planet, int NumberOfgameObjectAdd)
        {
            if (GUISettings.GameObjectAdd != null)
            {
                for (int i = 0; i < NumberOfgameObjectAdd; i++)
                {
                    forPrefab.Insert(newRocks + numberOfChest + numberOfTrees + 1, GUISettings.GameObjectAdd[i] as GameObject);
                    forPrefab[newRocks + numberOfChest + numberOfTrees + 1].transform.SetParent(planet.transform);
                }
            }
            
            foreach (Object gameObject in forPrefab)
            {
                //Set the path as within the ressources folder
                string localPath = "Assets/Resources/" + GUISettings.Name + ".prefab";
                PrefabUtility.SaveAsPrefabAsset(planet, localPath);
            }
        }

        public void ResetScene()
        {
            for (int i = 0; i < forPrefab.Count; i++)
            {
                DestroyImmediate(forPrefab[i]);
            }
            ClearList();
        }

        public void ClearList()
        {
            forPrefab.Clear();
        }

        public void CreateAnotherPlanet(GameObject planet, int compteur)
        {
            GameObject[] tabPlanet = new GameObject[200];
            int offset = 100;
            planet.transform.localPosition = new Vector3(offset * compteur, 0, 0);

        }

        private void SpawnChest(int numberOfChest, GameObject planet)
        {
            for (int i = 1; i <= numberOfChest; i++)
            {
                GameObject newChest = PrefabUtility.InstantiatePrefab(Resources.Load("Chest", typeof(GameObject))) as GameObject;
                var spawnPosition = new Vector3(0, 0, 0);

                if (GUISettings.SpawnPlanet == true)
                {
                    planet = GameObject.Find("polySurface1");
                    spawnPosition = Random.onUnitSphere * (planet.transform.localScale.x + newChest.transform.localScale.y + 2.1f );
                }
                else
                {
                    spawnPosition = Random.onUnitSphere * (planet.transform.localScale.x + 0.41f);
                }
                newChest.transform.position = spawnPosition;
                newChest.transform.LookAt(planet.transform.position);
                newChest.transform.Rotate(-90, 0, 0);
                newChest.transform.SetParent(planet.transform);
                forPrefab.Insert(i, newChest);
            }
        }

        private void SpawnTrees(int numberOfChest, int numberOfTrees, GameObject planet, PlanetType type)
        {
            string prefabTree = "Tree_" + type + "_0";
            int randomBasicTrees;
            GameObject newTree;

            for (int i = 1; i <= GUISettings.NumberOfTree; i++)
            {

                randomBasicTrees = Random.Range(1, 5);
                var spawnPosition = new Vector3(0, 0, 0);

                newTree = PrefabUtility.InstantiatePrefab(Resources.Load(prefabTree + randomBasicTrees, typeof(GameObject))) as GameObject;

                if (GUISettings.SpawnPlanet == true)
                {
                    planet = GameObject.Find("polySurface1");
                    spawnPosition = Random.onUnitSphere * (planet.transform.localScale.x + newTree.transform.localScale.y + 11.65f);
                }
                else
                {
                    spawnPosition = Random.onUnitSphere * ((planet.transform.localScale.x + newTree.transform.localScale.y)) + planet.transform.position;
                }
                newTree.transform.position = spawnPosition;
                newTree.transform.LookAt(planet.transform.position);
                newTree.transform.Rotate(-90, 0, 0);
                newTree.transform.SetParent(planet.transform);
                forPrefab.Insert(i + numberOfChest, newTree);
            }
        }

        private void SpawnRocks(int numberOfTrees, int numberOfRocks, GameObject planet, PlanetType type)
        {
            string prefabRock = "P_Rock" + type + "_v";
            int randomBasicRocks;
            GameObject newRocks;
            Vector3 spawnPosition;

            for (int i = 1; i <= GUISettings.NumberOfRock; i++)
            {
                randomBasicRocks = Random.Range(1, 7);

                newRocks = PrefabUtility.InstantiatePrefab(Resources.Load(prefabRock + randomBasicRocks, typeof(GameObject))) as GameObject;

                if (GUISettings.SpawnPlanet == true)
                {
                    planet = GameObject.Find("polySurface1");
                    spawnPosition = Random.onUnitSphere * (planet.transform.localScale.x + newRocks.transform.localScale.y + 11.5f);
                }
                else
                {
                    spawnPosition = Random.onUnitSphere * (planet.transform.localScale.x + newRocks.transform.localScale.y) + planet.transform.position;
                }

                newRocks.transform.position = spawnPosition;
                newRocks.transform.LookAt(planet.transform.position);
                newRocks.transform.Rotate(-90, 0, 0);
                newRocks.transform.SetParent(planet.transform);
                forPrefab.Insert(i + numberOfTrees, newRocks);
            }
        }
    }
}