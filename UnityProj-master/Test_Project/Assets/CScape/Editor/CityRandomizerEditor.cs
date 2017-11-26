using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;
using CScape;
using UnityEngine.Rendering;
//using UnityEditor;

namespace CScape
{

    [CustomEditor(typeof(CityRandomizer))]
    [CanEditMultipleObjects]

    public class CityRandomizerEditor : Editor
    {
        private Texture banner;
        public bool configurePrefab = false;
        public bool randomSettings;
        public bool streetLayout;
        public bool buildingTemplates;
        public bool rooftopTemplates;
        public bool streetTemplates;
        public bool streetDetailTemplates;
        public bool streetLightsTemplates;
        public bool streetFoliageTemplates;
        public bool folliageTemplates;
        public bool scanPrefabs;
        public bool citySize;
        public bool collidersSetup;
        bool checkConfiguration = false;


        void OnEnable()
        {
          //  CityRandomizer ce = (CityRandomizer)target;
            banner = Resources.Load("CSHeader") as Texture;
            if (System.IO.File.Exists(Application.dataPath + "/CSconfigured.txt")) checkConfiguration = true;
            

            }

        public override void OnInspectorGUI()
        {
            CityRandomizer ce = (CityRandomizer)target;
            GUILayout.Box(banner, GUILayout.ExpandWidth(true));
            if (PlayerSettings.colorSpace == ColorSpace.Gamma)
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label("WARNING! For best visual appearance");
                GUILayout.Label("switch Your project to Linear Color Space");
                if (GUILayout.Button("OK! Switch!"))
                {
                    PlayerSettings.colorSpace = ColorSpace.Linear;
                    PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel18;
                    EditorApplication.ExecuteMenuItem("Edit/Graphics Emulation/No Emulation");
                }
                GUILayout.EndVertical();
            }
            if (!checkConfiguration)
            {
                
                
                    
                    GUILayout.BeginVertical("box");
                    GUILayout.Label("WARNING! Set Graphics API build settings:");
                    GUILayout.Label("1. Android -  Graphics API change to OpenGLES3.0");
                    GUILayout.Label("2. iOS -  Graphics API change to Metal");
                    GUILayout.Label("3. Win Standalone -  Graphics API change to DX11");
                    GUILayout.Label("4. MacOS -  Graphics API change to OpenGLCore");

                    if (GUILayout.Button("Set Build Settings"))
                    {
                        GraphicsDeviceType[] apis = { GraphicsDeviceType.Direct3D11, GraphicsDeviceType.Direct3D12 };
                        GraphicsDeviceType[] apisIOS = { GraphicsDeviceType.Metal };
                        GraphicsDeviceType[] apisAndroid = { GraphicsDeviceType.OpenGLES3 };
                        GraphicsDeviceType[] apisMacOS = { GraphicsDeviceType.OpenGLCore };
                        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.StandaloneWindows, false);
                        PlayerSettings.SetGraphicsAPIs(BuildTarget.StandaloneWindows, apis);

                        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
                        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, apisAndroid);

                        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.iOS, false);
                        PlayerSettings.SetGraphicsAPIs(BuildTarget.iOS, apisIOS);

                        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.StandaloneOSXIntel64, false);
                        PlayerSettings.SetGraphicsAPIs(BuildTarget.StandaloneOSXIntel64, apisMacOS);

                        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.StandaloneOSXIntel, false);
                        PlayerSettings.SetGraphicsAPIs(BuildTarget.StandaloneOSXIntel, apisMacOS);
                    writeConfToFile();
                    checkConfiguration = true;
                }
                    GUILayout.EndVertical();
                

            }
            GUILayout.BeginVertical("box");
            randomSettings = EditorGUILayout.Foldout(randomSettings, "Random settings", true);
            if (randomSettings)
            {
                GUILayout.BeginHorizontal();
                ce.height = EditorGUILayout.Toggle(ce.height);
                ce.minFloors = EditorGUILayout.IntField("Floors", ce.minFloors);
                ce.maxFloors = EditorGUILayout.IntField("", ce.maxFloors);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                ce.width = EditorGUILayout.Toggle(ce.width);
                ce.minWidth = EditorGUILayout.IntField("Width", ce.minWidth);
                ce.maxWidth = EditorGUILayout.IntField("", ce.maxWidth);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                ce.depth = EditorGUILayout.Toggle(ce.depth);
                ce.minDepth = EditorGUILayout.IntField("Depth", ce.minDepth);
                ce.maxDepth = EditorGUILayout.IntField("", ce.maxDepth);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                ce.faccadeStyles = EditorGUILayout.Toggle(ce.faccadeStyles);
                ce.minMatIndex = EditorGUILayout.IntField("Mat Index", ce.minMatIndex);
                ce.maxMatIndex = EditorGUILayout.IntField("", ce.maxMatIndex);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                ce.faccadeStyles = EditorGUILayout.Toggle(ce.faccadeStyles);
                ce.minMatIndex1 = EditorGUILayout.IntField("Mat Index", ce.minMatIndex1);
                ce.maxMatIndex1 = EditorGUILayout.IntField("", ce.maxMatIndex1);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                ce.faccadeStyles = EditorGUILayout.Toggle(ce.faccadeStyles);
                ce.minMatIndex2 = EditorGUILayout.IntField("Mat Index", ce.minMatIndex2);
                ce.maxMatIndex2 = EditorGUILayout.IntField("", ce.maxMatIndex2);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                ce.faccadeStyles = EditorGUILayout.Toggle(ce.faccadeStyles);
                ce.minMatIndex4 = EditorGUILayout.IntField("Mat Index", ce.minMatIndex4);
                ce.maxMatIndex4 = EditorGUILayout.IntField("", ce.maxMatIndex4);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                ce.openWindow = EditorGUILayout.Toggle(ce.openWindow);
                ce.minWindowOpen = EditorGUILayout.FloatField("Window open", Mathf.Ceil(ce.minWindowOpen));
                ce.maxWindowOpen = EditorGUILayout.FloatField("", Mathf.Ceil(ce.maxWindowOpen));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                ce.openWindow = EditorGUILayout.Toggle(ce.openWindow);
                ce.streetSizeX = EditorGUILayout.IntField("Street Size X", ce.streetSizeX);
                ce.streetSizeXmax = EditorGUILayout.IntField("", ce.streetSizeXmax);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                ce.openWindow = EditorGUILayout.Toggle(ce.openWindow);
                ce.streetSizeZ = EditorGUILayout.IntField("Street Size Z", ce.streetSizeZ);
                ce.streetSizeZmax = EditorGUILayout.IntField("", ce.streetSizeZmax);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                ce.openWindow = EditorGUILayout.Toggle(ce.openWindow);
                ce.sidewalkSizeX = EditorGUILayout.IntField("Sidewalk Size X", ce.sidewalkSizeX);
                ce.sidewalkSizeXmax = EditorGUILayout.IntField("", ce.sidewalkSizeXmax);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                ce.openWindow = EditorGUILayout.Toggle(ce.openWindow);
                ce.sidewalkSizeZ = EditorGUILayout.IntField("Sidewalk Size Z", ce.sidewalkSizeZ);
                ce.sidewalkSizeZmax = EditorGUILayout.IntField("", ce.sidewalkSizeZmax);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                ce.openWindow = EditorGUILayout.Toggle(ce.openWindow);
                ce.folliageThreshold = EditorGUILayout.IntField("Folliage Thresshold", ce.folliageThreshold);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                ce.sectionDivisionMin = EditorGUILayout.IntField("Section Division Min/Max", ce.sectionDivisionMin);
                ce.sectionDivisionMax = EditorGUILayout.IntField(ce.sectionDivisionMax);
                GUILayout.EndHorizontal();

            }


            GUILayout.EndVertical();

            ///city size layout
            GUILayout.BeginVertical("box");
            citySize = EditorGUILayout.Foldout(citySize, "City Size", true);
            if (citySize)
            {
                GUILayout.BeginVertical();
                ce.numberOfBuildingsX = EditorGUILayout.IntField("Number of blocks X", ce.numberOfBuildingsX);
                ce.numberOfBuildingsZ = EditorGUILayout.IntField("Number of blocks Z", ce.numberOfBuildingsZ);
                ce.blockDistances = EditorGUILayout.IntField("Min Block distances", ce.blockDistances);
                ce.maxBlockDistances = EditorGUILayout.IntField("Max Block distances", ce.maxBlockDistances);
                ce.riverPosition = EditorGUILayout.IntField("River Position", ce.riverPosition);
                if(GUILayout.Button("Center river"))
                {
                    ce.riverPosition = ce.numberOfBuildingsX / 2;
                }

                EditorGUI.BeginChangeCheck();
                ce.cityCurve = EditorGUILayout.CurveField("Heights", ce.cityCurve);
                if (EditorGUI.EndChangeCheck())
                {
                    ce.UpdateHeights();
                    ce.Refresh();
                }
                //  ce.cityCurve = EditorGUILayout.CurveField("Heights", ce.cityCurve);


                GUILayout.EndVertical();

            }


            GUILayout.EndVertical();

            /// colliders setup
            GUILayout.BeginVertical("box");
            collidersSetup = EditorGUILayout.Foldout(collidersSetup, "Colliders Setup", true);
            if (collidersSetup)
            {

                ce.useMeshCollider = EditorGUILayout.Toggle("Use mesh collider", ce.useMeshCollider);
                if (ce.useMeshCollider) ce.useConvexCollider = EditorGUILayout.Toggle("Use Convex collider", ce.useConvexCollider);
                if (GUILayout.Button("Update Colliders"))
                {
                    ce.updateColliders = true;
                }
            }


            GUILayout.EndVertical();

            //street layout
            GUILayout.BeginVertical("Box");
            streetLayout = EditorGUILayout.Foldout(streetLayout, "Night Lights", true);
            if (streetLayout)
            {
                GUILayout.BeginVertical("Box");
                for (int i = 0; i < ce.nightColors.Length; i++)
                {
                    ce.nightColors[i] = EditorGUILayout.ColorField("Col " + i, ce.nightColors[i]);
                }
                GUILayout.BeginHorizontal("Box");
                if (GUILayout.Button("-", "Label", GUILayout.Width(20), GUILayout.Height(15)))
                    System.Array.Resize(ref ce.nightColors, ce.nightColors.Length - 1);
                if (GUILayout.Button("+", "Label", GUILayout.Width(20), GUILayout.Height(15)))
                    System.Array.Resize(ref ce.nightColors, ce.nightColors.Length + 1);
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();

            }


            GUILayout.EndVertical();


            ///choose possible prefab building tops
            GUILayout.BeginVertical("Box");
            rooftopTemplates = EditorGUILayout.Foldout(rooftopTemplates, "Rooftop templates", false);
            if (rooftopTemplates)
            {
                GUILayout.BeginVertical();
                for (int i = 0; i < ce.rooftopPrefabs.Length; i++)
                {
                    ce.rooftopPrefabs[i] = EditorGUILayout.ObjectField("Template " + i, ce.rooftopPrefabs[i], typeof(GameObject), true) as GameObject;
                }
                GUILayout.BeginHorizontal("Box");
                if (GUILayout.Button("-", "Label", GUILayout.Width(20), GUILayout.Height(15)))
                    System.Array.Resize(ref ce.rooftopPrefabs, ce.rooftopPrefabs.Length - 1);
                if (GUILayout.Button("+", "Label", GUILayout.Width(20), GUILayout.Height(15)))
                    System.Array.Resize(ref ce.rooftopPrefabs, ce.rooftopPrefabs.Length + 1);
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Scan Templates"))
                {

                    DirectoryInfo dir = new DirectoryInfo("Assets/CScape/BuildingTemplates");
                    FileInfo[] info = dir.GetFiles("*.prefab");
                    for (int j = 0; j < info.Length; j++)
                    {
                        System.Array.Resize(ref ce.rooftopPrefabs, info.Length);
                        Debug.Log(info[j] + "");
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();

            ///choose possible prefab buildings
            GUILayout.BeginVertical("Box");
            buildingTemplates = EditorGUILayout.Foldout(buildingTemplates, "Building templates", false);
            if (buildingTemplates)
            {
                GUILayout.BeginVertical();
                for (int i = 0; i < ce.prefabs.Length; i++)
                {
                    ce.prefabs[i] = EditorGUILayout.ObjectField("Template " + i, ce.prefabs[i], typeof(GameObject), true) as GameObject;
                }
                GUILayout.BeginHorizontal("Box");
                if (GUILayout.Button("-", "Label", GUILayout.Width(20), GUILayout.Height(15)))
                    System.Array.Resize(ref ce.prefabs, ce.prefabs.Length - 1);
                if (GUILayout.Button("+", "Label", GUILayout.Width(20), GUILayout.Height(15)))
                    System.Array.Resize(ref ce.prefabs, ce.prefabs.Length + 1);
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Load All Templates"))
                {
                    ce.prefabs = Resources.LoadAll<GameObject>("BuildingTemplates/Buildings");
                }
                GUILayout.EndVertical();

            }
            GUILayout.EndVertical();

            //Choose Possible streets
            GUILayout.BeginVertical("Box");
            streetTemplates = EditorGUILayout.Foldout(streetTemplates, "Street templates", false);
            if (streetTemplates)
            {
                GUILayout.BeginVertical();
                for (int i = 0; i < ce.buildPrefabs.Length; i++)
                {
                    ce.buildPrefabs[i] = EditorGUILayout.ObjectField("Template " + i, ce.buildPrefabs[i], typeof(GameObject), true) as GameObject;
                }
                GUILayout.BeginHorizontal("Box");
                if (GUILayout.Button("-", "Label", GUILayout.Width(20), GUILayout.Height(15)))
                    System.Array.Resize(ref ce.buildPrefabs, ce.buildPrefabs.Length - 1);
                if (GUILayout.Button("+", "Label", GUILayout.Width(20), GUILayout.Height(15)))
                    System.Array.Resize(ref ce.buildPrefabs, ce.buildPrefabs.Length + 1);
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Load Templates"))
                {
                    ce.buildPrefabs = Resources.LoadAll<GameObject>("BuildingTemplates/Streets");
                    //DirectoryInfo dir = new DirectoryInfo("Assets/CScape/BuildingTemplates/Streets");
                    //FileInfo[] info = dir.GetFiles("*.prefab");
                    //for (int j = 0; j < info.Length; j++)
                    //{
                    //    System.Array.Resize(ref ce.streetPrefabs, info.Length);
                    //    Debug.Log(info[j] + "");
                    //}
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();

            //Choose Possible street details
            GUILayout.BeginVertical("Box");
            streetDetailTemplates = EditorGUILayout.Foldout(streetDetailTemplates, "Street Detail templates", false);
            if (streetDetailTemplates)
            {
                GUILayout.BeginVertical();
                for (int i = 0; i < ce.streetDetailPrefabs.Length; i++)
                {
                    ce.streetDetailPrefabs[i] = EditorGUILayout.ObjectField("Template " + i, ce.streetDetailPrefabs[i], typeof(GameObject), true) as GameObject;
                }
                GUILayout.BeginHorizontal("Box");
                if (GUILayout.Button("-", "Label", GUILayout.Width(20), GUILayout.Height(15)))
                    System.Array.Resize(ref ce.streetDetailPrefabs, ce.streetDetailPrefabs.Length - 1);
                if (GUILayout.Button("+", "Label", GUILayout.Width(20), GUILayout.Height(15)))
                    System.Array.Resize(ref ce.streetDetailPrefabs, ce.streetDetailPrefabs.Length + 1);
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Scan Templates"))
                {

                    DirectoryInfo dir = new DirectoryInfo("Assets/CScape/BuildingTemplates/Details");
                    FileInfo[] info = dir.GetFiles("*.prefab");
                    for (int j = 0; j < info.Length; j++)
                    {
                        System.Array.Resize(ref ce.streetDetailPrefabs, info.Length);
                        Debug.Log(info[j] + "");
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
            //Choose possible light templates
            GUILayout.BeginVertical("Box");
            streetLightsTemplates = EditorGUILayout.Foldout(streetLightsTemplates, "Street Light templates", false);
            if (streetLightsTemplates)
            {
                GUILayout.BeginVertical();
                for (int i = 0; i < ce.streetLightsPrefabs.Length; i++)
                {
                    ce.streetLightsPrefabs[i] = EditorGUILayout.ObjectField("Template " + i, ce.streetLightsPrefabs[i], typeof(GameObject), true) as GameObject;
                }
                GUILayout.BeginHorizontal("Box");
                if (GUILayout.Button("-", "Label", GUILayout.Width(20), GUILayout.Height(15)))
                    System.Array.Resize(ref ce.streetLightsPrefabs, ce.streetLightsPrefabs.Length - 1);
                if (GUILayout.Button("+", "Label", GUILayout.Width(20), GUILayout.Height(15)))
                    System.Array.Resize(ref ce.streetLightsPrefabs, ce.streetLightsPrefabs.Length + 1);
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Scan Templates"))
                {

                    DirectoryInfo dir = new DirectoryInfo("Assets/CScape/BuildingTemplates/Details");
                    FileInfo[] info = dir.GetFiles("*.prefab");
                    for (int j = 0; j < info.Length; j++)
                    {
                        System.Array.Resize(ref ce.streetLightsPrefabs, info.Length);
                        Debug.Log(info[j] + "");
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
            ///Choose possible folliage
            GUILayout.BeginVertical("Box");
            streetFoliageTemplates = EditorGUILayout.Foldout(streetFoliageTemplates, "Street Folliage templates", false);
            if (streetFoliageTemplates)
            {
                GUILayout.BeginVertical();
                for (int i = 0; i < ce.streetFoliagePrefabs.Length; i++)
                {
                    ce.streetFoliagePrefabs[i] = EditorGUILayout.ObjectField("Template " + i, ce.streetFoliagePrefabs[i], typeof(GameObject), true) as GameObject;
                }
                GUILayout.BeginHorizontal("Box");
                if (GUILayout.Button("-", "Label", GUILayout.Width(20), GUILayout.Height(15)))
                    System.Array.Resize(ref ce.streetFoliagePrefabs, ce.streetFoliagePrefabs.Length - 1);
                if (GUILayout.Button("+", "Label", GUILayout.Width(20), GUILayout.Height(15)))
                    System.Array.Resize(ref ce.streetFoliagePrefabs, ce.streetFoliagePrefabs.Length + 1);
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Scan Templates"))
                {

                    DirectoryInfo dir = new DirectoryInfo("Assets/CScape/BuildingTemplates/Details");
                    FileInfo[] info = dir.GetFiles("*.prefab");
                    for (int j = 0; j < info.Length; j++)
                    {
                        System.Array.Resize(ref ce.streetFoliagePrefabs, info.Length);
                        Debug.Log(info[j] + "");
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();





            ce.randomSeed = EditorGUILayout.IntField("Random seed", ce.randomSeed);

            if (GUILayout.Button("Generate City"))
            {

                ce.Generate();
                ce.GenerateStreets();
                ce.UpdateCity();


            }

            if (GUILayout.Button("Generate Streets"))
            {
                ce.DeleteStreets();
                ce.GenerateStreets();

            }

            if (GUILayout.Button("Add Street Details"))
            {
                ce.DeleteStreeetDetails();
                ce.GenerateDetails();
            //    ce.UpdateCity();
            }

            if (GUILayout.Button("Add Street Lights"))
            {
                ce.DeleteLights();
                ce.GenerateLights();
              //  ce.UpdateCity();
            }

            if (GUILayout.Button("Add Foliage"))
            {
                ce.DeleteFolliage();
                ce.GenerateFolliage();
           //     ce.UpdateCity();
            }
            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Randomize +1"))
            {
                ce.randomize = true;
                ce.Randomize();
                ce.randomSeed = ce.randomSeed + 1;
             //   ce.UpdateCity();
            }
            if (GUILayout.Button("Randomize -1"))
            {
                ce.randomize = true;
                ce.Randomize();
                ce.randomSeed = ce.randomSeed - 1;
              //  ce.UpdateCity();
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Update Colliders"))
            {
                ce.SetColliders();
            }

            if (GUILayout.Button("Delete Buildings"))
            {
                ce.delete = true;
                ce.UpdateCity();
            }

            if (GUILayout.Button("Strip CS Scripts"))
            {
                ce.StripScripts();
            }

            if (GUILayout.Button("Refresh City"))
            {
                ce.Refresh();


            }
            ce.usePOM = EditorGUILayout.Toggle("Use Parralax Mapping", ce.usePOM);
            if (ce.usePOM == true)
                Shader.EnableKeyword ("_CSCAPE_DESKTOP_ON");
            else Shader.DisableKeyword("_CSCAPE_DESKTOP_ON");


            if (GUI.changed)
            {
                EditorUtility.SetDirty(ce);
#if UNITY_5_4_OR_NEWER
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
#endif
            }

        }
        void writeConfToFile()
        {
            string txt = "Configured";
            System.IO.File.WriteAllText(Application.dataPath + "/CSconfigured.txt", txt);
        }

        void readConfFromFile()
        {
            string txt;
            if (System.IO.File.Exists(Application.dataPath + "/CSconfigured.txt"))
            {
                txt = System.IO.File.ReadAllText(Application.dataPath + "/CSconfigured.txt");
                Debug.Log(txt);
            }
            
            
        }
    }
}
